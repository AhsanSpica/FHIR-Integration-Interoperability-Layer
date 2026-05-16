
using IGeneralLookUpService;
using Interface.Models.GeneralLookups;

namespace GlobalServices
{
    public class LookUpScoped
    {
        private readonly IGeneralLookupService _generalLookupService;
        public  Dictionary<string, List<GeneralLookup>> MaritalStatus;
        public Dictionary<string, List<GeneralLookup>> Race;
        public Dictionary<string, List<GeneralLookup>> Ethnicity;
        public Dictionary<string, List<GeneralLookup>> Gender;
        public Dictionary<string, List<GeneralLookup>> GenderIdentity;
        public Dictionary<string, List<GeneralLookup>> SexualOrientation;
        public Dictionary<string, List<GeneralLookup>> TribalAffiliation;
        public Dictionary<string, List<GeneralLookup>> Language;
        public Dictionary<string, List<GeneralLookup>> PhoneNumberType; 
        private  int PracticeId = 87825;
        private readonly string Lang = "en-US";
        
        private   List<string> PatientFHIRTemplateLookup = new List<string>();

        public LookUpScoped(IGeneralLookupService generalLookupService)
        {
            _generalLookupService = generalLookupService;
            PatientFHIRTemplateLookup.Add("MaritalStatus");
            PatientFHIRTemplateLookup.Add("Race");
            PatientFHIRTemplateLookup.Add("Ethnicity");
            PatientFHIRTemplateLookup.Add("Gender");
            PatientFHIRTemplateLookup.Add("GenderIdentity");
            PatientFHIRTemplateLookup.Add("SexualOrientation");
            PatientFHIRTemplateLookup.Add("TribalAffiliation");
            PatientFHIRTemplateLookup.Add("Language");
            PatientFHIRTemplateLookup.Add("PhoneNumberType");
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
            Staging();
        }
        public void Staging()
        {
            var staging = "staging";

        }
        public GeneralLookup GetLanguages(int code)
        {
            if (Language != null && Language.Any())
            {
                var statusList = Language.GetValueOrDefault("Language");

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
