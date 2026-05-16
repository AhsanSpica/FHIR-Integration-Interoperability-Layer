using Hl7.Fhir.Model;
using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.EncounterModels
{
    public class GoalMasterResponse : BaseModel
    {
        public List<GoalItemResponse>? GoalItems { get; set; }
        public long Id { get; set; }
        public long EncounterId { get; set; }
        public string? Notes { get; set; }
        public string? PatientMrn { get; set; }

    }
    public class GoalItemResponse
    {
        public long Id { get; set; }
        public long GoalId { get; set; }
        public string? GoalDetail { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public long? StatusId { get; set; }
        public string? AssignToName { get; set; }
        public int? PatientId { get; set; }
        public string? PatientMrn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public ResourceReference? PatientReference { get; set; }
    }
}
