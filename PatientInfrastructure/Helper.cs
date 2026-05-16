using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using EHR.Models.Patients;
using Interface.Models.Patients;

namespace PatientsInfrastructure
{
    public static class Helper
    {
        public static PatientInfo MapPatientInfo(this PatientInfoById patientInfoById)
        {
            var mapResult = new PatientInfo();
            
            #region Patient Info Mapping

            mapResult.Id = patientInfoById.Id;
            mapResult.MRN = patientInfoById.MRN;
            mapResult.PracticeId = patientInfoById.PracticeId;
            mapResult.IsCustomMRN = patientInfoById.IsCustomMRN;
            mapResult.FirstName = patientInfoById.FirstName;
            mapResult.LastName = patientInfoById.LastName;
            mapResult.MiddleInitial = patientInfoById.MiddleInitial;
            mapResult.BirthSex = patientInfoById.BirthSex;
            mapResult.DateOfBirth = patientInfoById.DateOfBirth;
            mapResult.SSN = patientInfoById.SSN;
            mapResult.Prefix = patientInfoById.Prefix;
            mapResult.Suffix = patientInfoById.Suffix;
            mapResult.MaidenName = patientInfoById.MaidenName;
            mapResult.AlternateName = patientInfoById.AlternateName;
            mapResult.SexualOrientation = patientInfoById.SexualOrientation;
            mapResult.OtherSexualOrientation= patientInfoById.OtherSexualOrientation;
            mapResult.GenderIdentity = patientInfoById.GenderIdentity;
            mapResult.OtherGenderIdentity= patientInfoById.OtherGenderIdentity;
            mapResult.PreferredLanguage = patientInfoById.PreferredLanguage;
            mapResult.LanguageAbility = patientInfoById.LanguageAbility;
            mapResult.LanguageProficiency = patientInfoById.LanguageProficiency;
            mapResult.CountryOfParish = patientInfoById.CountryOfParish;
            mapResult.Religion = patientInfoById.Religion;
            mapResult.Deceased = patientInfoById.Deceased;
            mapResult.CauseOfDeath = patientInfoById.CauseOfDeath;
            mapResult.DeceasedDate = patientInfoById.DeceasedDate;
            mapResult.MotherMaidenName = patientInfoById.MotherMaidenName;
            mapResult.Status = patientInfoById.Status;
            mapResult.ProfilePic = patientInfoById.ProfilePic;
            mapResult.CurrentGender = patientInfoById.CurrentGender;
            mapResult.MaritalStatus = patientInfoById.MaritalStatus;
            mapResult.RaceDeclinedToSpecify = patientInfoById.RaceDeclinedToSpecify;
            mapResult.EthnicityDeclinedToSpecify = patientInfoById.EthnicityDeclinedToSpecify;
            mapResult.Occupation = patientInfoById.Occupation;
            mapResult.OccupationIndustry = patientInfoById.OccupationIndustry;
            mapResult.TribalAffiliation = patientInfoById.TribalAffiliation;
            mapResult.SerialNo = patientInfoById.SerialNo;
            mapResult.BirthOrder = patientInfoById.BirthOrder;
            mapResult.ParishName = patientInfoById.CPCParsihName;
            mapResult.IsStatusActive = patientInfoById.IsStatusActive;
            mapResult.ACOKey = patientInfoById.ACOKey;
            #endregion

            #region Patient Alert Mapping

            mapResult.patientAlerts.Add(new PatientAlert() 
            {
                Id = patientInfoById.PAId,
                PatientId = patientInfoById.Id,
                Note = patientInfoById.PAlertNote
            });

            #endregion

            #region Preferred Insurance Preference Method Mapping

            mapResult.patientInsurancePreference.Add(new PatientInsurancePreference()
            {
                Id = patientInfoById.PIPId,
                PatientId = patientInfoById.Id,
                PreferredInsuranceMethod = patientInfoById.PIPPreferredInsuranceMethod
            });

            #endregion

            #region Patient Notes Mapping

            mapResult.PatientNotes.Add(new PatientNote()
            {
                Id = patientInfoById.PNId,
                PatientId = patientInfoById.Id,
                Notes = patientInfoById.PNNotes
            });

            #endregion

            #region Patient Immunization Registry Consents Mapping

            mapResult.PatientImmunizationRegistryConsents.Add(new PatientImmunizationRegistryConsent()
            {
                Id = patientInfoById.PIRCId,
                PatientId = patientInfoById.Id,
                RegistryStatus = patientInfoById.PIRCRegistryStatus,
                NotificationPreference = patientInfoById.PIRCNotificationPreference,
                ProtectInfo = patientInfoById.PIRCProtectInfo,
                ProtectInfoStatus = patientInfoById.PIRCProtectInfoStatus,
                EffectiveDate = patientInfoById.PIRCEffectiveDate
            });

            #endregion

            #region Patient Contact Preference Mapping

            mapResult.PatientContactPreference.Id = patientInfoById.PCPID;
            mapResult.PatientContactPreference.PatientId = patientInfoById.Id;
            mapResult.PatientContactPreference.PreferredContactMethod = patientInfoById.PCPPreferredContactMethod;
            mapResult.PatientContactPreference.SmsReminders = patientInfoById.PCPSmsReminders;
            mapResult.PatientContactPreference.EmailReminders = patientInfoById.PCPEmailReminders;
            mapResult.PatientContactPreference.ReminderAnticipationHours = patientInfoById.PCPReminderAnticipationHours;
            mapResult.PatientContactPreference.NoEmail = patientInfoById.PCPNoEmail;
            mapResult.PatientContactPreference.NoPhone = patientInfoById.PCPNoPhone;
            mapResult.PatientContactPreference.CreatedAt = patientInfoById.PCPCreateAt;
            mapResult.PatientContactPreference.UpdatedAt = patientInfoById.PCPUpdatedAt;
            #endregion

            #region Patient Email Address Mapping

            mapResult.PatientEmails.Add(new PatientEmail()
            {
                Id = patientInfoById.PEID,
                PatientId = patientInfoById.Id,
                EmailAddress = patientInfoById.PEEmailAddress,
                EmailAddressType = patientInfoById.PEEmailAddressType
            });

            #endregion

            return mapResult;
        }
    }
}
