using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.Procedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureRepository
{
    public class ProcedureRepository : IProcedureRepository.IProcedureRepository
    {   
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;

        public ProcedureRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }
       public async Task<List<CombinedProcedureDTO>> GetCombinedProcedures(long? PatientId = null, long? procedureId = null, string? tableName = null)
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", PatientId);
            _params.Add("@ProcedureId", procedureId);
            _params.Add("@TableName", tableName);
            return await _dBAccessFHIR.GetAll<CombinedProcedureDTO>("GetCombinedProceduresFHIR", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);

        }
    }
}
