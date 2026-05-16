
using IGeneralLookUpService;
using Interface.Models.GeneralLookups;

namespace GlobalHelpers
{
    public class LookUpScoped
    {
        private readonly IGeneralLookupService _generalLookupService;
        public Dictionary<string, List<GeneralLookup>> MaritalStatus;
        public Dictionary<string, List<GeneralLookup>> Race;
        public Dictionary<string, List<GeneralLookup>> Ethnicity;
        public Dictionary<string, List<GeneralLookup>> Gender;
        public Dictionary<string, List<GeneralLookup>> GenderIdentity;
        public Dictionary<string, List<GeneralLookup>> SexualOrientation;
        public Dictionary<string, List<GeneralLookup>> TribalAffiliation;
        public Dictionary<string, List<GeneralLookup>> PreferredLanguage;
        public Dictionary<string, List<GeneralLookup>> PhoneNumberType;
        public  List<Speciality> specialtyList;
        public Dictionary<string, List<GeneralLookup>> CareTeamMemberRelation;
        public Dictionary<string, List<GeneralLookup>> VaccineRefusalReason;
        public Dictionary<string, List<GeneralLookup>> Roles;
        public Dictionary<string, List<GeneralLookup>> DocumentType;
        public Dictionary<string, List<GeneralLookup>> VaccineSite;
        public Dictionary<string, List<GeneralLookup>> VaccineRoute;
        public Dictionary<string, List<GeneralLookup>> VaccineFundingSource;
        public Dictionary<string, List<GeneralLookup>> VaccineFundingProgram;
        public Dictionary<string, List<GeneralLookup>> AppointmentType;
        public Dictionary<string, List<GeneralLookup>> EncounterType;


        public  int PracticeId = 87825;
        public string UserAuth0Id = "auth0|65127fbd70d3cfe7ec83c6a6";
        public readonly string Lang = "en-US";
        public readonly string Country = "US";
        
        private   List<string> PatientFHIRTemplateLookup = new List<string>();
       // public int GetPracticeId () { return PracticeId; }
        public LookUpScoped(IGeneralLookupService generalLookupService)
        {
            _generalLookupService = generalLookupService;
            PatientFHIRTemplateLookup.Add("Roles");
        //    PatientFHIRTemplateLookup.Add("DocumentType");
            PatientFHIRTemplateLookup.Add("MaritalStatus");
            PatientFHIRTemplateLookup.Add("Race");
            PatientFHIRTemplateLookup.Add("Ethnicity");
            PatientFHIRTemplateLookup.Add("Gender");
            PatientFHIRTemplateLookup.Add("GenderIdentity");
            PatientFHIRTemplateLookup.Add("SexualOrientation");
            PatientFHIRTemplateLookup.Add("TribalAffiliation");
            PatientFHIRTemplateLookup.Add("PreferredLanguage");
            PatientFHIRTemplateLookup.Add("PhoneNumberType");
            PatientFHIRTemplateLookup.Add("Specialty");
            PatientFHIRTemplateLookup.Add("CareTeamMemberRelation");
            PatientFHIRTemplateLookup.Add("VaccineRefusalReason");
            PatientFHIRTemplateLookup.Add("VaccineSite");
            PatientFHIRTemplateLookup.Add("VaccineRoute");
            PatientFHIRTemplateLookup.Add("VaccineFundingSource");
            PatientFHIRTemplateLookup.Add("VaccineFundingProgram");
            PatientFHIRTemplateLookup.Add("AppointmentType");
            PatientFHIRTemplateLookup.Add("EncounterType");
        }

