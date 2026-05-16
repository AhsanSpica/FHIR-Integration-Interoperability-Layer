 
using EHR.Models.Patients;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.Patients;
using IPatientsRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientsRepository
{
    public class PatientsPhoneRepository : IPatientsPhoneRepository
    {

        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;

        public PatientsPhoneRepository(IDBAccess dBAccess,
            DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

        public async Task<long> Add(List<PatientPhone> patientPhones)
        {   
            var PatPhones = ListToDataTableConverter.ToDataTable<PatientPhone>(patientPhones);
            PatPhones.Columns.Remove("TotalRows");
            PatPhones.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientPhones", PatPhones, DbType.Object);
            return await _dBAccess.Insert<long>("dbo.CreatePatientPhone", _params,_dBAccess.GetConnectionString(),System.Data.CommandType.StoredProcedure);
        }

        public async Task<long?> Delete(long Id)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@Id", Id);
            _params.Add("@IsDeleted", 1);
            _params.Add("@DeletedId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            await _dBAccess.Update<long>("dbo.DeletePatientPhone", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
            var deletedId = _params.Get<long>("@DeletedId");
            return deletedId == 0 ? (long?)null : deletedId;

        }

        public async Task<PatientPhone> GetByPatientID(long patientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", patientId);

            return await _dBAccess.Get<PatientPhone>(
                            "dbo.GetByIdPatientPhone",
                            _params, _dBAccess.GetConnectionString(),
                            System.Data.CommandType.StoredProcedure
                          );
        }

        public async Task<PatientPhone> Update(List<PatientPhone> patientPhones)
        {
            var PatPhones = ListToDataTableConverter.ToDataTable<PatientPhone>(patientPhones);
            PatPhones.Columns.Remove("TotalRows");
            PatPhones.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientPhones", PatPhones, DbType.Object);
            return await _dBAccess.Update<PatientPhone>("[patient].[UpdatePhone]", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);

        }                                                                                               

        public async Task<List<PatientPhone>> GetListByPatientID(long patientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", patientId);

            return await _dBAccessFHIR.GetAll<PatientPhone>(
                            "GetByIdPatientPhoneFHIR",
                            _params, _dBAccess.GetConnectionString(),
                            System.Data.CommandType.StoredProcedure
                          );
        }
    }
}
