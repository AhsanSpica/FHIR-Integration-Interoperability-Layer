
 
using EHR.Models.Patients;
using Interface.Models.Patients;
using IPatientsInfrastructure;
using IPatientsInfrasturcture;
using IPatientsRepository;

namespace PatientsInfrastructure
{
    public class PatientsInfoInfrastructure : IPatientsInfoInfrastructure
    {
        private readonly IPatientsInfoRepository _patientsInfoRepository;

        public PatientsInfoInfrastructure(IPatientsInfoRepository patientsInfoRepository)
        {
            _patientsInfoRepository = patientsInfoRepository;
        }
       

        public async Task<List<PatientInfo>> GetAll()
        {
            return await _patientsInfoRepository.GetAll();
        }

        public async Task<PatientInfo> GetByID(long PatientId)
        {
            return await _patientsInfoRepository.GetByID(PatientId);
        }

        
      
        public async Task<Tuple<IEnumerable<PatientInfoListItem>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>, IEnumerable<PatientInsurancePreference> >> GetPatientDtoCollaborateMd(string patientMRN)
        {
            return await _patientsInfoRepository.GetPatientDtoCollaborateMd(patientMRN);
        }
        public async Task<Tuple<IEnumerable<PatientInfo>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>>> FindDuplicatePatients(PatientInfo patientInfos)
        {
            return await _patientsInfoRepository.FindDuplicatePatients(patientInfos);
        }
 

        public async Task<PatientInfo> GetPatientInfoById(long patientId, long PracticeId)
        {
            var patientInfoResult = await _patientsInfoRepository.GetPatientInfoById(patientId, PracticeId);
            if(patientInfoResult == null)
            {
                return null!;
            }
            return patientInfoResult.MapPatientInfo();
        }
        #region sync patient info to ACO_staging
        public async Task<bool> SyncPatientInfoToACO(PatientInfo patientInfo)
        {
            return await _patientsInfoRepository.SyncPatientInfoToACO(patientInfo);
        }
        #endregion
    }
}