using Hl7.Fhir.Model;
using IEncounterInfrastructure;
using IEncounterRepository;
using Interface.Misc.Helpers;
using Interface.Misc.Implementation;
using Interface.Misc.Interfaces;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterInfrastructure
{
    public class AllergyIntoleranceInfrastructure : IAllergyIntoleranceInfrastructure
    {
        private readonly IAllergyIntoleranceRepository _allergyIntoleranceRepository;
        private readonly IFhirService _fhirService;
        public AllergyIntoleranceInfrastructure(IAllergyIntoleranceRepository allergyIntoleranceRepository
            , IFhirService fhirService
            )
        {
            _allergyIntoleranceRepository = allergyIntoleranceRepository;
            _fhirService = fhirService;
        }
        public async Task<List<ORMChartAllergyView>> GetAllergiesView(long? encounterId = null, long? chartallergiesid = null)
        {
            var result = await _allergyIntoleranceRepository.GetAllergiesView(encounterId , chartallergiesid );
            //foreach (var item in result)
            //{
                
            //  item.EncounterResourceReference = _fhirService.GetResourceReference(item.EncounterId,"Encounter", item.PatientMrn);
            //  item.PractitionerResourceReference = _fhirService.GetResourceReference(item.ProviderId, "Practitioner", item.PatientMrn);
            //  item.PatientResourceReference = _fhirService.GetResourceReference(item.PatientId, "Patient", item.PatientMrn);
            //}

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
