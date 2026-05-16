using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterService
{
    public interface IAssessmentGoalService
    {
        Task<List<GoalMasterResponse>> GoalGetByEncounterId(long? patientId = null, long? goalId = null);
    }
}
