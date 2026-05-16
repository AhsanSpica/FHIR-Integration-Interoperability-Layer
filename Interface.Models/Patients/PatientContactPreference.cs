 using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientContactPreference : BaseModel
    {
        public long Id { get; set; }

        public long PatientId { get; set; }

        public int? PreferredContactMethod { get; set; }

        public bool SmsReminders { get; set; }

        public bool EmailReminders { get; set; }

        public int ReminderAnticipationHours { get; set; }

        public bool? NoEmail { get; set; }
        public bool? NoPhone { get; set; }

    }

}
