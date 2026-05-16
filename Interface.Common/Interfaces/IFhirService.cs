using Hl7.Fhir.Model;
using Interface.Models.BackgroundServices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Misc.Interfaces
{
    public interface IFhirService
    {
        // Task<ConcurrentBag<PatientResourceRecords>> GetNewRecordsByIdentifierAsync( string resourceType, string extensionUrl);
        Task<ConcurrentBag<PatientResourceRecords>> SearchByIdentifierAsync
             (ConcurrentBag<PatientResourceRecords> resourceRecords, string resourceType, string extensionUrl);
        Task<ConcurrentBag<PatientResourceRecords>> ExtractResourceIds(string resourceType);
        Task<string> GetResourceIdByIdentifierAsync(string resourceType, string mrn, long id); 
        System.Threading.Tasks.Task CreateResourceAsync(string resource, string resourceType);
        System.Threading.Tasks.Task GetResourceCount(string resourceType, int Count);
        System.Threading.Tasks.Task CreateTokenAsync();
        ResourceReference GetResourceReference(long? id, string resourceType, string? mrn);
          Task<string> ReturnTokenAsync();
        Task<(ConcurrentBag<PatientResourceRecords> RecordsNotInFhir, ConcurrentBag<PatientResourceRecords> RecordsToUpdate)>
    FetchAndProcessIdentifiersAsync(ConcurrentBag<PatientResourceRecords> resourceRecords, string resourceType, string extensionUrl);
    }
}
