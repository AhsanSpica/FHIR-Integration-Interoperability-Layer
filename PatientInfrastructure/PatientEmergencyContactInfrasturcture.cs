using EHR.Models.Patients;
using Interface.Models.Patients;
using IPatientsInfrastructure;
using IPatientsRepository;

namespace PatientsInfrastructure
{
    public class PatientEmergencyContactInfrasturcture : IPatientEmergencyContactInfrastructure
    {
        private readonly IPatientEmergencyContactRepository _patientEmergencyContactRepository;
        public PatientEmergencyContactInfrasturcture
        (
            IPatientEmergencyContactRepository patientEmergencyContactRepository
        )
        {
            _patientEmergencyContactRepository = patientEmergencyContactRepository;
        }

        public async Task<long> Add(PatientEmergencyContact patient)
        {
            return await _patientEmergencyContactRepository.Add( patient );
        }

        public async Task<PatientEmergencyContact> Delete(long id)
        {
            return await _patientEmergencyContactRepository.Delete( id );
        }

        public async Task<List<PatientEmergencyContact>> GetAll()
        {
            return await _patientEmergencyContactRepository.GetAll();
        }

        public async Task<PatientEmergencyContact> GetByID(long id)
        {
            return await _patientEmergencyContactRepository.GetByID( id );
        }

        public async Task<PatientEmergencyContact> GetByPatientID(long patienId)
        {
            return await _patientEmergencyContactRepository.GetByPatientID( patienId );
        }

        public async Task<PatientEmergencyContact> Update(PatientEmergencyContact patient)
        {
            return await _patientEmergencyContactRepository.Update( patient );
        }

        public async Task<List<PatientEmergencyContact>> GetListByPatientID(long patienId)
        {
            return await _patientEmergencyContactRepository.GetListByPatientID( patienId );
        }
        public async Task<List<PatientEmergencyContact>> GetNextOfKinByPatientId(long patienId)
        {
            return await _patientEmergencyContactRepository.GetNextOfKinByPatientId(patienId);
        }
        
    }
}