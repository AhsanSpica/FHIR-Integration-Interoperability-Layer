using EHR.Models.Patients;
using Interface.Models.Patients;
 using IPatientsInfrastructure;
using IPatientsRepository;

namespace PatientsInfrastructure
{
    public class PatientCareTeamInfrastructure : IPatientCareTeamInfrastructure
    {
        public readonly IPatientCareTeamRepository _patientCareTeamRepository;
        public PatientCareTeamInfrastructure(IPatientCareTeamRepository 
                                              patientCareTeamRepository)
        {
            _patientCareTeamRepository = patientCareTeamRepository;
        }

     

        public async Task<List<PatientCareTeamAndMemberResponseModel>> GetAll(long patienId)
        {
            return await _patientCareTeamRepository.GetAll(patienId);
        }

        public async Task<PatientCareTeam> GetByID(long id)
        {
            return await _patientCareTeamRepository.GetByID(id);
        }

        public async Task<PatientCareTeam> GetByPatientID(long patienId)
        {
            return await _patientCareTeamRepository.GetByPatientID(patienId);
        }

        public async Task<List<PatientCareTeam>> GetListByPatientID(long? patienId = null, long? careTeamId = null)
        {
            return await _patientCareTeamRepository.GetListByPatientID(patienId ,  careTeamId );
        }

        
    }
}