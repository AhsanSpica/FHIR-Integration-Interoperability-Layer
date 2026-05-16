using Hl7.Fhir.Utility;
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
    public class ProblemCondtionInfrastructure : IProblemCondtionInfrastructure
    {
        public IProblemConditionRepository _patientProblemRepository;
        private readonly IFhirService _fhirService;

        public ProblemCondtionInfrastructure(IProblemConditionRepository problemConditionRepository,
            IFhirService fhirService) 
        {
            _patientProblemRepository = problemConditionRepository;
            _fhirService = fhirService;

        }
        public async Task<List<PatientProblem>> GetPatientProblemById(long? patientId = null, long? problemId = null, string? tableName = null, long? encounterId =null)
        {
            var result = await _patientProblemRepository.GetPatientProblemById(patientId , problemId,  tableName );

            foreach (var item in result)
            {
                if (item.EncounterId.HasValue )
                {
                    item.EncounterReference = _fhirService.GetResourceReference(item.EncounterId, "Encounter", "");
                }
                if (item.ProviderId.HasValue)
                {
                    item.PractitionerReference = _fhirService.GetResourceReference(item.ProviderId, "Practitioner", "");
                }
                if (!string.IsNullOrEmpty(item.PatientMrn))
                {
                    item.PatientReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn.ToString());
                }
            }

            return result;
        }
    }
}
