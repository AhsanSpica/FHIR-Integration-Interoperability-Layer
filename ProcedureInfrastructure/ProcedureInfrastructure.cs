using Interface.Misc.Interfaces;
using Interface.Models.Procedure;
using IProcedureInfrastructure;
using IProcedureRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcedureInfrastructure
{
    public class ProcedureInfrastructure : IProcedureInfrastructure.IProcedureInfrastructure
    {
        private readonly IProcedureRepository.IProcedureRepository _repository;
        private readonly IFhirService _fhirService;

        public ProcedureInfrastructure (IProcedureRepository.IProcedureRepository procedureRepository,
            IFhirService fhirService)
        { 
            _repository = procedureRepository;
            _fhirService = fhirService;
        }
      public async  Task<List<CombinedProcedureDTO>> GetCombinedProcedures(long? PatientId = null, long? procedureId = null, string? tableName = null)
        {
            var result = await _repository.GetCombinedProcedures(PatientId, procedureId, tableName);

            foreach (var item in result)
            {
                if (item.EncounterId.HasValue)
                {
                    item.EncounterReference = _fhirService.GetResourceReference(item.EncounterId, "Encounter", item.PatientMrn.ToString());
                }
                // item.PractitionerReference = _fhirService.GetResourceReference(item.id, "Practitioner", item.PatientMrn.ToString());
                if (!string.IsNullOrEmpty(item.PatientMrn.ToString()))
                {
                    item.PatientReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn.ToString());
                }
            }

            return result;
        }
    }
}
