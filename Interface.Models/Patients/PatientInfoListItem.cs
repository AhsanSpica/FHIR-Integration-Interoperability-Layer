using Interface.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientInfoListItem : BaseModel , IValidatableObject
    {

        public List<PatientAddress>? PatientAddress { get; set; }
        public List<PatientPhone>? PatientPhone { get; set; }
        public List<PatientEmail>? PatientEmail { get; set; }
        public PatientContactPreference? PatientContactPreference { get; set; }

        public List<PatientInsurance>? patientInsurances { get; set; }
        public List<PatientInsurancePreference>? patientInsurancePreference { get; set; }
        public long Id { get; set; }

        public long PracticeId { get; set; }

        public int LastEncounterId { get; set; }

        public DateTimeOffset? LastAppointmentDate { get; set; }

        public string? MRN { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? MiddleInitial { get; set; }

        public int BirthSex { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? ProfilePic  { get; set; }

        public int GenderIdentity { get; set; }

        public int? PreferredLanguage { get; set; }

        public int? Status { get; set; }
        public bool? IsStatusActive { get; set; }

        public long? TotalCount { get; set; }

        public long? ProviderId { get; set; }

        public long? LocationId { get; set; }

        //DQ_DEV_AHSAN adding following new fields on 5th of dec 2023
        public string? SSN { get; set; }
        public string? RaceText { get; set; }
        public string? MaritalStatusText { get; set; }
        public string? GenderIdentityText { get; set; }
        public string? BirthSexText { get; set; }
        public string? PreferredLanguageText { get; set; }
        public string? EthnicityText { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DateOfBirth > DateTime.Now)
                yield return new ValidationResult("DateOfBirth should not be future date");

        }
    }
}
