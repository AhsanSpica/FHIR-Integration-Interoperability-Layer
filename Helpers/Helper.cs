using Hl7.Fhir.Model;
using Hl7.Fhir.Validation;
using static Hl7.Fhir.Model.Address;
using static Hl7.Fhir.Model.ContactPoint;
using static Hl7.Fhir.Model.HumanName;
using System.Reflection;
using static Hl7.Fhir.Model.Patient;
using Interface.Models.InterfaceModels;
 using System;
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
using Firely.Fhir.Packages;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;

namespace GlobalHelpers
{
    public static class Helper
    {
        private static readonly IOptions<AppSetting> _common;
        //private static IConfiguration _configuration = new ConfigurationBuilder()
        //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //.Build();

        public static string EMRBaseUrl => _common.Value.EMRBaseURL;
        public static FhirPackageSource GetResolver()
        {
            var package1 = "./SolutionContent/package.tgz";
            FhirPackageSource resolver = new(ModelInfo.ModelInspector, new string[] { package1 });
            return resolver;

        }
        //public static FhirUrl GetCodeSystem (string )
        //{
        //    var resolver = GetResolver();
        //    resolver.FindCodeSystemAsync();
        //}
        public static async Task<string> GetStructureDefinitionUrl(string resourceTypeNamePart)
        {
            if (string.IsNullOrEmpty(resourceTypeNamePart))
            {
                throw new ArgumentNullException(nameof(resourceTypeNamePart));
            }

            using (var client = new RestClient("http://terminology.hl7.org"))
            {
                var request = new RestRequest($"/CodeSystem/$translate?code={resourceTypeNamePart}&system=http://hl7.org/fhir/StructureDefinition");
                request.AddParameter("accept", "application/json");

                var response = await client.ExecuteGetAsync(request);

                if (response.IsSuccessful && response.ContentLength > 0)
                {
                    var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(response.Content);

                    // Check if any translations (matches) are found
                    if (translations.ContainsKey(resourceTypeNamePart))
                    {
                        return translations[resourceTypeNamePart];
                    }
                }
            }

            // No matching StructureDefinition URL found
            return null;
        }
        public static FhirUrl GetFHIRURL(string typename)
        {
            var resolver = GetResolver();
            string structureDefIdentifier = "us-core-" + typename.ToLower();
            var uri = "http://hl7.org/fhir/us/core/StructureDefinition/" + structureDefIdentifier;
            var canonicalUrisList = resolver.ListCanonicalUris();
            var artifactNames = resolver.ListArtifactNames();
            var resourceUris = resolver.ListResourceUris();
            // var artifact = resolver.LoadArtifactByName();
            List<StructureDefinition> structDefList = new List<StructureDefinition>();
              foreach (var resourceuri in canonicalUrisList)
            {
                var temp = resolver.FindStructureDefinitionAsync(resourceuri).GetAwaiter().GetResult();
                if (temp != null)
                {
                    structDefList.Add(temp);
                }
            }
             var structureDefinition = resolver.FindStructureDefinitionAsync(uri).GetAwaiter().GetResult();
            if (structureDefinition != null)
            { return new FhirUrl { Value = structureDefinition.Url }; }
            else
            {
                return new FhirUrl();
            }
           
        }
        public static PatientR4B CreatePatientObject(PatientR4B patientR4B,string gender)
        {
            patientR4B.Id = Guid.NewGuid().ToString();
            patientR4B.Meta = new Meta { LastUpdated = DateTimeOffset.Parse("2022-11-02T03:00:20.899-07:00") };

            // Identifier
            patientR4B.Identifier = new List<Identifier>
                {
                    new Identifier
                    {
                        System = "https://www.xyzehr.com",
                        Value = "PAT0005"
                    }
                };

            // Active
            patientR4B.Active = true;

            // Name
            patientR4B.Name = new List<HumanName>
                {
                    new HumanName
                    {
                        Use = NameUse.Official,
                        Text = "Roger Federer",
                        Family = "Federer",
                        Given = new List<string> { "Roger" }
                    }
                };

            // Telecom
            patientR4B.Telecom = new List<ContactPoint>
                {
                    new ContactPoint
                    {
                        System = ContactPointSystem.Phone,
                        Value = "5557702787",
                        Use = ContactPointUse.Home
                    },
                    new ContactPoint
                    {
                        System = ContactPointSystem.Email,
                        Value = "roger222@gmail.com"
                    }
                };

            AdministrativeGender adminGender;
            if (Enum.TryParse(gender, true, out adminGender))
            {
                patientR4B.Gender = adminGender.ToString();
            }
           

            // BirthDate
            patientR4B.BirthDate = "1940-09-05";

            // Address
            patientR4B.Address = new List<Address>
                {
                    new Address
                    {
                        Use = AddressUse.Home,
                        Line = new List<string> { "599 Schowalter Promenade" },
                        City = "West Springfield",
                        State = "MA",
                        PostalCode = "01089",
                        Country = "us",
                        Period = new Period { Start = DateTimeOffset.Parse("2022-10-14T05:35:41-07:00").ToString() }
                    }
                };

            // MaritalStatus
            patientR4B.MaritalStatus = new CodeableConcept
            {
                Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://terminology.hl7.org/CodeSystem/v3-MaritalStatus",
                            Code = "M",
                            Display = "Married"
                        }
                    },
                Text = "A current marriage contract is active"
            };

