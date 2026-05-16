
using EHR.Models.Patients;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
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
    public class PatientEthnicityRepository : IPatientEthnicityRepository              
    {

        private readonly IDBAccess _dBAccess;

        public PatientEthnicityRepository(IDBAccess dBAccess)
        {
             _dBAccess = dBAccess;
        }                                                

        public async Task<long> Add(List<PatientEthnicity> patientEthnicities)
        {
            var Ethnicities = ListToDataTableConverter.ToDataTable<PatientEthnicity>(patientEthnicities);
            Ethnicities.Columns.Remove("TotalRows");
            Ethnicities.Columns.Remove("EthnicityName");
            Ethnicities.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientEthnicity", Ethnicities, DbType.Object);
            return await _dBAccess.Insert<long>("dbo.CreatePatientEthnicity", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }                  

        public async Task<long?> Delete(long Id)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@Id", Id);
            _params.Add("@DeletedId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            _params.Add("@IsDeleted", true);
            await _dBAccess.Delete<long>("dbo.DeletePatientEthnicity", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
            var deletedId = _params.Get<long>("@DeletedId");
            return deletedId == 0 ? (long?)null : deletedId;
        }  

        public async Task<List<PatientEthnicity>> GetListByPatientID(long PatientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", PatientId);
            return await _dBAccess.GetAll<PatientEthnicity>("dbo.GetByIdPatientEthnicity", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

        public async Task<PatientEthnicity> Update(List<PatientEthnicity> patientEthnicity)
        {
            var Ethnicities = ListToDataTableConverter.ToDataTable<PatientEthnicity>(patientEthnicity);
            Ethnicities.Columns.Remove("TotalRows");
            Ethnicities.Columns.Remove("EthnicityName");
            Ethnicities.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientEthnicity", Ethnicities, DbType.Object);
            return await _dBAccess.Update<PatientEthnicity>("patient.UpdateEthnicity", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);              
        }
    }
}
