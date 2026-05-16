 
using EHR.Models.Patients;
using Interface.Misc.Helpers;
using Interface.Misc.Implementation;
using Interface.Misc.Interfaces;
using Interface.Models.Patients;
using IPatientsRepository;

namespace PatientsRepository
{
    public class PatientCareTeamRepository : IPatientCareTeamRepository
    {
        public readonly IDBAccess _dBAccess;
        public readonly DBAccessFHIR _dBAccessFHIR;
       
 
        public PatientCareTeamRepository(IDBAccess dBAccess,
            DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

         
        public async Task<List<PatientCareTeamAndMemberResponseModel>> GetAll(long patientId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patientId != null || patientId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patientId));
            }
            return await _dBAccess.GetAll<PatientCareTeamAndMemberResponseModel>
                ("USP_GETALL_PatientCareTeam", _params, _dBAccess.GetConnectionString());
        }

        public async Task<PatientCareTeam> GetByID(long id)
        {
            var _params = new Dapper.DynamicParameters();

            if (id != null || id != 0)
            {
                _params.Add("@Id", HelperMethods.ReturnLongValue(id));
            }

            return await _dBAccess.Get<PatientCareTeam>
                ("USP_GETBYID_PatientCareTeam", _params, _dBAccess.GetConnectionString());
        }

  

        public async Task<PatientCareTeam> GetByPatientID(long patienId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patienId != null || patienId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patienId));
            }

            return await _dBAccess.Get<PatientCareTeam>
                ("USP_GETBYPATIENTID_PatientCareTeam", _params, _dBAccess.GetConnectionString());
        }

        public async Task<List<PatientCareTeam>> GetListByPatientID(long? patientId = null, long? careTeamId = null)
        {
            var _params = new Dapper.DynamicParameters();

                _params.Add("@PatientId", patientId);
            _params.Add("@CareTeamId", careTeamId);


            return await _dBAccessFHIR.GetAll<PatientCareTeam>
                ("USP_GETBYPATIENTID_PatientCareTeam", _params, _dBAccess.GetConnectionString());
        }
    }
}