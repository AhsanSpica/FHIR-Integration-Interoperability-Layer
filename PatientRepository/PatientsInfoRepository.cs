
using EHR.Models.Patients;
using Interface.Misc.Implementation;
using Interface.Misc.Interfaces;
using Interface.Models.Common;
using Interface.Models.Patients;
using IPatientsRepository;
using System.Data;
using System.Data.Common;


namespace PatientsRepository
{
    public class PatientsInfoRepository : IPatientsInfoRepository
    {

        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;

        public PatientsInfoRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

      
        public async Task<List<PatientInfo>> GetAll()
        {
            var _params = new Dapper.DynamicParameters();

            return await _dBAccess.GetAll<PatientInfo>("dbo.get_patients", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

        public async Task<PatientInfo> GetByID(long PatientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", PatientId);
            return await _dBAccess.Get<PatientInfo>("dbo.get_patients", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

     
        public async Task<Tuple<IEnumerable<PatientInfoListItem>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>, IEnumerable<PatientInsurancePreference>>> GetPatientDtoCollaborateMd(string patientMRN)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientMrn", patientMRN);
          
            return await _dBAccess.GetAllMultiple5<PatientInfoListItem,  PatientAddress, PatientPhone, PatientEmail, PatientContactPreference, PatientInsurance, PatientInsurancePreference>("dbo.GetPatientDtoCollaborateMd", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

        public async Task<Tuple<IEnumerable<PatientInfo>, IEnumerable<PatientAddress>, IEnumerable<PatientPhone>, IEnumerable<PatientEmail>, IEnumerable<PatientContactPreference>, IEnumerable<PatientInsurance>>> FindDuplicatePatients(PatientInfo patientInfos)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@FirstName", patientInfos.FirstName);
            _params.Add("@LastName", patientInfos.LastName);
            _params.Add("@DateOfBirth", (patientInfos.DateOfBirth == null ? null : patientInfos.DateOfBirth.Value.ToString("MM/dd/yyyy")));
            _params.Add("@BirthSex", patientInfos.BirthSex);
            _params.Add("@PracticeId", patientInfos.PracticeId);
            return await _dBAccess.GetAllMultiple4<PatientInfo, PatientAddress, PatientPhone, PatientEmail, PatientContactPreference, PatientInsurance>("dbo.FindDuplicatePatients", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

        public async Task<PatientInfoById> GetPatientInfoById(long patientId, long PracticeId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", patientId);
            _params.Add("@PracticeId", PracticeId);
            return await _dBAccessFHIR.Get<PatientInfoById>("GetPatientByIdFHIR", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }
        #region sync patient info to ACO_staging
        public async Task<bool> SyncPatientInfoToACO(PatientInfo patientInfo)
        {
            var patientAddress = patientInfo.PatientAddresses!.FirstOrDefault();
            var patientPhone= patientInfo.PatientPhones!.FirstOrDefault();
            //var patientEmail = patientInfo.PatientEmails!.FirstOrDefault();

            var _params = new Dapper.DynamicParameters();
            _params.Add("@PATIENTID", patientInfo.Id);
            _params.Add("@PracticeID", patientInfo.PracticeId);
            _params.Add("@FirstName", patientInfo.FirstName);
            _params.Add("@LastName", patientInfo.LastName);
            _params.Add("@FullName", patientInfo.FirstName + " " + patientInfo.LastName);
            _params.Add("@MRN", patientInfo.MRN);
            _params.Add("@DateOfBirth", patientInfo.DateOfBirth);
            _params.Add("@GenderIndicator", patientInfo.GenderIdentity);
            _params.Add("@Age", DateTime.Now.Year - patientInfo.DateOfBirth!.Value.Year);
            if (patientAddress != null)
            {
                _params.Add("@AddressLine1", patientAddress.AddressLine1);
                _params.Add("@AddressLine2", patientAddress.AddressLine2);
                _params.Add("@City", patientAddress.City);
                _params.Add("@State", patientAddress.State);
                _params.Add("@Zip", patientAddress.Zip);
            }
            else
            {
                _params.Add("@AddressLine1", string.Empty);
                _params.Add("@AddressLine2", string.Empty);
                _params.Add("@City", string.Empty);
                _params.Add("@State", string.Empty);
                _params.Add("@Zip", string.Empty);
            }
            _params.Add("@AssignedIndicator", "Y");
            _params.Add("@ALIVE", patientInfo.DeceasedDate == null);
            _params.Add("@AliveIndicator", patientInfo.DeceasedDate == null?true:false);
            _params.Add("@DateOfDeath", patientInfo.DeceasedDate);
            if (patientPhone != null) {
                _params.Add("@PatientPhone", patientPhone.PhoneNumber);
            }
            else
            {
                _params.Add("@PatientPhone",string.Empty);
            }
            _params.Add("@CreatedOnDate",patientInfo.CreatedAt);
            _params.Add("@UpdatedOnDate", patientInfo.UpdatedAt);
            _params.Add("@Deleted", false);
            _params.Add("@OrganizationID", 0);
            return await _dBAccess.Insert<bool>("Create_Patients_EMR", _params, _dBAccess.GetConnectionString(DataBaseConnections.ACOStagingDB), System.Data.CommandType.StoredProcedure);
        }
        #endregion
    }
}