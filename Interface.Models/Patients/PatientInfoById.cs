using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Patients
{
    public class PatientInfoById
    {

        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientInfo]
        /// </summary>

        public long Id { get; set; }
        public string? MRN { get; set; }
        [Required] public long PracticeId { get; set; }
        public bool? IsCustomMRN { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string? MiddleInitial { get; set; }
        [Required] public int BirthSex { get; set; }
        [Required] public DateTime? DateOfBirth { get; set; }
        public string? SSN { get; set; }
        public string? Prefix { get; set; }
        public string? Suffix { get; set; }
        public string? MaidenName { get; set; }
        public string? AlternateName { get; set; }
        public int? SexualOrientation { get; set; }
        public string? OtherSexualOrientation { get; set; }
        [Required] public int GenderIdentity { get; set; }
        public string? OtherGenderIdentity { get; set; }
        public int? PreferredLanguage { get; set; }
        public int? LanguageAbility { get; set; }
        public int? LanguageProficiency { get; set; }
        public int? CountryOfParish { get; set; }
        public int? Religion { get; set; }
        public string? Deceased { get; set; }
        public string? CauseOfDeath { get; set; }
        public DateTime? DeceasedDate { get; set; }
        public string? MotherMaidenName { get; set; }
        public int? Status { get; set; }
        public string? ProfilePic { get; set; }
        public int? CurrentGender { get; set; }
        public int? MaritalStatus { get; set; }
        public bool? IsStatusActive { get; set; }
        public bool? RaceDeclinedToSpecify { get; set; }
        public bool? EthnicityDeclinedToSpecify { get; set; }
        public int? OccupationIndustry { get; set; }
        public int? Occupation { get; set; }
        public int? TribalAffiliation { get; set; }
        public int? SerialNo { get; set; }
        public int? BirthOrder { get; set; }
        public long? ACOKey { get; set; }


        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[CountryParishCode]
        /// </summary>

        public int? CPCId { get; set; }
        public string? CPCParsihName { get; set; }


        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientAlert]
        /// </summary>

        public long PAId { get; set; }
        public string? PAlertNote { get; set; }

        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientInsurancePreference]
        /// </summary>

        public long PIPId { get; set; }
        public int? PIPPreferredInsuranceMethod { get; set; }

        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientNotes]
        /// </summary>

        public long PNId { get; set; }
        public string? PNNotes { get; set; }

        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientImmunizationRegistryConsents]
        /// </summary>

        public long PIRCId { get; set; }
        public int? PIRCRegistryStatus { get; set; }
        public int? PIRCNotificationPreference { get; set; }
        public bool PIRCProtectInfo { get; set; }
        public int PIRCProtectInfoStatus { get; set; }
        public DateTime? PIRCEffectiveDate { get; set; }


        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientContactPreference]
        /// </summary>

        public long PCPID { get; set; }
        public int? PCPPreferredContactMethod { get; set; }
        public bool PCPSmsReminders { get; set; }
        public bool PCPEmailReminders { get; set; }
        public int PCPReminderAnticipationHours { get; set; }
        public bool? PCPNoEmail { get; set; }
        public bool? PCPNoPhone { get; set; }
        public DateTimeOffset? PCPCreateAt { get; set; }
        public DateTimeOffset? PCPUpdatedAt { get; set; }



        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientEmail]
        /// </summary>


        public long PEID { get; set; }
        public string PEEmailAddress { get; set; }
        public int PEEmailAddressType { get; set; }

        /// <summary>
        /// Fields Returned from SP : [patient].[GetPatientById] for Table: [dbo].[PatientAddress]
        /// </summary>

        public long PAID { get; set; }
        public string PAAddressLine1 { get; set; }
        public string PAAddressLine2 { get; set; }
        public string PACity { get; set; }
        public string PAState { get; set; }
        public string PAZip { get; set; }
        public int? PAAddressType { get; set; }
        public int? PAAddressStatus { get; set; }
        public DateTime? PAStartDate { get; set; }
        public DateTime? PAEndDate { get; set; }

    }
}
