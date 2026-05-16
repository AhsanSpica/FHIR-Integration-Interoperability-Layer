using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.EncounterModels
{
    
        public class ORMChartAllergyView : BaseEntity
        {
            public long Id { get; set; }
            public long PatientId { get; set; }
            public string? PatientMrn { get; set; }
            public long PracticeId { get; set; }
            public long? ProviderId { get; set; }
            public long? EncounterId { get; set; }
            public string? Description { get; set; }
            public string? AllergyDate { get; set; }
            public string? Severity { get; set; }
            public int? Criticality { get; set; }
            public string? CriticalityName { get; set; }
            public string? ConceptType { get; set; }
            public string? Notes { get; set; }
            public string? OnsetDate { get; set; }
            public string? ExternalAllergyDate { get; set; }
            public string? RxNorm { get; set; }
            public string? Status { get; set; }
            public bool? IsEallergy { get; set; }
            public int? CategoryId { get; set; }
        public string? CategoryCode { get; set; }
        public string? CategorySystem { get; set; }
        public string? CategoryName { get; set; }
        public ResourceReference? PatientResourceReference { get; set; }
        public ResourceReference? EncounterResourceReference { get; set; }
        public ResourceReference? PractitionerResourceReference { get; set; }
        public string?    ManifestationCode { get; set; }
		 public string?    ManifestationDisplay {  get; set; } 
		  public string?   ManifestationSystem { get; set; }


    }
}
