using Hl7.Fhir.Model;
using System;
using Hl7.Fhir.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Hl7.Fhir.Support;
using Hl7.Fhir.Introspection;
using Hl7.Fhir.Language.Debugging;
 using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Hl7.Fhir.Serialization;
using System.Collections;
using System.Text.RegularExpressions;
using System.Net.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Hl7.Fhir.Rest;
using System.Text;
using System.Xml.Serialization;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.FhirPath;
using Hl7.Fhir.Specification.Navigation;
using Hl7.FhirPath;
using Hl7.Fhir.ElementModel;
using Hl7.Fhir.Specification;
using System.Security.AccessControl;


namespace GlobalHelpers
{
    public class FHIRValidation
    {
        private static string serverUrl = "https://localhost:44348";
    private static FhirClient _fhirClient = new FhirClient(serverUrl);
    private static void UpdateToken(string token)
    {
        _fhirClient.RequestHeaders.Remove("Authorization");
        _fhirClient.RequestHeaders.Add("Authorization", $"Bearer {token}");
    }
    public static Resource GetStructureDefintion(string structureDefIdentifier, string token)
    {
        // string structureDefIdentifier = "us-core-" + resource.TypeName.ToLower();
        var url = $"https://localhost:44348/StructureDefintion/{structureDefIdentifier}";
        var locationUri = new Uri(url);

        UpdateToken(token);

        var readResponse = _fhirClient.ReadAsync<Resource>(locationUri).GetAwaiter().GetResult();
        return readResponse;
    }
    public static OperationOutcome ValidateResourceUSCDIV3(Resource resource, string token)
    {
        //var package1 = "../SolutionItems/package.tgz";

        string structureDefIdentifier = "StructureDefinition-us-core-" + resource.TypeName.ToLower();
        var uri = "http://hl7.org/fhir/us/core/StructureDefinition/" + structureDefIdentifier;

        StructureDefinition structDef = null;
        var structFromServer = GetStructureDefintion(structureDefIdentifier, token);


        var serializer = new FhirJsonSerializer();
        var resourceJson = serializer.SerializeToString(resource);

        // Parse the resource JSON into an ITypedElement
        var parser = new FhirJsonParser();
        var resourceElement = resource.ToTypedElement();
        var outcome = ValidateAgainstStructureDefinition(resource, structFromServer);

        return outcome;
    }

    public static OperationOutcome ValidateAgainstStructureDefinition(Resource resource, Resource structureDefinition)
    {
        // Initialize the operation outcome to collect validation issues
        var operationOutcome = new OperationOutcome();

        var validationContext = new ValidationContext(resource);
        var validationResults2 = structureDefinition.Validate(validationContext);
        ICollection<ValidationResult> validationResults = new List<ValidationResult>();
        var isValid = resource.TryValidate((ICollection<ValidationResult>?)validationResults, recurse: true, narrativeValidation: NarrativeValidationKind.FhirXhtml);

        // If validation fails, add validation issues to the operation outcome
        if (!isValid)
        {
            foreach (var validationResult in validationResults)
            {
                operationOutcome.AddIssue(new OperationOutcome.IssueComponent
                {
                    Severity = OperationOutcome.IssueSeverity.Error,
                    Code = OperationOutcome.IssueType.Invalid,
                    Diagnostics = validationResult.ErrorMessage
                });
            }
        }

        return operationOutcome;
    }

    //if (validationResults.Any())
    //{
    //    foreach (var validationResult in validationResults)
    //    {
    //        operationOutcome.AddIssue(new OperationOutcome.IssueComponent
    //        {
    //            Severity = OperationOutcome.IssueSeverity.Error,
    //            Code = OperationOutcome.IssueType.Invalid,
    //            Diagnostics = validationResult.ErrorMessage
    //        });
    //    }
    //}

    //return operationOutcome;

    //var navigator = new ElementDefinitionNavigator(structureDefinition);