        public  void FetchAllLookup()
        {
            foreach (var marker in PatientFHIRTemplateLookup)
            {
                var field = GetType().GetField(marker);
                if (field != null)
                { 
                    var value =   _generalLookupService.GetByCriteria(marker, Lang, PracticeId).GetAwaiter().GetResult();
                    field.SetValue(this, value);
                }
            }
            specialtyList = _generalLookupService.GetAllSpecialty().GetAwaiter().GetResult();
        }
        public GeneralLookup GetEncounterType(int code)
        {
            if (EncounterType != null && EncounterType.Any())
            {
                var statusList = EncounterType.GetValueOrDefault("EncounterType");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetAppointmentType(int code)
        {
            if (AppointmentType != null && AppointmentType.Any())
            {
                var statusList = AppointmentType.GetValueOrDefault("AppointmentType");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public Speciality GetSpeciality(int code)
        {
            if (specialtyList != null && specialtyList.Any())
            {
                //var statusList = specialtyList.GetValueOrDefault("Specialty");

                if (specialtyList != null && specialtyList.Any())
                {
                    var status = specialtyList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new Speciality();
        }

        public GeneralLookup GetVaccineRoute(int code)
        {
            if (VaccineRoute != null && VaccineRoute.Any())
            {
                var statusList = VaccineRoute.GetValueOrDefault("VaccineRoute");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public GeneralLookup GetVaccineFundingSource(int code)
        {
            if (VaccineFundingSource != null && VaccineFundingSource.Any())
            {
                var statusList = VaccineFundingSource.GetValueOrDefault("VaccineFundingSource");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetVaccineFundingProgram(int code)
        {
            if (VaccineFundingProgram != null && VaccineFundingProgram.Any())
            {
                var statusList = VaccineFundingProgram.GetValueOrDefault("VaccineFundingProgram");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetVaccineSite(int code)
        {
            if (VaccineSite != null && VaccineSite.Any())
            {
                var statusList = VaccineSite.GetValueOrDefault("VaccineSite");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetVaccineRefusalReason(int code)
        {
            if (VaccineRefusalReason != null && VaccineRefusalReason.Any())
            {
                var statusList = VaccineRefusalReason.GetValueOrDefault("VaccineRefusalReason");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetRoles(int code)
        {
            if (Roles != null && Roles.Any())
            {
                var statusList = Roles.GetValueOrDefault("EMRRole");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetLanguages(int code)
        {
            if (PreferredLanguage != null && PreferredLanguage.Any())
            {
                var statusList = PreferredLanguage.GetValueOrDefault("PreferredLanguage");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }
            return new GeneralLookup();
        }
        public GeneralLookup GetPhoneNumberType(int code)
        {
            if (PhoneNumberType != null && PhoneNumberType.Any())
            {
                var statusList = PhoneNumberType.GetValueOrDefault("PhoneNumberType");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetDocumentType(int? code)
        {
            if (DocumentType != null && DocumentType.Any())
            {
                var statusList = DocumentType.GetValueOrDefault("DocumentType");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public GeneralLookup GetMaritalStatus(int code)
        {
            if (MaritalStatus != null && MaritalStatus.Any())
            {
                var statusList = MaritalStatus.GetValueOrDefault("MaritalStatus");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetRace(int code)
        {
            if (Race != null && Race.Any())
            {
                var statusList = Race.GetValueOrDefault("Race");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public GeneralLookup GetCareTeamMemberRelation(int code)
        {
            if (CareTeamMemberRelation != null && CareTeamMemberRelation.Any())
            {
                var statusList = CareTeamMemberRelation.GetValueOrDefault("CareTeamMemberRelation");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Code.Equals(code.ToString()));

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public GeneralLookup GetEthnicity(int code)
        {
            if (Ethnicity != null && Ethnicity.Any())
            {
                var statusList = Ethnicity.GetValueOrDefault("Ethnicity");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetGenderSex(int code)
        {
            if (Gender != null && Gender.Any())
            {
                var statusList = Gender.GetValueOrDefault("Gender");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
        public GeneralLookup GetBirthSex(int code)
        {
            if (Gender != null && Gender.Any())
            {
                var statusList = Gender.GetValueOrDefault("Gender");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public GeneralLookup GetGenderIdentity(long code)
        {
            if (GenderIdentity != null && GenderIdentity.Any())
            {
                var statusList = GenderIdentity.GetValueOrDefault("GenderIdentity");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }

        public GeneralLookup GetSexualOrientation(int code)
        {
            if (SexualOrientation != null && SexualOrientation.Any())
            {
                var statusList = SexualOrientation.GetValueOrDefault("SexualOrientation");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }
            return new GeneralLookup();
        }

        public GeneralLookup GetTribalAffiliation(int code)
        {
            if (TribalAffiliation != null && TribalAffiliation.Any())
            {
                var statusList = TribalAffiliation.GetValueOrDefault("TribalAffiliation");

                if (statusList != null && statusList.Any())
                {
                    var status = statusList.FirstOrDefault(item => item.Id == code);

                    if (status != null)
                    {
                        return status;
                    }
                }
            }

            return new GeneralLookup();
        }
       
    }
}
