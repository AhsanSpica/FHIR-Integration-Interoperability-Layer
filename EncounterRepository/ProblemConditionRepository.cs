using Dapper;
using IEncounterRepository;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class ProblemConditionRepository : IProblemConditionRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;

        public ProblemConditionRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

        public async Task<List<PatientProblem>> GetPatientProblemById(long? patientId = null, long? problemId = null, string? tableName = null,long? encounterId = null )
        {
            var _param = new DynamicParameters();
            _param.Add("@PatientId", patientId);
            _param.Add("@ProblemId",  problemId);
            _param.Add("@TableName", tableName);
            var patientProblem = await _dBAccessFHIR.GetAll<PatientProblem>("sp_GetPatientProblemByIdFHIR", _param, _dBAccess.GetConnectionString(), CommandType.StoredProcedure);
            return patientProblem;
        }
    }
}