    //navigator.MoveToFirstChild();
    //navigator.MoveToNext();
    //var outcome = new OperationOutcome();

    //while (navigator.MoveToNext())
    //{
    //    string elementPath = navigator.Path;

    //    var elementInResource = resource.Select(elementPath);

    //    if (elementInResource is FhirString stringElement)
    //    {
    //        if (navigator.Current.Min != null && navigator.Current.Max != null)
    //        {
    //            int minLength = (int)navigator.Current.Min;
    //            int maxLength = int.Parse(navigator.Current.Max);

    //            if (stringElement.Value.Length < minLength || stringElement.Value.Length > maxLength)
    //            {
    //                string errorMessage = $"String length must be between {minLength} and {maxLength} characters.";
    //                outcome.Issue.Add(new OperationOutcome.IssueComponent
    //                {
    //                    Severity = OperationOutcome.IssueSeverity.Error,
    //                    Code = OperationOutcome.IssueType.Invalid,
    //                    Diagnostics = errorMessage
    //                });
    //            }
    //        }
    //    }
    //    // Validate the element against the profile constraints
    //    // Perform your validation logic here...
    //}

    //return outcome;


    public static async Task<OperationOutcome> ValidateResourceServer(Resource resource, string mode = "no action", string profile = null)
    {
        HttpClient _httpClient = new HttpClient();
        var operationOutcome = new OperationOutcome();
        _httpClient.BaseAddress = new Uri("https://localhost:44348/");
        var jsonParser = new FhirJsonParser();

        FhirXmlSerializer _xmlSerializer = new FhirXmlSerializer();

        try
        {
            string validateUrl = $"{resource.TypeName}/$validate";

            if (!string.IsNullOrEmpty(profile))
            {
                validateUrl += $"?profile={profile}";
            }

            var xml = _xmlSerializer.SerializeToString(resource);
            var content = new StringContent(xml, Encoding.UTF8, "application/fhir+xml");

            // Include the mode parameter if provided
            //if (!string.IsNullOrEmpty(mode))
            //{
            //    // Check if the validateUrl already contains query parameters
            //    if (validateUrl.Contains("?"))
            //    {
            //        validateUrl += $"&mode={mode}";
            //    }
            //    else
            //    {
            //        validateUrl += $"?mode={mode}";
            //    }
            //}

            var response = await _httpClient.PostAsync(validateUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                operationOutcome = jsonParser.Parse<OperationOutcome>(responseContent);

                if (operationOutcome.Issue.Any(issue => issue.Severity == OperationOutcome.IssueSeverity.Warning || issue.Severity == OperationOutcome.IssueSeverity.Error))
                {
                    // Set the overall result to indicate that there were issues
                    operationOutcome.Issue.Add(new OperationOutcome.IssueComponent
                    {
                        Severity = OperationOutcome.IssueSeverity.Error,
                        Code = OperationOutcome.IssueType.Exception,
                        Diagnostics = "Resource validation failed. Server returned a response with warnings or errors."
                    });
                }
            }
            else
            {
                operationOutcome.Issue.Add(new OperationOutcome.IssueComponent
                {
                    Severity = OperationOutcome.IssueSeverity.Error,
                    Code = OperationOutcome.IssueType.Processing,
                    Diagnostics = "Resource validation failed. Server returned an error response."
                });
            }
        }
        catch (Exception ex)
        {
            operationOutcome.Issue.Add(new OperationOutcome.IssueComponent
            {
                Severity = OperationOutcome.IssueSeverity.Fatal,
                Code = OperationOutcome.IssueType.Exception,
                Diagnostics = $"An error occurred during resource validation: {ex.Message}"
            });
        }

        return operationOutcome;
    }
    public static StructureDefinition GetPatientStructureDefinitionFromServer(string typeName)
    {
        try
        {
            string patientStructureDefinitionUrl = $"{_fhirClient.Endpoint}StructureDefinition/" + typeName;

            var patientStructureDefinition = _fhirClient.ReadAsync<StructureDefinition>(patientStructureDefinitionUrl).GetAwaiter().GetResult();

            if (patientStructureDefinition != null)
            {
                Console.WriteLine("Found Patient StructureDefinition:");
                Console.WriteLine($"URL: {patientStructureDefinition.Url}");
                Console.WriteLine($"Name: {patientStructureDefinition.Name}");
                Console.WriteLine($"Description: {patientStructureDefinition.Description.ToString()}");
            }
            else
            {
                Console.WriteLine("No Patient StructureDefinition found.");
            }
            return patientStructureDefinition;
        }
        catch (FhirOperationException ex)
        {
            Console.WriteLine($"FHIR operation error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        return new StructureDefinition();
    }
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
    //public static OperationOutcome ValidatePatient(Resource resource)
    //{
    //    var outcome = new OperationOutcome();
    //    if (!(resource is Patient pat))
    //    {
    //        outcome.Issue.Add(new OperationOutcome.IssueComponent { Severity = OperationOutcome.IssueSeverity.Fatal, Diagnostics = "Resource is not a Patient." });
    //        return outcome;
    //    }
    //    Patient patient = new Patient();
    //    var validationContext = new ValidationContext(patient, serviceProvider: null, items: null);
    //    var validationResults = new List<ValidationResult>();

    //    if (!Validator.TryValidateObject(resource, validationContext, validationResults, validateAllProperties: true))
    //    {
    //        foreach (var validationResult in validationResults)
    //        {
    //            outcome.Issue.Add(new OperationOutcome.IssueComponent
    //            {
    //                Severity = OperationOutcome.IssueSeverity.Error,
    //                Code = OperationOutcome.IssueType.Invalid,
    //                Location = validationResult.MemberNames,
    //                Diagnostics = validationResult.ErrorMessage
    //            });
    //        }
    //    }
    //    else
    //    {
    //        if (patient.Name.Count < 1) 
    //        {
    //            outcome.Issue.Add(new OperationOutcome.IssueComponent
    //            {
    //                Severity = OperationOutcome.IssueSeverity.Error,
    //                Code = OperationOutcome.IssueType.Invalid,
    //                Location = new List<string> { "Patient Name" },
    //                Diagnostics = " must have at least a name defined."
    //            });
    //            return outcome;
    //        }

    //        outcome.Issue.Add(new OperationOutcome.IssueComponent { Severity = OperationOutcome.IssueSeverity.Information, Diagnostics = "Patient is valid." });
    //    }

    //    return outcome;
    //}

    //public static OperationOutcome ValidatePatientAgainstProfile(Resource resource)
    //{
    //    var operationOutcome = new OperationOutcome();

    //    if (!(resource is Patient patient))
    //    {
    //        operationOutcome.Issue.Add(new OperationOutcome.IssueComponent { Severity = OperationOutcome.IssueSeverity.Fatal, Diagnostics = "Resource is not a Patient." });
    //        return operationOutcome;
    //    }
    // var patientProfile = EncounterProfile.CreatePatientProfile();
    //    // Validate the resource against the profile
    //    var validationContext = new ValidationContext(patientProfile, serviceProvider: null, items: null);
    //    var validationResults = new List<ValidationResult>();


    // if (!Validator.TryValidateObject(resource, validationContext, validationResults, validateAllProperties: true))
    // {
    //        foreach (var validationResult in validationResults)
    //        {
    //            operationOutcome.AddIssue(new OperationOutcome.IssueComponent
    //            {
    //                Severity = OperationOutcome.IssueSeverity.Error,
    //                Code = OperationOutcome.IssueType.Invalid,
    //                Location = validationResult.MemberNames,
    //                Diagnostics = validationResult.ErrorMessage
    //            });
    //        }
    //    }
    //    return operationOutcome;
    //}
    //public static OperationOutcome ValidatePatientAgainstCustomProfile(Resource patient)
    //{
    //    var patientProfile = EncounterProfile.CreatePatientProfile();
    //    ValidationContext valdiationContext = new ValidationContext(patient);
    //    var validationResults = patientProfile.Validate(valdiationContext);

    //    var operationOutcome = new OperationOutcome();

    //    if (validationResults.Any())
    //    {
    //        foreach (var validationResult in validationResults)
    //        {
    //            operationOutcome.AddIssue(new OperationOutcome.IssueComponent
    //            {
    //                Severity = OperationOutcome.IssueSeverity.Error,
    //                Code = OperationOutcome.IssueType.Invalid,
    //                Diagnostics = validationResult.ErrorMessage
    //            });
    //        }
    //    }

    //    return operationOutcome;
    //}

    //public static OperationOutcome ValidateResourceUSCDIV3(Resource resource)
    //{
    //    var igJson = File.ReadAllText("../FHIRValidation/ImplementationGuide-hl7.fhir.us.core.json");
    //    var igObject = JObject.Parse(igJson);
    //    var resourceTypeName = resource.TypeName;
    //    var supportedProfiles = new HashSet<string>();
    //    var structuredDefintions = new Dictionary<string,string>();

    //    string pattern = @"\bUS\s*Core\s*(\w+)\b";
    //    Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);

    //    var resources = igObject.SelectTokens("$.definition.resource[*]").ToList();

    //    foreach (var res in resources)
    //    {
    //        var reference = res.SelectToken("$.reference.reference")?.Value<string>();
    //        var structuredDefinition = res.SelectToken("$.extension[0].url")?.Value<string>(); 
    //        var name = res.SelectToken("$.name")?.Value<string>();

    //        if (!string.IsNullOrEmpty(reference))
    //        {
    //            supportedProfiles.Add(reference);
    //        }
    //        if (!string.IsNullOrEmpty(structuredDefinition))
    //        {
    //            structuredDefintions.Add(name, structuredDefinition);
    //        }
    //    }
    //    var outcome = new OperationOutcome();

    //    var profileMatch = false;
    //    foreach (var structuredDefintionPair in structuredDefintions)
    //    {
    //        Match match = regex.Match(structuredDefintionPair.Key);
    //        string succeedingText = match.Groups[1].Value;
    //        if (succeedingText.Equals(resource.TypeName) || structuredDefintionPair.Key.Equals(resource.TypeName) )
    //        {
    //            //Call match structured defintion fetch
    //           var structuredDefintion =  FetchStructureDefinitionFromUrlAsync(structuredDefintionPair.Value).GetAwaiter().GetResult();
    //           var validationResult = ValidateResourceAgainstStructureDefinition(resource, structuredDefintion);
    //            if (validationResult.ErrorMessage!=null)
    //            {
    //                outcome.Issue.Add(new OperationOutcome.IssueComponent
    //                {
    //                    Severity = OperationOutcome.IssueSeverity.Error,
    //                    Code = OperationOutcome.IssueType.Invalid,
    //                    Location = validationResult.MemberNames,
    //                    Diagnostics = validationResult.ErrorMessage
    //                });

    //                return outcome;
    //            }
    //        }
    //    }

    //    //foreach (var profile in supportedProfiles)
    //    //{
    //    //    if (resource.Meta?.Profile?.Contains(profile) == true)
    //    //    {
    //    //        profileMatch = true;
    //    //        break;
    //    //    }
    //    //}
    //    return new OperationOutcome();
    //}

    //private static ValidationResult ValidateResourceAgainstStructureDefinition(Resource resource, StructureDefinition structureDefinition)
    //{
    //    // Check if the resource type matches the type defined in the StructureDefinition
    //    if (resource.TypeName != structureDefinition.Type)
    //    {
    //        return new ValidationResult($"Resource type '{resource.TypeName}' does not match StructureDefinition type '{structureDefinition.Type}'.");
    //    }

    //    // Perform further validation logic comparing the resource's properties with elements in the StructureDefinition
    //    // For example, you can iterate through elements in the StructureDefinition and check corresponding properties in the resource
    //    foreach (var element in structureDefinition.Differential.Element)
    //    {
    //        var propertyName = element.Path; // Path of the element in the StructureDefinition
    //        var propertyInfo = resource.GetType().GetProperty(propertyName); // Get the corresponding property in the resource

    //        if (propertyInfo == null)
    //        {
    //            // Property not found in the resource, which may indicate a mismatch between the resource and StructureDefinition
    //            return new ValidationResult($"Property '{propertyName}' not found in the resource.");
    //        }

    //        // Validate the property according to the constraints specified in the StructureDefinition
    //        // You can access constraints such as cardinality, data type, and other metadata from the StructureDefinition
    //        // Implement your validation logic here...

    //        // Example: Check if the property is required
    //        if (element.Min > 0 && propertyInfo.GetValue(resource) == null)
    //        {
    //            return new ValidationResult($"Property '{propertyName}' is required but missing in the resource.");
    //        }

    //        // Example: Check data type conformity (e.g., for primitive types)
    //        // Note: You might need more complex logic for complex types or data types with multiple allowed types
    //        if (element.Type != null && element.Type.Count > 0)
    //        {
    //            var expectedTypes = string.Join(", ", element.Type.Select(t => t.Code));
    //            var actualType = propertyInfo.PropertyType.Name;

    //            //if (!element.Type.Any(t => IsTypeCompatible(propertyInfo.PropertyType, t)))
    //            //{
    //            //    return new ValidationResult($"Property '{propertyName}' type '{actualType}' does not match the expected types: {expectedTypes}.");
    //            //}
    //        }

    //        // Add more validation rules as needed...

    //    }

    //    // If no validation issues are found, return success
    //    return ValidationResult.Success;
    //}





}
    //DotNetAttributeValidation.Validate(patient, true);

    //var operationOutcome = new OperationOutcome();

    //if (DotNetAttributeValidation.TryValidate(patient, true));
    //{
    //    // Iterate over the validation errors and add them to the OperationOutcome
    //    foreach (var error in DotNetAttributeValidation.Errors(patient))
    //    {
    //        operationOutcome.AddIssue(new OperationOutcome.IssueComponent
    //        {
    //            Severity = OperationOutcome.IssueSeverity.Error,
    //            Code = OperationOutcome.IssueType.Invalid,
    //            Diagnostics = error.ErrorMessage
    //        });
    //    }
    //}

    // return operationOutcome;
    //   }
    //  }
}
//var package1 = "../SolutionItems/package.tgz";

//FhirPackageSource resolver = new(ModelInfo.ModelInspector, new string[] { package1 });
//string structureDefIdentifier = "us-core-" + resource.TypeName.ToLower();
//var uri = "http://hl7.org/fhir/us/core/StructureDefinition/" + structureDefIdentifier;
//var canonicalUrisList = resolver.ListCanonicalUris();
//var resourceuris = resolver.ListResourceUris();
//List<StructureDefinition> structDefList = new List<StructureDefinition>();
//StructureDefinition structDef = null;
//var structFromServer = GetStructureDefintion(structureDefIdentifier, token);
//foreach (var resourceuri in canonicalUrisList)
//{
//    var temp = resolver.FindStructureDefinitionAsync(resourceuri).GetAwaiter().GetResult();
//    if (temp != null)
//    {
//        structDefList.Add(temp);
//    }
//}
//var structureDefinition = resolver.FindStructureDefinitionAsync(uri).GetAwaiter().GetResult();

//var serverStructureDefinition = GetPatientStructureDefinitionFromServer(structureDefIdentifier);

//if (structureDefinition == null)
//{
//    return OperationOutcome.ForMessage("Resource is valid.", OperationOutcome.IssueType.Informational, OperationOutcome.IssueSeverity.Information);
//}
