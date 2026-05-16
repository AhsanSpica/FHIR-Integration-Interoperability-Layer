using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace Interface.Misc.Interfaces
{
    public interface IFhirClient
    {
        Task CreateAsync(string resource, string resourceType );
        //  Task<T> GetAsync<T>(string resourceId) where T : Resource;
        public  Task<ConcurrentBag<PatientResourceRecords>> SearchByIdentifierAsync(ConcurrentBag<PatientResourceRecords> resourceRecords,  string resourceType, string extensionUrl);
        Task<string> GetIdByIdentifierAsync<T>(string resourceType, string mrn, long id) where T : Resource;
        Task CreateTokenAsync();
        Task<string> ReturnTokenAsync();
        Task GetResourceCount(string resourceType, int Count);

        Task<(ConcurrentBag<PatientResourceRecords> RecordsNotInFhir, ConcurrentBag<PatientResourceRecords> RecordsToUpdate)>
    FetchAndProcessIdentifiersAsync(string resourceType, string fhirServerUrl, ConcurrentBag<PatientResourceRecords> resourceRecords);
    }
     
}
