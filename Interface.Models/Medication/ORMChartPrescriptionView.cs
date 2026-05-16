using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Medication
{
    public class ORMChartPrescriptionView :BaseModel
    {
        public long Id { get; set; }
        public long? PatientId { get; set; }
        public long? ProviderId { get; set; }
        public long? PracticeId { get; set; }
        public long? EncounterId { get; set; }
        public string? DrugInfo { get; set; }
        public string? Rxnorm { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public string Archive { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string NumOfRefillsAllowed { get; set; }
        public string? SigText { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? ModifiedSig { get; set; }
        public string PharmacistNotes { get; set; }
        public bool IsEprescription { get; set; }
        public string? PharmacyInfo { get; set; }
        public string? Strength { get; set; }
        public string? Take { get; set; }
        public string? DosageFrequency { get; set; }
        public string? Quantity { get; set; }
        public int TotalRows { get; set; }
        public string PatientMrn { get; set; }
        public ResourceReference PatientResourceReference { get; set; }
        public ResourceReference EncounterResourceReference { get; set; }
        public ResourceReference PractitionerResourceReference { get; set; }

    }
}
