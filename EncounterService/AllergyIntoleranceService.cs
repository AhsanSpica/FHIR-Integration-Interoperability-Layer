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
    public class AllergyIntoleranceService : IAllergyIntoleranceService
    {
        private readonly IAllergyIntoleranceInfrastructure _allergyIntoleranceInfrastructure;
        private readonly IEncounterInfrastructure.IEncounterInfrastructure _encounterInfrastructure;
        public AllergyIntoleranceService(IAllergyIntoleranceInfrastructure allergyIntoleranceInfrastructure,
            IEncounterInfrastructure.IEncounterInfrastructure encounterInfrastructure)
        {
            _allergyIntoleranceInfrastructure = allergyIntoleranceInfrastructure;
            _encounterInfrastructure = encounterInfrastructure;

        }
        public async Task<List<ORMChartAllergyView>> GetAllergiesViewSingular( long? chartallergiesid = null)
        { 
                 var result = await _allergyIntoleranceInfrastructure.GetAllergiesView(null,chartallergiesid);
                    if (result != null && result.Count > 0)
                    {
                        return result;
                    }
               
            return null!;
        }
        public async Task<List<ORMChartAllergyView>> GetAllergiesView(long patientId)
        {
            if (patientId > 0)
            {
                var encounterId = await _encounterInfrastructure.GetPatientLatestEncounter(patientId);
                if (encounterId.EncounterId > 0)
                {
                    var result = await _allergyIntoleranceInfrastructure.GetAllergiesView(encounterId.EncounterId);
                    if (result != null && result.Count > 0)
                    {
                        return result;
                    }
                }
            }
            return null!;
        }
    }
    }

