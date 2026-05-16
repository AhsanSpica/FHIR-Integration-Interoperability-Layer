using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using Interface.Models.Common;

namespace Interface.Models.Patients
{
    public class PatientInsurancePreference : BaseModel
    {
        public long Id { get; set; }
        public long PatientId { get; set; }
        public int? PreferredInsuranceMethod { get; set; }
    }
}
