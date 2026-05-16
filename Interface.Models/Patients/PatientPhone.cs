using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientPhone : BaseModel
    {

        public long Id { get; set; }

        public long PatientId { get; set; }

        public string? PhoneNumber { get; set; }

        public int? PhoneNumberType { get; set; }

        public bool? IsPrimary { get; set; }
        public int? Ranking { get; set; }
    }
    public class PatientPhoneDto
    {
        public string? PhoneNumber { get; set; }
        public string? PhoneNumberType { get; set; }
    }
}
