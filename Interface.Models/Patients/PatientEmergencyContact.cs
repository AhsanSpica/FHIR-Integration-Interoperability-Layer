 using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientEmergencyContact : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }

        [MaxLength(100), MinLength(0)]
        public string MiddleName { get; set; }

        [MaxLength(256), MinLength(0)]
        public string AddressLine1 { get; set; }

        [MaxLength(256), MinLength(0)]
        public string AddressLine2 { get; set; }

        [MaxLength(100), MinLength(0)]
        public string City { get; set; }

        [MaxLength(100), MinLength(0)]
        public string State { get; set; }

        [MaxLength(25), MinLength(0)]
        public string Zip { get; set; }

        [Required]
        [MaxLength(100), MinLength(0)]
        public string FirstName { get; set; }        

        [Required]
        [MaxLength(100), MinLength(0)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50), MinLength(0)]
        public string PhoneNumber { get; set; }
        
        [Required]
        //[MaxLength(50), MinLength(0)]
        public int? RelationToPatient { get; set; }
        public string? RelationToPatientText { get; set; }

        public string? EmailAddress { get; set; }
        public bool? Guarantor { get; set; }
        public bool? NextofKin { get; set; }
        public string? OfficeNumber { get; set; }

    }
}
