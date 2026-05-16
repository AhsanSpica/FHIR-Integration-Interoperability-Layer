using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientImmunizationRegistryConsent : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long PatientId { get; set; }

        //[MaxLength(50), MinLength(0)]
        public int? RegistryStatus { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public bool? ProtectInfo { get; set; }
        public int ProtectInfoStatus { get; set; }

        //[MaxLength(50), MinLength(0)]
        public int? NotificationPreference { get; set; }
    }
}
