using IEncounterRepository;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class AssessmentGoalRepository : IAssessmentGoalRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;
        public AssessmentGoalRepository(IDBAccess dBAccess
            , DBAccessFHIR dBAccessFHIR
            )
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }
        public async Task<List<GoalItemResponse>> GetGoalItemByPlanId(long GoalId)
        {
            var _params = new Dapper.DynamicParameters();

            if (GoalId != null || GoalId != 0)
            {
                _params.Add("@GoalId", HelperMethods.ReturnLongValue(GoalId));
            }

            return await _dBAccessFHIR.GetAll<GoalItemResponse>(
                "USP_GETByGoalId_AssessmentGoalItem",
                _params,
                _dBAccess.GetConnectionString()
            );
        }
         
        public async Task<List<GoalMasterResponse>> GoalGetByEncounterId(long? patientId = null, long? goalId = null)
        {
                var _params = new Dapper.DynamicParameters();
             
                    _params.Add("@PatientId", patientId);
                          _params.Add("@GoalId", goalId);
             
                return await _dBAccessFHIR.GetAll<GoalMasterResponse>(
                    "GetPatientGoalsByPatient",
                    _params,
                    _dBAccess.GetConnectionString()
                );
            
        }
    }
}
