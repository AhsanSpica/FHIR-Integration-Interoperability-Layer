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
    public class VitalInfrastructure : IVitalsInfrastructure
    {
        private readonly IVitalsRepository _vitalsRepository;
        private readonly IFhirService _fhirService;

        public VitalInfrastructure(IVitalsRepository vitalsRepository,
            IFhirService fhirService)
        {
            _vitalsRepository = vitalsRepository;
            _fhirService = fhirService;
        }

       public async Task<List<EncounterPatientVitalDto>> PatientVitalsSessionViewModels(long? vitalId, long? encounterId, bool GroupCurrentEncounterVitals)
        {
            var result = await _vitalsRepository.PatientVitalsSessionViewModels(vitalId, encounterId, GroupCurrentEncounterVitals);
          
            foreach (var item in result)
            {
                if (item.EncounterId.HasValue)
                {
                    item.EncounterReference = _fhirService.GetResourceReference(item.EncounterId, "Encounter", item.PatientMrn.ToString()); 
                }
                // item.PractitionerReference = _fhirService.GetResourceReference(item.p, "Practitioner", item.PatientMrn.ToString());
                if (!string.IsNullOrEmpty(item.PatientMrn))
                {
                    item.PatientReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn.ToString());
                }
            }

            return result;
        }
    }
}
