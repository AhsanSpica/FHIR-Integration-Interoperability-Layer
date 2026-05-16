using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.ImmunizationModels
{
    public class ImmunizationDTO : BaseModel
    { 
        public ImmunizationDTO()
        {
            VisDocumentDetails = new List<ImmunizationVISEdition>();
        }
        public long Id { get; set; }

        public string? VaccineName { get; set; }

        public DateTimeOffset? AdministeredDate { get; set; }

        public decimal? Dosage { get; set; }

        public int? Unit { get; set; }

        public int? Route { get; set; }

        public int? Site { get; set; }

        public int? OrderBy { get; set; }

        public int? AdministeredBy { get; set; }

        public int? Facility { get; set; }

        public string? ManufactureName { get; set; }

        public string? LotNumber { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public int? FundingSource { get; set; }

        public int? FundingProgram { get; set; }

        public string? VisDocumentName { get; set; }

        public string? Notes { get; set; }

        public string? ReportingFacility { get; set; }

        public int? RefuseReason { get; set; }

        public DateTimeOffset? RefusalDate { get; set; }

        public int? ImmunizationType { get; set; }

        public long? PatientId { get; set; }

       // public long? EncounterId { get; set; }

        public string? OtherReason { get; set; }

        public int? CVXCode { get; set; }

        public string? AgeWhenVaccinated { get; set; }

        public List<ImmunizationVISEdition>? VisDocumentDetails { get; set; }

        public string? AdministeredByName { get; set; }
        public string? NDC11 { get; set; }
        public string? MVXCode { get; set; }
        public bool? isPrimary { get; set; } 
        
        //ETL 
        public string? PatientMrn { get; set; }
        public ResourceReference PatientReference { get; set; }
        public ResourceReference EncounterReference { get; set; }
        public ResourceReference LocationReference { get; set; }
        public ResourceReference PractitionerReference { get; set; }
    }
    public class ImmunizationVISEdition
    {
        public int? Id { get; set; }
        public int? ImmunizationID { get; set; }
        public int? VISId { get; set; }
        public string? VISFullyEncodedText { get; set; }
        public string? VISDocumentName { get; set; }
        public DateTime? VisEditionDate { get; set; }
    }
 
}
