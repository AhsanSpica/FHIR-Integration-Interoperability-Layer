using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.DocumentReferenceModels
{
    public class PatientDocumentMultipleDto : BaseModel
    {

        public long Id { get; set; }

        public long? PatientId { get; set; }

        public string? Extension { get; set; }

        public string? DisplayName { get; set; }

        public string? DocumentName { get; set; }

        public int? DocumentType { get; set; }

        public string? DocumentUri { get; set; }

        public long? SizeInBytes { get; set; }
        public long? PracticeId { get; set; }

        //    public long? AssignedTo { get; set; }

        public bool? ShowOnPortal { get; set; }

        public bool? Signed { get; set; }

        public string? Comments { get; set; }

        public bool? IsActive { get; set; }

        public DateTimeOffset? Date { get; set; }

        public bool? ShareWithPatient { get; set; }

        public string? AssignedBy { get; set; }

        public string? ShareWithPatientUser { get; set; }
        public List<DocumentUsers>? AssignedUsers { get; set; }
        public List<DocumentActionODT> documentActions { get; set; }
        public bool CanSign { get; set; }
        public bool CanReview { get; set; }
    }
    public class DocumentActionODT
    {
        public int Id { get; set; }
        public string? AssignedBy { get; set; }
        public int? AssignedUserId { get; set; }
        public string? UserName { get; set; }
        public long? DocumentId { get; set; }
        public bool? IsSigned { get; set; }
        public bool? IsReviewed { get; set; }
        public bool? IsProvider { get; set; }
        public DateTimeOffset? AssignedDate { get; set; }
        public DateTimeOffset? SignedDate { get; set; }
        public DateTimeOffset? ReviewedDate { get; set; }

    }
    public class DocumentUsers
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
    }
}
