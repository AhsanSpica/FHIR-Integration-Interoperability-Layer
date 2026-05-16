using EHR.Models.Patients;
using Interface.Misc.Interfaces;
using Interface.Models.Patients;
using IPatientsInfrasturcture;
using IPatientsRepository;

namespace PatientsInfrastructure
{
    public class PatientCareTeamMemberInfrastructure : IPatientCareTeamMemberInfrastructure
    {
        private readonly IPatientCareTeamMemberRepository _patientCareTeamMemberRepository;
        private readonly IFhirService _fhirService;
        public PatientCareTeamMemberInfrastructure
        (
            IPatientCareTeamMemberRepository patientCareTeamMemberRepository,
            IFhirService fhirService
        )
        {
            _patientCareTeamMemberRepository = patientCareTeamMemberRepository;
            _fhirService = fhirService;
        }

      

        public async Task<List<PatientCareTeamMember>> GetAll()
        {
            return await _patientCareTeamMemberRepository.GetAll();
        }

        public async Task<List<PatientCareTeamMember>> GetByCareTeamID(long careTeamId, long patientId)
        {
            return await _patientCareTeamMemberRepository.GetByCareTeamID(careTeamId, patientId);
        }

       

        

        public async Task<PatientCareTeamMember> GetByPatientID(long patientId)
        {
            return await _patientCareTeamMemberRepository.GetByPatientID(patientId);
        }

        public async Task<List<PatientCareTeamMember>> GetListByPatientID(long patientId)
        {
              
            var result = await _patientCareTeamMemberRepository.GetListByPatientID(patientId);

            //foreach (var item in result)
            //{
            //    //  item.EncounterReference = _fhirService.GetResourceReference(item.id, "Encounter", item.PatientMrn.ToString());
            //       item.PractitionerReference = _fhirService.GetResourceReference(item.ProviderReferenceId, "Practitioner", item.PatientMrn.ToString());
            //    item.PatientReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn.ToString());
            //}

            return result;
        }
    }
}