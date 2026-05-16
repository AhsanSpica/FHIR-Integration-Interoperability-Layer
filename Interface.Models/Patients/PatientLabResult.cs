using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientLabResult : BaseModel
    {
        public long Id { get; set; }
        public long ResultCode { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset ResultDate { get; set; }
        public long? FlagValue { get; set; }
        public string? Unit { get; set; }
        public string? ReferenceRange { get; set; }
        public long? ResultStatus { get; set; }
        public bool? AbnormalFlag { get; set; }
        public string? LabNotes { get; set; }
        public string? PhysicianNotes { get; set; }
        public long? LabOrderId { get; set; }
    }
}
