
using Interface.Models.Common;

namespace Interface.Models.Patients
{
    public class PatientMedicalPastHistory : BaseModel
    {
        public long Id { get; set; }
        public long? PatientId { get; set; }
        public string? ICDCode { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
    }
    public class AddPatientMedicalPastHistory
    {
        public long? PatientId { get; set; }
        public string? ICDCode { get; set; }
        public string? Description { get; set; }
        public string? Notes { get; set; }
        public string? CreatedBy { get; set; }
    }
}
