using EHR.Models.Patients;
using Interface.Models.Patients;

namespace IPatientsRepository
{
    public interface IPatientCareTeamRepository
    {
       
        Task<List<PatientCareTeamAndMemberResponseModel>> GetAll(long patientId);
 

        Task<PatientCareTeam> GetByID(long id);

        

        Task<PatientCareTeam> GetByPatientID(long patienId);

        Task<List<PatientCareTeam>> GetListByPatientID(long? patienId = null, long? careTeamId = null);
    }
}