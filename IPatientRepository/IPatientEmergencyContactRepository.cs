using EHR.Models.Patients;
using Interface.Models.Patients;

namespace IPatientsRepository
{
    public interface IPatientEmergencyContactRepository
    {
        Task<long> Add(PatientEmergencyContact patient);

        Task<List<PatientEmergencyContact>> GetAll();

        Task<PatientEmergencyContact> Update(PatientEmergencyContact patient);

        Task<PatientEmergencyContact> GetByID(long id);

        Task<PatientEmergencyContact> Delete(long id);

        Task<PatientEmergencyContact> GetByPatientID(long patienId);

        Task<List<PatientEmergencyContact>> GetListByPatientID(long patienId);
        Task<List<PatientEmergencyContact>> GetNextOfKinByPatientId(long patienId);
    }
}