using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientAlert : BaseModel
    {

        public long Id { get; set; }

        public long PatientId { get; set; }

        public string? Note { get; set; }
    }
}
