using Hl7.Fhir.Model;
using IEncounterInfrastructure;
using IEncounterService;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterService
{
    public class MedicationService : IMedicationService
    {
        private readonly IMedicationInfrastructure _medicationInfrastructure ;
        private readonly IEncounterService.IEncounterService _encounterService ;
        public MedicationService(IMedicationInfrastructure medicationInfrastructure, IEncounterService.IEncounterService encounterService)
        {
            _medicationInfrastructure = medicationInfrastructure;
            _encounterService = encounterService;
        }
        public async Task<List<ORMChartPrescriptionView>> GetChartPrescriptionView(long PatientId)
        {
            if (PatientId > 0)
            {
                var encounterId = await _encounterService.GetPatientLatestEncounter(PatientId);
                if (encounterId > 0)
                {
                    var result = await _medicationInfrastructure.GetChartPrescriptionView(encounterId);
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
