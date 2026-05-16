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
    public class SmokingStatusInfrastructure : ISmokingStatusInfrastructure
    {
        private readonly ISmokingStatusRepository _smokingStatusRepository;
        private readonly IFhirService _fhirService;
        public SmokingStatusInfrastructure(ISmokingStatusRepository smokingStatusRepository,
            IFhirService fhirService)
        {
            _smokingStatusRepository = smokingStatusRepository;
            _fhirService = fhirService;

        }
      public async  Task<List<SmokingStatusDTO>> GetSmokingByPatientId(long patientId, long encounterid)
        {
              
            var result = await _smokingStatusRepository.GetSmokingByPatientId(patientId, encounterid);

            foreach (var item in result)
            {
                if (item.EncounterId.HasValue)
                {
                    item.EncounterReference = _fhirService.GetResourceReference(item.EncounterId, "Encounter", "");
                }
                //  item.PractitionerReference = _fhirService.GetResourceReference(item., "Practitioner", item.PatientMrn.ToString());
                if (!string.IsNullOrEmpty(item.PatientMrn))
                {
                    item.PatientReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn.ToString());
                }
            }

            return result;
        }
    }
}
