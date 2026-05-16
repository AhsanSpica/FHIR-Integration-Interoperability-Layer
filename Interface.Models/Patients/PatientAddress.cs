 using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;   

namespace Interface.Models.Patients
{
    public class PatientAddress : BaseModel
    {

        public long Id { get; set; }

        //[Required]
        public long PatientId { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; } 

        [Required]
        [StringLength(100)]
        public string City { get; set; } 

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        [StringLength(25)]
        public string Zip { get; set; }

        public int? AddressType { get; set; }

        public int? AddressStatus { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? IsPrimary { get; set; }

    }
}
