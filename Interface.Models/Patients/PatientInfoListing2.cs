 using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Interface.Models.Patients
{
    public class PatientInfoListing : BaseModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MRN { get; set; }
        public DateTime DateOfBirth { get; set; }
        public List<PatientAddress>? PatientAddress { get; set; }
        public List<PatientPhone>? PatientPhone { get; set; }
        public DateTimeOffset? Accessed { get; set; }
        public string? AccessedBy { get; set; }
        public string? ProfilePic { get; set; }
        public long? BirthSex { get; set; }
        public long PatientId { get; set; }
        public string? Phone { get; set; }
        public string? PhoneType { get; set; }
        public bool? isStatusActive { get; set; }
    }
}
