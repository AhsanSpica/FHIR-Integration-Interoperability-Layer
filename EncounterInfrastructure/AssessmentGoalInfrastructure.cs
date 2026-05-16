using Hl7.Fhir.Model;
using IEncounterInfrastructure;
using IEncounterRepository;
using Interface.Misc.Interfaces;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterInfrastructure
{
    public class AssessmentGoalInfrastructure : IAssessmentGoalInfrastructure
    {
        private readonly IAssessmentGoalRepository _assessmentGoalRepository;

        private readonly IFhirService _fhirService;
        public AssessmentGoalInfrastructure(IAssessmentGoalRepository assessmentGoalRepository,
            IFhirService fhirService)
        {
            _assessmentGoalRepository = assessmentGoalRepository;
            _fhirService = fhirService;
        }
        public async Task<List<GoalMasterResponse>> GoalGetByEncounterId(long? patientId = null, long? goalId = null)
        {
            return await _assessmentGoalRepository.GoalGetByEncounterId(patientId, goalId);
        }
        
        public async Task<List<GoalItemResponse>> GetGoalItemByPlanId(long GoalId)
        {
            var goalItem =  await _assessmentGoalRepository.GetGoalItemByPlanId(GoalId);
            foreach (var goal in goalItem)
            {
               
                if (!string.IsNullOrEmpty(goal.PatientMrn))
                {
                    goal.PatientReference = _fhirService.GetResourceReference(goal.PatientId.Value, "Patient", goal.PatientMrn);
                }
            }
            
            return goalItem;
        }
       
    }
}
