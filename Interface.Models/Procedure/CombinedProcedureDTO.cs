using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Procedure
{
    public class CombinedProcedureDTO :BaseModel
    {
        public long Id { get; set; }
        public long? EncounterId { get; set; }
        public long? PatientId { get; set; }
        public string? PatientMrn { get; set; }
        public long? RecorderId { get; set; }
        public string? ProcedureCode { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public int Unit { get; set; }
        public decimal Charges { get; set; }
        public int? POSCode { get; set; }
        public string? DiagnosisPointer { get; set; }
        public string? Modifier1 { get; set; }
        public string? Modifier2 { get; set; }
        public string? Modifier3 { get; set; }
        public string? Modifier4 { get; set; }
        public string? NDC { get; set; }
        public string? PriorAuth { get; set; }
        public int SequenceNumber { get; set; }
        public decimal TotalCharges { get; set; }
        public string? ProcedureCodeType { get; set; }
        public int? ReportId { get; set; }
        public string? ReasonString { get; set; }
        public string? CodeDetail { get; set; }
        public long? ClaimId { get; set; } // Added for Claim Procedure

        //ETL Linking
        public ResourceReference PatientReference { get; set; }
        public ResourceReference EncounterReference { get; set; }
    }
    public enum ProcedureTableName
    {
        [Display(Name = "EncounterBilledProcedure")]
        EncounterBilledProcedure,
        [Display(Name = "ClaimProcedure")]
        ClaimProcedure
    }

}