            // Communication
            patientR4B.Communication = new List<CommunicationComponent>
                {
                    new CommunicationComponent
                    {
                        Language = new CodeableConcept
                        {
                            Coding = new List<Coding>
                            {
                                new Coding
                                {
                                    System = "urn:ietf:bcp:47",
                                    Code = "en",
                                    Display = "English"
                                }
                            },
                            Text = "The language which can be used to communicate with the patient about his or her health"
                        },Preferred = true
                    }
                };

            // Add extensions
            patientR4B.Extension = new List<Extension>
{
    // Race extension
    new Extension
    {
        Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-race",
        Value = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "urn:oid:2.16.840.1.113883.6.238",
                    Code = "2106-3",
                    Display = "White"
                }
            }
        }
    },
    // Ethnicity extension
    new Extension
    {
        Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-ethnicity",
        Value = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "urn:oid:2.16.840.1.113883.6.238",
                    Code = "2186-5",
                    Display = "Not Hispanic or Latino"
                }
            }
        }
    },
    // Mother's Maiden Name extension
    new Extension
    {
        Url = "http://hl7.org/fhir/StructureDefinition/patient-mothersMaidenName",
        Value = new FhirString("Christen366 Murray856")
    },
    // Birth sex extension
    new Extension
    {
        Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-birthsex",
        Value = new Code("M")
    },
    // Sex extension
    new Extension
    {
        Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-sex",
        Value = new Code("248153007")
    },
    // Birth place extension
    new Extension
    {
        Url = "http://hl7.org/fhir/StructureDefinition/patient-birthPlace",
        Value = new Address
        {
            City = "Springfield",
            State = "Massachusetts",
            Country = "US"
        }
    },
    // Gender identity extension
    new Extension
    {
        Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-genderIdentity",
        Value = new CodeableConcept
        {
            Coding = new List<Coding>
            {
                new Coding
                {
                    System = "http://snomed.info/sct",
                    Code = "446151000124109",
                    Display = "Identifies as male gender (finding)"
                }
            }
        }
    },
    // Tribal affiliation extension
    new Extension
    {
        Url = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-tribal-affiliation",
        Extension = new List<Extension>
        {
            new Extension
            {
                Url = "tribalAffiliation",
                Value = new CodeableConcept
                {
                    Coding = new List<Coding>
                    {
                        new Coding
                        {
                            System = "http://terminology.hl7.org/CodeSystem/v3-TribalEntityUS",
                            Code = "338",
                            Display = "Native Village of Afognak"
                        }
                     }
                    }
                  }
                 }
              }
            };
            return patientR4B;
        }

    }
}
