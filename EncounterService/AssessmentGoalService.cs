using Hl7.Fhir.Model;
using IEncounterInfrastructure;
using IEncounterService;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace EncounterService
{
    public class AssessmentGoalService :IAssessmentGoalService
    {
        private readonly IAssessmentGoalInfrastructure _assessmentGoalInfrastructure;
        public AssessmentGoalService (IAssessmentGoalInfrastructure assessmentGoalInfrastructure)
        {
            _assessmentGoalInfrastructure = assessmentGoalInfrastructure;
        }

        

        public async Task<List<GoalMasterResponse>> GoalGetByEncounterId(long? patientId = null, long? goalId = null)
        {
            using (var tranScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var goals = await _assessmentGoalInfrastructure.GoalGetByEncounterId(patientId, goalId);

                foreach (var goal in goals)
                {
                    var goalItems = await _assessmentGoalInfrastructure.GetGoalItemByPlanId(goal.Id);
                    goal.GoalItems = goalItems.ToList();
                }

                tranScope.Complete();

                return goals;
            }
        }
    }
}
