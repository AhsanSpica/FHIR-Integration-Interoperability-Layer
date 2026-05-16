 
using EHR.Models.Patients;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Models.Patients;
using IPatientsRepository;

namespace PatientsRepository
{
    public class PatientEmergencyContactRepository : IPatientEmergencyContactRepository
    {
        private readonly IDBAccess _dBAccess;
        const string insertSP = "USP_Insert_PatientEmergencyContact";
        const string updateSP = "USP_Update_PatientEmergencyContact";
        const string deleteSP = "USP_DELETE_PatientEmergencyContact";
        const string getIdSP = "USP_GETBYID_PatientEmergencyContact";
        const string getAllSP = "USP_GETALL_PatientEmergencyContact";
        const string getPatientSP = "USP_GETBYPATIENTID_PatientEmergencyContact";

        public PatientEmergencyContactRepository(IDBAccess dBAccess)
        {
            _dBAccess = dBAccess;
        }

        public async Task<long> Add(PatientEmergencyContact patient)
        {
            var _params = new Dapper.DynamicParameters();
            if (patient.PatientId != null || patient.PatientId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patient.PatientId));
            }
            _params.Add("@CreatedBy", patient.CreatedBy);
            //_params.Add("@UpdatedBy", patient.UpdatedBy);
            _params.Add("@CreatedAt", DateTime.Now);
            //_params.Add("@UpdatedAt", DateTime.Now);
            _params.Add("@IsDeleted", 0);
            _params.Add("@FirstName", patient.FirstName.Trim());
            _params.Add("@MiddleName", patient.MiddleName.Trim());
            _params.Add("@LastName", patient.LastName.Trim());
            _params.Add("@PhoneNumber", patient.PhoneNumber);
            _params.Add("@AddressLine1", patient.AddressLine1);
            _params.Add("@AddressLine2", patient.AddressLine2);
            _params.Add("@City", patient.City);
            _params.Add("@State", patient.State);
            _params.Add("@Zip", patient.Zip);
            _params.Add("@RelationToPatient", patient.RelationToPatient);
            _params.Add("@EmailAddress", patient.EmailAddress);
            _params.Add("@Guarantor", patient.Guarantor);
            _params.Add("@NextofKin", patient.NextofKin);
            _params.Add("@OfficeNumber", patient.OfficeNumber);

            return await _dBAccess.Insert<long>
               (insertSP, _params, _dBAccess.GetConnectionString());
        }

        public async Task<PatientEmergencyContact> Delete(long id)
        {
            var _params = new Dapper.DynamicParameters();

            if (id != null || id != 0)
            {
                _params.Add("@Id", HelperMethods.ReturnLongValue(id));
            }

            return await _dBAccess.Delete<PatientEmergencyContact>
               (deleteSP, _params, _dBAccess.GetConnectionString());
        }

        public async Task<List<PatientEmergencyContact>> GetAll()
        {
            return await _dBAccess.GetAll<PatientEmergencyContact>
               (getAllSP, null, _dBAccess.GetConnectionString());
        }

        public async Task<PatientEmergencyContact> GetByID(long id)
        {
            var _params = new Dapper.DynamicParameters();

            if (id != null || id != 0)
            {
                _params.Add("@Id", HelperMethods.ReturnLongValue(id));
            }

            return await _dBAccess.Get<PatientEmergencyContact>
               (getIdSP, _params, _dBAccess.GetConnectionString());
        }

        public async Task<PatientEmergencyContact> Update(PatientEmergencyContact patient)
        {
            var _params = new Dapper.DynamicParameters();

            if (patient.Id != null || patient.Id != 0)
            {
                _params.Add("@Id", HelperMethods.ReturnLongValue(patient.Id));
            }

            if (patient.PatientId != null || patient.PatientId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patient.PatientId));
            }

            //_params.Add("@CreatedBy", patient.CreatedBy);
            _params.Add("@UpdatedBy", patient.UpdatedBy);
            //_params.Add("@CreatedAt", DateTime.Now);
            _params.Add("@UpdatedAt", DateTime.Now);
            _params.Add("@IsDeleted", 0);
            _params.Add("@FirstName", patient.FirstName);
            _params.Add("@MiddleName", patient.MiddleName);
            _params.Add("@LastName", patient.LastName);
            _params.Add("@PhoneNumber", patient.PhoneNumber);
            _params.Add("@AddressLine1", patient.AddressLine1);
            _params.Add("@AddressLine2", patient.AddressLine2);
            _params.Add("@City", patient.City);
            _params.Add("@State", patient.State);
            _params.Add("@Zip", patient.Zip);
            _params.Add("@RelationToPatient", patient.RelationToPatient);
            _params.Add("@EmailAddress", patient.EmailAddress);
            _params.Add("@Guarantor", patient.Guarantor);
            _params.Add("@NextofKin", patient.NextofKin);
            _params.Add("@OfficeNumber", patient.OfficeNumber);

            return await _dBAccess.Update<PatientEmergencyContact>
               (updateSP, _params, _dBAccess.GetConnectionString());
        }

        public async Task<PatientEmergencyContact> GetByPatientID(long patienId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patienId != null || patienId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patienId));
            }

            return await _dBAccess.Get<PatientEmergencyContact>
               (getPatientSP, _params, _dBAccess.GetConnectionString());
        }

        public async Task<List<PatientEmergencyContact>> GetListByPatientID(long patienId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patienId != null || patienId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patienId));
            }

            return await _dBAccess.GetAll<PatientEmergencyContact>
               (getPatientSP, _params, _dBAccess.GetConnectionString());
        }
        public async Task<List<PatientEmergencyContact>> GetNextOfKinByPatientId(long patienId)
        {
            var _params = new Dapper.DynamicParameters();

            if (patienId != null || patienId != 0)
            {
                _params.Add("@PatientId", HelperMethods.ReturnLongValue(patienId));
            }

            return await _dBAccess.GetAll<PatientEmergencyContact>
               ("getNextofKinByPatientId", _params, _dBAccess.GetConnectionString());
        }
         
    }
}