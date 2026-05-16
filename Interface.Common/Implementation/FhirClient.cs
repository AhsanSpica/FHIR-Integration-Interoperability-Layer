using Azure.Core;
using Fhir.Metrics;
using GlobalHelpers;
using Hl7.Fhir.Model;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Utility;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Models.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using Task = System.Threading.Tasks.Task;


namespace Interface.Misc.Implementation
{
    public class FhirClient : IFhirClient
    {
        private readonly HttpClient _httpClient;
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
        private readonly int _maxConcurrency = 100;
        private readonly IOptions<AzureFhirServiceSettings> _afsSettings;
        private readonly string _fhirServerUrl;
        private readonly AzureAdTokenService _azureAdTokenService;

        private readonly IOptions<AppSetting> _options;

        public FhirClient(HttpClient httpClient, IConfiguration configuration, AzureAdTokenService azureAdTokenService,
            IFhirSerializer.IFhirSerializer fhirSerializer,
            IOptions<AzureFhirServiceSettings> afsSettings,
            IOptions<AppSetting> options
            //, FhirEvaluationContext fhirCtx
            )
        {
            _afsSettings = afsSettings;
            _httpClient = httpClient;
            _options = options;

            if (_httpClient.Timeout.TotalSeconds == 100)
            {
                _httpClient.Timeout = TimeSpan.FromMinutes(_options.Value.BundlePOSTTimeOut);
            }

            _azureAdTokenService = azureAdTokenService;
            _fhirSerializer = fhirSerializer;
            var isLocal = _afsSettings.Value.IsLocalFHIR;
            if (!isLocal)
            { _fhirServerUrl = _afsSettings.Value.AzureUrl; }
            else { _fhirServerUrl = _afsSettings.Value.LocalFhir; };
            if (!isLocal)
            {
                AddAuthenticationHeaderAsync().GetAwaiter().GetResult();
            }
            //  _fhirCtx = fhirCtx;

        }
        private async Task AddAuthenticationHeaderAsync()
        {
            var token = await _azureAdTokenService.GetTokenAsync();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/fhir+json"));
            // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        public async Task CreateTokenAsync()
        {
         await AddAuthenticationHeaderAsync();

        }
        public async Task<string> ReturnTokenAsync()
        {
            return await _azureAdTokenService.GetTokenAsync();

        }

        //::TDO DEV_TEST batch search function
        //:: Functionality : to retreive all identifers in batches, redcuing server hits
        //:: Functionality : Return Tuple Update Collection of Record Ids as well for Put Request

        public async Task<(ConcurrentBag<PatientResourceRecords> RecordsNotInFhir, ConcurrentBag<PatientResourceRecords> RecordsToUpdate)>
    FetchAndProcessIdentifiersAsync(string resourceType, string fhirServerUrl, ConcurrentBag<PatientResourceRecords> resourceRecords)
        {
            const int batchSize = 100; // Define the maximum batch size
            var recordsNotInFhir = new ConcurrentBag<PatientResourceRecords>();
            var recordsToUpdate = new ConcurrentBag<PatientResourceRecords>();

            // Prepare batched resource records
            var resourceRecordsList = resourceRecords.ToList();
            int totalRecords = resourceRecordsList.Count;
            int totalBatches = (int)Math.Ceiling(totalRecords / (double)batchSize);

            HelperMethods.CreateConsoleLog($"Fetching identifiers for {totalRecords} {resourceType} records in {totalBatches} batches...");

            for (int batchIndex = 0; batchIndex < totalBatches; batchIndex++)
            {
                var currentBatch = resourceRecordsList.Skip(batchIndex * batchSize).Take(batchSize).ToList();
                HelperMethods.CreateConsoleLog($"Processing batch {batchIndex + 1}/{totalBatches} with {currentBatch.Count} records...");

                // Build FHIR URL for batch request
                string batchUrl = $"{fhirServerUrl}/{resourceType}?&_elements=identifier&_count={currentBatch.Count}";

                HttpResponseMessage response = await _httpClient.GetAsync(batchUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var bundle = _fhirSerializer.FhirR4DeSerializeBundle(responseContent);

                    if (bundle.Entry != null && bundle.Entry.Count > 0)
                    {
                        foreach (var record in currentBatch)
                        {
                            // Compare the resource type directly
                            var matchingEntry = bundle.Entry.FirstOrDefault(e =>
                                e.Resource?.TypeName?.ToString().Equals(record.ResourceType, StringComparison.OrdinalIgnoreCase) == true);

                            if (matchingEntry != null)
                            {
                                // If the resource types match, add to the update list
                                recordsToUpdate.Add(record);
                                HelperMethods.CreateConsoleLog($"Found matching record for PatientId {record.PatientId} - flagged for update.");
                            }
                            else
                            {
                                // If no match, add to the new records list
                                recordsNotInFhir.Add(record);
                                HelperMethods.CreateConsoleLog($"No matching record found for PatientId {record.PatientId} - flagged for post.");
                            }
                        }
                    }
                    else
                    {
                        // If no entries are found in the bundle, all current batch records are considered new
                        foreach (var record in currentBatch)
                        {
                            recordsNotInFhir.Add(record);
                        }

                        HelperMethods.CreateConsoleLog($"No matching records found in FHIR for batch {batchIndex + 1}/{totalBatches}. All records are new.");
                    }
                }
                else
                {
                    HelperMethods.CreateConsoleLog($"Failed to fetch identifiers for batch {batchIndex + 1}/{totalBatches}. HTTP {response.StatusCode}: {response.ReasonPhrase}");
                }

                await Task.Delay(500); // Throttle between batch requests
            }

            return (recordsNotInFhir, recordsToUpdate);
        }


        public async Task<ConcurrentBag<PatientResourceRecords>> SearchByIdentifierAsync(ConcurrentBag<PatientResourceRecords> resourceRecords, string resourceTyped, string extensionUrl)
        {
            var resultList = new ConcurrentBag<PatientResourceRecords>();
            var resourceType = returnResourceType(resourceTyped);

            HelperMethods.CreateConsoleLog($"Inside Task Block for checking existing {resourceTyped} {resourceRecords.Count} Records");

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                await AddAuthenticationHeaderAsync();
            }

            //::TODO
            //::alternate approach using element=_identifier, but for large records will have to batch the request
            //:: {fhirurl}/Resource?|&_elements=identifier&_count=111

            // Reduce semaphore count to prevent overloading
            using (var semaphore = new SemaphoreSlim(10))
            {
                var tasks = new List<Task>();

                foreach (var record in resourceRecords)
                {
                    await semaphore.WaitAsync();
                    tasks.Add(Task.Run(async () =>
                    {
                        try
                        {
                            HttpResponseMessage response = await QueryResourceAsync(resourceType, record, resourceTyped);

                            if (response.IsSuccessStatusCode)
                            {
                                var jsonString = await response.Content.ReadAsStringAsync();
                                var bundle = _fhirSerializer.FhirR4DeSerializeBundle(jsonString);

                                if (bundle.Entry == null || !(bundle.Entry.Count > 0))
                                {
                                    resultList.Add(record);
                                }
                                else
                                {
                                    // Bundle = new 
                                    //:: TODO add all records in db in a put bundle
                                    // customBundle.Entry.Add(new Bundle.EntryComponent { Resource = deserialized,
                                    // Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.PUT, Url = resourceType }
                                    //:: console logging
                                    //   HelperMethods.CreateConsoleLog($"Record Found {bundle.Entry.First().Resource.Id} against {record.ResourceType}/{record.ResourceId} patient id {record.PatientId} ");
                                }
                                await Task.Delay(500);
                            }
                            else
                            {
                                HelperMethods.CreateConsoleLog($"Error {response.StatusCode}: {response.ReasonPhrase}");
                            }
                        }
                        catch (Exception ex)
                        {
                            parallelTaskErrorMessage($"Exception {ex.Message} checking for resource id {record.PatientId} resourcetype {resourceTyped}");
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }));
                }

                await Task.WhenAll(tasks);  // Wait for all tasks to complete
            }

            return resultList;
        }

        // Abstracted query logic to reduce duplication

        private async Task<HttpResponseMessage> QueryResourceAsync(string resourceType, PatientResourceRecords record, string resourceTyped)
        {
            const int maxRetryAttempts = 10;  // Maximum number of retries
             int delayMilliseconds = 4000;  // Delay between retries
            int retryCount = 0;
            HttpResponseMessage response = null;

            while (retryCount < maxRetryAttempts)
            {
                try
                {
                    var src = "";
                    var url = "";
                    var emrSystem = $"https://qa.wmi360.com/EHR/api/main/api/v1/patient/getpatientbyid?PatientId={record.PatientId}";

                    if (record.ResourceType.Equals("Patient"))
                    {
                        src = $"{_fhirServerUrl}/{resourceType}?identifier={emrSystem}|{record.PatientMrn}";
                        response = await _httpClient.GetAsync(src);
                    }
                    else if (resourceType.Equals("Observation"))
                    {
                        url = getUrl(resourceTyped);
                        src = $"{_fhirServerUrl}/{resourceType}?identifier={url}|{record.ResourceId}";
                        response = await _httpClient.GetAsync(src);
                    }
                    else if (resourceType.Equals("Encounter"))
                    {
                        url = $"https://qa.wmi360.com/EHR/api/main/api/v1/encounters/getencounterbyid/{record.EncounterId}";
                        src = $"{_fhirServerUrl}/{resourceType}?identifier={url}|{record.EncounterId}";
                        response = await _httpClient.GetAsync(src);
                    }
                    else if (resourceType.Equals("Procedure"))
                    {
                        url = $"https://qa.wmi360.com/EHR/api/main/api/v1encounters/getencounterbilledprocedurebyencounterid?EncounterId={record.EncounterId}";
                        src = $"{_fhirServerUrl}/{resourceType}?identifier={url}|{record.ResourceId}";
                        response = await _httpClient.GetAsync(src);
                    }
                    else
                    {
                        src = $"{_fhirServerUrl}/{resourceType}?identifier={record.ResourceId}";
                        response = await _httpClient.GetAsync(src);
                    }

                    // If the request is successful, break out of the loop
                    if (response.IsSuccessStatusCode)
                    {
                        break;
                    }
                }
                catch (HttpRequestException ex)
                {
                    parallelTaskErrorMessage($"Request failed: {ex.Message}");
                }

                retryCount++;
                await Task.Delay(delayMilliseconds);
                delayMilliseconds *= 2; // Wait before retrying
            }

            return response;
        }
        public async Task<string> GetIdByIdentifierAsync<T>(string resourceType, string mrn, long id) where T : Resource
        {
            HttpResponseMessage response = null;
            var url = "";
            var resourceId = "";

            var src = "";
            var emrSystem = $"https://qa.wmi360.com/EHR/api/main/api/v1/patient/getpatientbyid?PatientId={id}";

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                await AddAuthenticationHeaderAsync();
            }

            if (resourceType.Equals("Patient"))
            {
                src = $"{_fhirServerUrl}/{resourceType}?identifier={emrSystem}|{mrn}";
                response = await _httpClient.GetAsync(src);
            }
            else if (resourceType.Equals("Observation"))
            {
                url = getUrl(resourceType);
                src = $"{_fhirServerUrl}/{resourceType}?identifier={url}|{id}";
                response = await _httpClient.GetAsync(src);
            }
            else if (resourceType.Equals("Encounter"))
            {
                url = $"https://qa.wmi360.com/EHR/api/main/api/v1/encounters/getencounterbyid/{id}";
                src = $"{_fhirServerUrl}/{resourceType}?identifier={url}|{id}";
                response = await _httpClient.GetAsync(src);
            }
            else
            {
                src = $"{_fhirServerUrl}/{resourceType}?identifier={id}";
                response = await _httpClient.GetAsync(src);
            }

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var bundle = _fhirSerializer.FhirR4DeSerializeBundle(jsonString);

                if (bundle.Entry.Count > 0)
                {
                    resourceId = bundle.Entry.FirstOrDefault().Resource.Id;
                }
            }
            return resourceId;
        }
        private string getUrl(string resourceTyped)
        {
            var srcURL = "";
            if (resourceTyped.Equals("SmokingStatus"))
            {
                srcURL = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-smokingstatus";
            }
            else if (resourceTyped.Equals("Vital"))
            {
                srcURL = "http://hl7.org/fhir/us/core/StructureDefinition/us-core-vital-signs";
            }

            return srcURL;
        }
        private string returnCategoryType(string resourceTyped)
        {
            if (resourceTyped.Equals("SmokingStatus"))
            {
                resourceTyped = "smoking-status";
            }
            else if (resourceTyped.Equals("Vital"))
            {
                resourceTyped = "vital-signs";
            }

            return resourceTyped;
        }

