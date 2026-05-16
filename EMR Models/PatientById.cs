using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMR_Models
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
    }

}
