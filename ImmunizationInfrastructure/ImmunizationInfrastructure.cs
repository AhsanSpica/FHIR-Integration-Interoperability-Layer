using Hl7.Fhir.Model;
using IImmunizationInfrastructure;
using IImmunizationRepository;
using Interface.Misc.Interfaces;
using Interface.Models.ImmunizationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmunizationInfrastructure
{
    public class ImmunizationInfrastructure : IImmunizationInfrastructure.IImmunizationInfrastructure
    {
        private readonly IImmunizationRepository.IImmunizationRepository _immunizationRepository;
        private readonly IFhirService _fhirService;

        public ImmunizationInfrastructure(IImmunizationRepository.IImmunizationRepository immunizationRepository,
            IFhirService fhirService)
        {
            _immunizationRepository = immunizationRepository;
            _fhirService = fhirService;
        }

        
        public async Task<List<ImmunizationDTO>> GetAllImmunization(long? PatientId = null, long? ImmunizationId = null)
        {
            var result = await _immunizationRepository.GetAllImmunization(PatientId ,  ImmunizationId);

            foreach (var item in result)
            {
                 //  item.EncounterReference = _fhirService.GetResourceReference(item.EncounterId, "Encounter", item.PatientMrn.ToString());
                if (item.Facility.HasValue)
                {
                    item.LocationReference = _fhirService.GetResourceReference(item.Facility, "Location", "");
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
