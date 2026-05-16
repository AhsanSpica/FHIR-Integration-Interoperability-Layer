using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientEmail : BaseModel
    {
        public long Id { get; set; }

        [Required]
        public long PatientId { get; set; }

        public string EmailAddress { get; set; }

        public int EmailAddressType { get; set; }

        public bool IsPrimary { get; set; }
    }
}
