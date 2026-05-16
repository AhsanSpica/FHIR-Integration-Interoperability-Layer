using FHIR.Interface.Helpers;
using FHIRMappers;
using GlobalHelpers;
 using Hl7.Fhir.Model;
using Interface.Misc.Helpers;
using Interface.Models.BackgroundServices;
using Interface.Models.InterfaceModels;
using IPatientService;


namespace PatientMapper
{
    public class PatientMapper : IPatientMapper.IPatientMapper

    {
        private IPatientProfileService _patientProfileService;
        private readonly LookUpScoped _lookUpScoped; 
        private readonly string typeName = "Patient";
        private readonly IFhirSerializer.IFhirSerializer _fhirSerializer;
        public PatientMapper(LookUpScoped lookUpScoped,  IPatientProfileService patientProfileService, IFhirSerializer.IFhirSerializer fhirSerializer)
        {
            _patientProfileService = patientProfileService;
            _lookUpScoped = lookUpScoped;
            _lookUpScoped.FetchAllLookup();
            _fhirSerializer = fhirSerializer;
        }
       
        public Bundle MapSync(PatientResourceRecords inputs)
        {
            var patientInfoDTO  =   _patientProfileService.GetByID(inputs.PatientId.Value, inputs.PracticeId.Value).GetAwaiter().GetResult();
            var customBundle = new Bundle();
            var count = 0;
            if (patientInfoDTO != null)
                {       
                    PatientFhirR4MappingProfile patientInfoMapper = new PatientFhirR4MappingProfile(_lookUpScoped);
                    var mappedPatient = patientInfoMapper.MapToPatient(patientInfoDTO);
               
                try { 
                    customBundle = new Bundle
                    {
                        Entry = new List<Bundle.EntryComponent>(),
                        Type = Bundle.BundleType.Transaction,
                        // Meta = new Meta { LastUpdated = DateTimeOffset.Now }
                    };

                    var jsonString = _fhirSerializer.FhirR4SerializeResource(mappedPatient);
                    var deserialized = _fhirSerializer.FhirR4DeSerialize(jsonString);

                    customBundle.Entry.Add(new Bundle.EntryComponent
                    {
                        Resource = deserialized,
                        Request = new Bundle.RequestComponent { Method = Bundle.HTTPVerb.POST, Url = "Patient" }
                    });
                }

            catch (Exception ex)
            {
                    var given = mappedPatient.Name.FirstOrDefault().Given.FirstOrDefault();
                    var family = mappedPatient.Name.FirstOrDefault().Family.FirstOrDefault();
                HelperMethods.CreateConsoleLog($"Patient mapping expection for {given+" "+family} {ex.Message}");
            }
                    //   customBundle.Total = customBundle.Entry.Count;
                    //   customBundle.Id = Guid.NewGuid().ToString();
                }
           
                return customBundle;
        }
      
    }
}
