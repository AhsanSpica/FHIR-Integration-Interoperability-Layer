 using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientPreferredPharmacy : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }

        //[MaxLength(50), MinLength(0)]
        public int? Type { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        [Required]
        public string Name { get; set; }
       
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Fax { get; set; }
    }
}
