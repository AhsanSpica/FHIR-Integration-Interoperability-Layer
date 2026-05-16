using IEncounterInfrastructure;
using IEncounterRepository;
using Interface.Misc.Interfaces;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterInfrastructure
{
    public class MedicationInfrastructure : IMedicationInfrastructure
    {
        private readonly IMedicationRepository _medicationRepository;
        private readonly IFhirService _fhirService;
        public MedicationInfrastructure(IMedicationRepository medicationRepository,
            IFhirService fhirService)
        {
            _medicationRepository = medicationRepository;
            _fhirService = fhirService;
        }
       public async Task<List<ORMChartPrescriptionView>> GetChartPrescriptionView(long encounterId)
        {
            var result = await _medicationRepository.GetChartPrescriptionView(encounterId);
             
            foreach (var item in result)
            {
                if (item.EncounterId.HasValue)
                {
                    item.EncounterResourceReference = _fhirService.GetResourceReference(item.EncounterId, "Encounter", "");
                }
                if (item.ProviderId.HasValue)
                {
                    item.PractitionerResourceReference = _fhirService.GetResourceReference(item.ProviderId, "Practitioner", "");
                }
                if (!string.IsNullOrEmpty(item.PatientMrn))
                {
                    item.PatientResourceReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn.ToString());
                }
            }
            return result;
        }
    }
}