        private string returnResourceType(string resourceType)
        {
            if (resourceType.Equals("Vital") || resourceType.Equals("SmokingStatus"))
            {
                resourceType = "Observation";
            }
            return resourceType;
        }

        private void parallelTaskErrorMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine("*************************************");
            System.Diagnostics.Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine("*************************************");
        }

        public async Task CreateAsync(string jsonResource, string resourceType)
        {
            resourceType = returnResourceType(resourceType);


            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
               await AddAuthenticationHeaderAsync();
            }

              var deserialize = (Bundle)_fhirSerializer.FhirR4DeSerialize(jsonResource);

            //::Entry indivdual POST
            //foreach ( var entry in deserialize.Entry )
            //  {
            //   var serializeResoruce = _fhirSerializer.FhirR4SerializeResource(entry.Resource);

            //   var content = new StringContent(serializeResoruce, Encoding.UTF8, "application/fhir+json");

            //  var response = await _httpClient.PostAsync($"{_fhirServerUrl}/{entry.Resource.TypeName}", content);

            //:: Bundle Post
            var content = new StringContent(jsonResource, Encoding.UTF8, "application/fhir+json");

            const int maxRetryAttempts = 10;  
            const int delayMilliseconds = 8000;  
            int retryCount = 0;

            while (retryCount < maxRetryAttempts)
            {
                try
                {
                    HelperMethods.CreateConsoleLog($"Posting {deserialize.Entry.First().Resource.TypeName} Bundle of {deserialize.Entry.Count} . . .");
                    var response = await _httpClient.PostAsync($"{_fhirServerUrl}/", content);
               

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var resource = (Bundle)_fhirSerializer.FhirR4DeSerialize(jsonString);

                        HelperMethods.CreateConsoleLog($"Successfully added to FHIRServer ResoruceType / Resource-Id/Bundle-Id : {resourceType} / {resource.Entry.FirstOrDefault()?.Resource.Id}/{resource.Id}");
                    break;  
                }
                else
                {
                        HelperMethods.CreateConsoleLog($"Error posting FHIR resource: {response.StatusCode} - {response.ReasonPhrase}");

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        retryCount++;

                        if (retryCount >= maxRetryAttempts)
                        {
                                HelperMethods.CreateConsoleLog($"Failed to post resource after {retryCount} attempts.");
                            break;
                        }

                            HelperMethods.CreateConsoleLog($"Retrying... attempt {retryCount} of {maxRetryAttempts}");

                        await Task.Delay(delayMilliseconds);
                    }
                    else
                    {
                        break;
                    }
                }
                }
                catch(Exception ex)
                    {
                    HelperMethods.CreateConsoleLog($"exception for posting resoruce of type {resourceType} {ex.Message}");
                }
            }
        }

        public async Task GetResourceCount(string resourceType, int Count)
        {

            resourceType = returnResourceType(resourceType);

            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                await AddAuthenticationHeaderAsync();
            }

            var response = await _httpClient.GetAsync($"{_fhirServerUrl}/{resourceType}_summary=count");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var fhirSummary = System.Text.Json.JsonSerializer.Deserialize<FhirSummary>(jsonString);

                System.Diagnostics.Debug.WriteLine($"The following {resourceType} has the following total records posted {fhirSummary.Total}  from{Count} ");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Error get FHIR resource: {response.StatusCode} - {response.ReasonPhrase}");

            }
        }
         

    }

}
