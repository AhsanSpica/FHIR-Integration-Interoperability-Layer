
using EHR.Models.Patients;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Models.Patients;
using IPatientsRepository;

namespace PatientsRepository
{
    public class PatientCareTeamMemberRepository : IPatientCareTeamMemberRepository
    {
        private readonly IDBAccess _dBAccess;
       
        const string getIdSP = "USP_GETBYID_PatientCareTeamMember";
        const string getAllSP = "USP_GETALL_PatientCareTeamMember";
        const string getCareTeamSP = "USP_GETCARETEAMBYID_PatientCareTeamMember";
 
        public PatientCareTeamMemberRepository(IDBAccess dBAccess)
        {
            _dBAccess = dBAccess;
        }

     

        public async Task<List<PatientCareTeamMember>> GetAll()
        {
            return await _dBAccess.GetAll<PatientCareTeamMember>
               (getAllSP, null, _dBAccess.GetConnectionString());
        }


       
        public async Task<List<PatientCareTeamMember>> GetByCareTeamID(long careTeamId, long patientId)
        {
            var _params = new Dapper.DynamicParameters();

            if (careTeamId != null && careTeamId != 0)
            {
                _params.Add("@CareTeamId", HelperMethods.ReturnLongValue(careTeamId));
            }

            return await _dBAccess.GetAll<PatientCareTeamMember>
               (getCareTeamSP, _params, _dBAccess.GetConnectionString());
        }

        public async Task<PatientCareTeamMember> GetByPatientID(long patienId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patienId != null || patienId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patienId));
            }

            return await _dBAccess.Get<PatientCareTeamMember>
               ("USP_GETBYPATIENTID_PatientCareTeamMember", _params, _dBAccess.GetConnectionString());
        }

        public async Task<List<PatientCareTeamMember>> GetListByPatientID(long patienId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patienId != null || patienId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patienId));
            }

            return await _dBAccess.GetAll<PatientCareTeamMember>
               ("USP_GETBYPATIENTID_PatientCareTeamMember", _params, _dBAccess.GetConnectionString());
        }
    }
}