using Hl7.FhirPath.Sprache;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.BackgroundServices
{
    public class PatientResourceRecords
    {
        public long? PatientId { get; set; }
        public string? PatientMrn { get; set; }
        public string? ResourceType { get; set; }
        public string? TableName { get; set; }
        public long? EncounterId { get; set; }
        public long? PracticeId { get; set; }
        public long? ResourceId { get; set; }
        public int? TotalCount { get; set; }
    }
  
  

   
}
