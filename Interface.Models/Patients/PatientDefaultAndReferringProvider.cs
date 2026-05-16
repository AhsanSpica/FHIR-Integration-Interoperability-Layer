using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientDefaultAndReferringProvider : BaseModel
    {
        public long PatientId { get; set; }
        public long PracticeId { get; set; }
        public long ProviderId { get; set; }
        public long LocationId { get; set; }

        [MaxLength (25), MinLength(0) ]
        public string? ReferringProviderNpi { get; set; }

        [Key]
        public long Id { get; set; }
        
    }
    
}
