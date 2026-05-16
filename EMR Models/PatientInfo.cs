using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMR_Models
{
    public class PatientInfo : BaseModel, IValidatableObject
    {
        public PatientInfo()
        {
            PatientAddresses = new List<PatientAddress>();
            PatientPhones = new List<PatientPhone>();
            PatientEmails = new List<PatientEmail>();
            PatientContactPreference = new PatientContactPreference();
            PatientInsurances = new List<PatientInsurance>();
            PatientCareTeams = new List<PatientCareTeam>();
            PatientCareTeamMembers = new List<PatientCareTeamMember>();
            PatientImmunizationRegistryConsents = new List<PatientImmunizationRegistryConsent>();
            PatientNotes = new List<PatientNote>();
            PatientPreferredPharmacies = new List<PatientPreferredPharmacy>();
            PatientEmergencyContacts = new List<PatientEmergencyContact>();
            PatientDefaultAndReferringProviders = new List<PatientDefaultAndReferringProvider>();
            patientRaces = new List<PatientRace>();
            patientEthnicities = new List<PatientEthnicity>();
            patientAlerts = new List<PatientAlert>();
            patientInsurancePreference = new List<PatientInsurancePreference>();
        }

        public List<PatientAddress>? PatientAddresses { get; set; }
        public List<PatientPhone>? PatientPhones { get; set; }
        public List<PatientEmail>? PatientEmails { get; set; }
        public PatientContactPreference? PatientContactPreference { get; set; }
        public List<PatientContactPreference>? PatientContactPreferences { get; set; }
        public List<PatientInsurance>? PatientInsurances { get; set; }
        public List<PatientCareTeam>? PatientCareTeams { get; set; }
        public List<PatientCareTeamMember>? PatientCareTeamMembers { get; set; }
        public List<PatientImmunizationRegistryConsent>? PatientImmunizationRegistryConsents { get; set; }
        public List<PatientNote>? PatientNotes { get; set; }
        public List<PatientPreferredPharmacy>? PatientPreferredPharmacies { get; set; }
        public List<PatientEmergencyContact>? PatientEmergencyContacts { get; set; }
        public List<PatientDefaultAndReferringProvider>? PatientDefaultAndReferringProviders { get; set; }
        public List<PatientRace>? patientRaces { get; set; }
        public List<PatientEthnicity>? patientEthnicities { get; set; }
        public List<PatientAlert>? patientAlerts { get; set; }
        public List<PatientInsurancePreference>? patientInsurancePreference { get; set; }

        public long Id { get; set; }

        [Required]
        public long PracticeId { get; set; }

        public int LastEncounterId { get; set; }

        public DateTimeOffset? LastAppointmentDate { get; set; }

        public string? MRN { get; set; }

        public string? CustomMRN { get; set; }

        public bool? IsCustomMRN { get; set; }


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string? MiddleInitial { get; set; }

        [Required]
        public int BirthSex { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        public string? SSN { get; set; }

        public int? Race { get; set; }

        //public int? Ethnicity { get; set; }

        public string? Prefix { get; set; }

        public string? Suffix { get; set; }

        public string? ProfilePic { get; set; }

        public string? MaidenName { get; set; }

        public string? AlternateName { get; set; }

        public int? SexualOrientation { get; set; }
        public string? OtherSexualOrientation { get; set; }
        [Required]
        public int GenderIdentity { get; set; }
        public string? OtherGenderIdentity { get; set; }

        public int? PreferredLanguage { get; set; }

        public int? LanguageAbility { get; set; }

        public int? LanguageProficiency { get; set; }

        public int? CountryOfParish { get; set; }

        public int? Religion { get; set; }

        public string? Deceased { get; set; }

        public string? CauseOfDeath { get; set; }

        //[Required]
        public DateTime? DeceasedDate { get; set; }

        public string? MotherMaidenName { get; set; }

        public int? Status { get; set; }

        public long? TotalCount { get; set; }

        public string? RaceName { get; set; }
        public string? ParishName { get; set; }

        public int? SerialNo { get; set; }

        public int? CurrentGender { get; set; }

        public int? MaritalStatus { get; set; }

        public bool? RaceDeclinedToSpecify { get; set; }

        public bool? EthnicityDeclinedToSpecify { get; set; }

        public int? OccupationIndustry { get; set; }

        public int? Occupation { get; set; }

        public int? TribalAffiliation { get; set; }

        public bool? IsStatusActive { get; set; }

        public int? BirthOrder { get; set; }
        public long? ACOKey { get; set; }
        public PatientInsurance? DefaultInsurance
        {
            get
            {
                if (PatientInsurances != null && PatientInsurances.Count > 0)
                {
                    return PatientInsurances[0];
                }
                return null;
            }
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DeceasedDate > DateTime.Now)
                yield return new ValidationResult("Deceased Date should not be future date");

            if (DateOfBirth > DateTime.Now)
                yield return new ValidationResult("DateOfBirth should not be future date");


            if (BirthOrder.HasValue && (BirthOrder.Value < 1 || BirthOrder.Value > 10))
            {
                yield return new ValidationResult("BirthOrder should be within the range of 1-10.");
            }


            //if (!(RaceDeclinedToSpecify ?? false) && (patientRaces == null || patientRaces.Count == 0))
            // yield return new ValidationResult("At least one race must be specified.");

            //if (!(EthnicityDeclinedToSpecify ?? false) && (patientEthnicities == null || patientEthnicities.Count == 0))
            //  yield return new ValidationResult("At least one PatientEthnicities must be specified.");


        }
        public class PatientVitalHeightDto
        {
            public decimal HeightValue { get; set; }
            public string? HeightUnit { get; set; }
            public string? HeightObservedDate { get; set; }
        }
    }
}
