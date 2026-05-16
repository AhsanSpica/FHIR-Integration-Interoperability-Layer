using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientNote : BaseModel
    {
        [Key]
        public long Id { get; set; }

        public string? Notes { get; set; }

        [Required]
        public long PatientId { get; set; }
        
    }
}
