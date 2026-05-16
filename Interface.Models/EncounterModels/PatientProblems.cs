using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.EncounterModels
{
    public class PatientProblem : BaseModel
    {
        public long Id { get; set; }
        public long claimId { get; set; }
        public long PatientId { get; set; }
        public long? EncounterId { get; set; }
        public string? ICDCode { get; set; }
        public string? ICDCodeDescription { get; set; }
        public DateTimeOffset? OnsetDate { get; set; }
        public DateTimeOffset? ResolvedDate { get; set; }
        public int ProblemStatus { get; set; }
        public string? ProblemStatusName { get; set; }
        public bool Historical { get { return ResolvedDate != null; } }
        public string? Notes { get; set; }
        public int Acuity { get; set; }
        public string? AcuityName { get; set; }
        public long? ProviderId { get; set; }
        public string? PatientMrn {  get; set; }
        public ResourceReference PatientReference { get; set; }
        public ResourceReference EncounterReference { get; set; }
        public ResourceReference PractitionerReference { get; set; }
    }
    public enum ProblemTableName
    {
        [Display(Name = "patientproblem")]
        patientproblem,

        [Display(Name = "claimdiagnosis")]
        claimdiagnosis
    }
}
