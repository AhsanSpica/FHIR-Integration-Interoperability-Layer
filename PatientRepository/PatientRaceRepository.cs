 
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
    public class PatientRaceRepository : IPatientRaceRepository
    {

        private readonly IDBAccess _dBAccess;

        public PatientRaceRepository(IDBAccess dBAccess)
        {
            _dBAccess = dBAccess;
        }   

        public async Task<long> Add(List<PatientRace> patientRaces)
        {
            var races = ListToDataTableConverter.ToDataTable<PatientRace>(patientRaces);
            races.Columns.Remove("TotalRows");
            races.Columns.Remove("RaceName");
            races.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientRace", races, DbType.Object);
            return await _dBAccess.Insert<long>("dbo.CreatePatientRace", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

        public async Task<long?> Delete(long Id)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@Id", Id);
            _params.Add("@DeletedId", dbType: System.Data.DbType.Int64, direction: System.Data.ParameterDirection.Output);
            _params.Add("@IsDeleted", true);
            await _dBAccess.Delete<long>("dbo.DeletePatientRace", _params, _dBAccess.GetConnectionString(),System.Data.CommandType.StoredProcedure);
            var deletedId = _params.Get<long>("@DeletedId");
            return deletedId == 0 ? (long?)null : deletedId;
        }

        public async Task<List<PatientRace>> GetListByPatientID(long PatientId)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("PatientId", PatientId);
            return await _dBAccess.GetAll<PatientRace>("dbo.GetByIdPatientRace", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }

        public async Task<PatientRace> Update(List<PatientRace> patientRace)
        {
            var races = ListToDataTableConverter.ToDataTable<PatientRace>(patientRace);
            races.Columns.Remove("TotalRows");
            races.Columns.Remove("RaceName");
            races.Columns.Remove("TotalCount");
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientRace", races, DbType.Object);
            return await _dBAccess.Insert<PatientRace>("[patient].[UpdateRace]", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }
    }
}
