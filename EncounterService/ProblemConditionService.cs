using Hl7.Fhir.Model;
using IEncounterInfrastructure;
using IEncounterService;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterService
{
    public class ProblemConditionService : IProblemConditionService
    {
        private readonly IProblemCondtionInfrastructure _patientProblemInfrastructure;
        private readonly IEncounterService.IEncounterService _encounterService;
        public ProblemConditionService
        (
            IProblemCondtionInfrastructure patientProblemInfrastructure,
            IEncounterService.IEncounterService encounterService
        )
        {
            _patientProblemInfrastructure = patientProblemInfrastructure;
            _encounterService = encounterService;
        }
        public async Task<List<PatientProblem>>  GetPatientProblemById (long? patientId = null, long? problemId = null, string? tableName = null, long? encounterId=null)
        {
               var patientProblemList = await  _patientProblemInfrastructure.GetPatientProblemById( patientId , problemId ,  tableName );

           // foreach( var patientProblem in patientProblemList ) 
           // {
               // var encounter = await _encounterService.GetEncounterById(patientProblem.EncounterId);

                //if (encounter.Id.Equals(patientProblem.EncounterId))
                //            {
                    //    patientProblem.ProviderId = encounter.ProviderId;

                //    }
           // }
               
            return patientProblemList;
        }
    }
}
