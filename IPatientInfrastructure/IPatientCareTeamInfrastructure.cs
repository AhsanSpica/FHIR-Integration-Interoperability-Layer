using EHR.Models.Patients;
using Interface.Models.Patients;

namespace IPatientsInfrastructure
{
    public interface IPatientCareTeamInfrastructure
    {
       
        Task<List<PatientCareTeamAndMemberResponseModel>> GetAll(long patienId);

         

        Task<PatientCareTeam> GetByID(long id);

 
        Task<PatientCareTeam> GetByPatientID(long patienId);

        Task<List<PatientCareTeam>> GetListByPatientID(long? patienId = null, long? careTeamId = null);
    }
}