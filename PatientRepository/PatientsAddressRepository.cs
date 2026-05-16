 
using EHR.Models.Patients;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Models.Patients;
using IPatientsRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PatientsRepository
{
    public class PatientsAddressRepository : IPatientsAddressRepository
    {
        private readonly IDBAccess _dBAccess;

        public PatientsAddressRepository(IDBAccess dBAccess)
        {
            _dBAccess = dBAccess;
        }

        public async Task<long> Add(List<PatientAddress> patientAddresses)
        {
            var address = ListToDataTableConverter.ToDataTable<PatientAddress>(patientAddresses);
            address.Columns.Remove("TotalRows");
            address.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientAddresses", address,DbType.Object) ;
            return await _dBAccess.Insert<long>("dbo.CreatePatientAddresses", _params,_dBAccess.GetConnectionString(),System.Data.CommandType.StoredProcedure);


        }

        public async Task<long?> Delete(long Id)
        {

            var _params = new Dapper.DynamicParameters();
            _params.Add("@Id", Id);
            _params.Add("@IsDeleted", "true");
            _params.Add("@DeletedId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            await _dBAccess.Update<long>("dbo.DeletePatientAddress", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
            var deletedId = _params.Get<long>("@DeletedId");
            return deletedId == 0 ? (long?)null : deletedId;
        }

        public async Task<PatientAddress> GetByPatientID(long patientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", patientId);

            return await _dBAccess.Get<PatientAddress>(
                            "dbo.GetByIdPatientAddress", 
                            _params, _dBAccess.GetConnectionString(), 
                            System.Data.CommandType.StoredProcedure
                          );
        }

        public async Task<PatientAddress> Update(List<PatientAddress> patientAddress)
        {

            var address = ListToDataTableConverter.ToDataTable<PatientAddress>(patientAddress);
            address.Columns.Remove("TotalRows");
            address.Columns.Remove("TotalCount");

            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientAddresses", address, DbType.Object);
            
            return await _dBAccess.Update<PatientAddress>("[patient].[UpdateAddress]",
                _params, 
                _dBAccess.GetConnectionString(), 
                System.Data.CommandType.StoredProcedure);
        }

        public async Task<List<PatientAddress>> GetListByPatientID(long patientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", patientId);

            return await _dBAccess.GetAll<PatientAddress>(
                            "dbo.GetByIdPatientAddress",
                            _params, _dBAccess.GetConnectionString(),
                            System.Data.CommandType.StoredProcedure
                          );
        }
    }
}
