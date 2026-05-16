using EHR.Models.Patients;
using Interface.Models.Patients;

namespace IPatientsInfrasturcture
{
    public interface IPatientCareTeamMemberInfrastructure
    {

        Task<List<PatientCareTeamMember>> GetAll();



        Task<List<PatientCareTeamMember>> GetByCareTeamID(long careTeamId,long patientId);


        Task<PatientCareTeamMember> GetByPatientID(long patienId);

        Task<List<PatientCareTeamMember>> GetListByPatientID(long patienId);
    }
}