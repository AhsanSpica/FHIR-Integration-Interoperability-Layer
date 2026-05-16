using Dapper;
using Hl7.Fhir.Model;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Models.BackgroundServices;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace Interface.Misc.Implementation
{
    public class FhirService : IFhirService
    {
        private readonly IFhirClient _fhirClient;
        private readonly DBAccessFhirSingleton _dBAccessFHIR;
        private readonly int batchSize = 100;
        private readonly IQueueSubscriberService _queueSubscriberService;


        public FhirService(IFhirClient fhirClient,
            DBAccessFhirSingleton dBAccessFHIR,
            IQueueSubscriberService queueSubscriberService)
        {
            _fhirClient = fhirClient;
            _dBAccessFHIR = dBAccessFHIR;
            _queueSubscriberService = queueSubscriberService;
        }
        public async System.Threading.Tasks.Task CreateTokenAsync()
        {
           await _fhirClient.CreateTokenAsync();
        }


        //::TODO DEV-TEST
       public async Task<(ConcurrentBag<PatientResourceRecords> RecordsNotInFhir, ConcurrentBag<PatientResourceRecords> RecordsToUpdate)> 
            FetchAndProcessIdentifiersAsync(ConcurrentBag<PatientResourceRecords> resourceRecords,string resourceType, string extensionUrl )
        {
            return await _fhirClient.FetchAndProcessIdentifiersAsync(resourceType, extensionUrl, resourceRecords);
        }
        //:: DEV-TEST Above Method


        public async Task<ConcurrentBag<PatientResourceRecords>> SearchByIdentifierAsync
            (ConcurrentBag<PatientResourceRecords> resourceRecords, string resourceType, string extensionUrl)
        {
            return await _fhirClient.SearchByIdentifierAsync(resourceRecords, resourceType, extensionUrl); 
        }
        public async Task<string> ReturnTokenAsync()
        {
            return await _fhirClient.ReturnTokenAsync();
        }
        public async Task<ConcurrentBag<PatientResourceRecords>> ExtractResourceIds(string resourceType)
        {
            int offset = 0;
            ConcurrentBag<PatientResourceRecords> allResourceIds = new ConcurrentBag<PatientResourceRecords>();
            List<PatientResourceRecords> resourceIds;


            // Fetch messages from Azure Service Bus
            if (_queueSubscriberService.ServiceBusIsActive())
            {
                var messages = _queueSubscriberService.GetMessages();
                foreach (var message in messages)
                {
                    // Process the message and possibly add to resourceIds
                    // Your logic to process the message
                    HelperMethods.CreateConsoleLog($"Processing message: {message.Body}");

                    var resourceRecord = JsonConvert.DeserializeObject<PatientResourceRecords>(message.Body.ToString());
                    if (resourceRecord != null)
                    {
                        allResourceIds.Add(resourceRecord);
                    }
                }
            }
            do
            {
                var _param = new DynamicParameters();
                _param.Add("@FetchRecord", offset);
                _param.Add("@resourceType", resourceType);

                resourceIds = await _dBAccessFHIR.GetAll<PatientResourceRecords>
                   ("GetAllNewRecord", _param, _dBAccessFHIR.GetConnectionString(), System.Data.CommandType.StoredProcedure);

                offset += batchSize;
                foreach (var resource in resourceIds)
                { allResourceIds.Add(resource); }

            } while (resourceIds.Count == batchSize);

            return allResourceIds;
        }

        //DB call method
        //public async Task<List<PatientResourceRecords>> ExtractResourceIds(string resourceType)
        //{
        //    int offset = 0;
        //    List<PatientResourceRecords> allResourceIds = new List<PatientResourceRecords>();
        //    List<PatientResourceRecords> resourceIds;

        //    do
        //    {
        //        var _param = new DynamicParameters();
        //        _param.Add("@FetchRecord", offset);
        //        _param.Add("@resourceType", resourceType);

        //        resourceIds = await _dBAccessFHIR.GetAll<PatientResourceRecords>
        //           ("GetAllNewRecord", _param, _dBAccessFHIR.GetConnectionString(), System.Data.CommandType.StoredProcedure);

        //        offset += batchSize;

        //        allResourceIds.AddRange(resourceIds);


        //    } while (resourceIds.Count == batchSize);

        //    return allResourceIds;
        //}
        public async Task<string> GetResourceIdByIdentifierAsync(string resourceType, string mrn, long id)
        {       
                var resource = await _fhirClient.GetIdByIdentifierAsync<Resource>(resourceType, mrn, id);
                return resource;
        }

        public async System.Threading.Tasks.Task CreateResourceAsync(string resource, string mapperResourceType)
        {
            try
            {
                await _fhirClient.CreateAsync(resource, mapperResourceType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error for creating the rsoruce on the Fhir Server {mapperResourceType} Error : {ex.ToString()}");
            }
        }

        public async System.Threading.Tasks.Task GetResourceCount(string resourceType, int count) 
        {
            try
            {
                await _fhirClient.GetResourceCount(resourceType, count);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error for creating the rsoruce on the Fhir Server "+ex.ToString());
            }
        }
        public ResourceReference GetResourceReference(long? id, string resourceType, string? mrn)
        {

            string fhirId = GetResourceIdByIdentifierAsync(resourceType, mrn, id.Value).GetAwaiter().GetResult();

            if (string.IsNullOrEmpty(fhirId))
            {
                if (resourceType.Equals("Patient"))
                {
                    return new ResourceReference { Reference = $"{resourceType}/{mrn}" };
                }
                else
                {
                    return new ResourceReference { Reference = $"{resourceType}/{id}" };
                }
            }
            else
            {
                return new ResourceReference { Reference = $"{resourceType}/{fhirId}" };
            }
        }

      
    }
}

 //private void ProcessResource<TSource, TTarget>(IService<TSource> service, IMapper<TSource, TTarget> mapper, string cacheKey)
 //{
 //    if (!_memoryCache.TryGetValue(cacheKey, out List<int> itemIds))
 //    {
 //        itemIds = service.GetAllItemIds();
 //        _memoryCache.Set(cacheKey, itemIds, TimeSpan.FromHours(1));
 //    }

//    foreach (var itemId in itemIds)
//    {
//        var item = service.GetItemById(itemId);
//        var resource = mapper.Map(item);
//        _fhirClient.Create(resource).GetAwaiter().GetResult(); // Synchronously wait for async method
//    }
//}
// Other methods...
