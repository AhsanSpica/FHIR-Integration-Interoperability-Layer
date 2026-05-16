using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterRepository
{
    public interface IAssessmentGoalRepository
    {
        Task<List<GoalMasterResponse>> GoalGetByEncounterId(long? patientId = null, long? goalId = null);
        Task<List<GoalItemResponse>> GetGoalItemByPlanId(long GoalId);
        // Task<List<GoalItemResponse>> GetGoalItemById(long? goalItemId = null);
    }
}
