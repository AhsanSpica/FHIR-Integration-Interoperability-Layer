using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using IEncounterInfrastructure;
using IEncounterService;
using Interface.Misc.Helpers;
using Interface.Misc.Interfaces;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterService
{
    public class EncounterService : IEncounterService.IEncounterService
    {
          private readonly IEncounterInfrastructure.IEncounterInfrastructure _encounterInfrastructure;
        private readonly IFhirService _fhirService;

            public EncounterService(

              IEncounterInfrastructure.IEncounterInfrastructure encounterInfrastructure,
              IFhirService fhirService
               )
            {
                _encounterInfrastructure = encounterInfrastructure;
            _fhirService = fhirService;
            }
        public async Task<long> GetPatientLatestEncounter(long PatientId)
        {
            var patientEncounter = await _encounterInfrastructure.GetPatientLatestEncounter(PatientId);
            if (patientEncounter != null)
            {
                return patientEncounter.EncounterId;
            }
            return 0;
        }
        public async Task<EncounterPagedWrapperModel> GetPatientEncountersPaged(long patientId)
        {
            var modal = new EncounterPagedWrapperModel();
            var result = await _encounterInfrastructure.GetPatientEncountersPaged(patientId);

            var encounterList = result.Item1.ToList();

            foreach (var encounter in encounterList)
            {   if (encounter.AppointmentId !=null)
                {
                    encounter.AppointmentReference = _fhirService.GetResourceReference((long)encounter.AppointmentId, "Appointment", encounter.PatientMrn);
                }

                if (encounter.LocationId != null)
                {
                    encounter.LocationReference = _fhirService.GetResourceReference((long)encounter.LocationId, "Location", encounter.PatientMrn);
                }

                if (encounter.PatientId != null)
                {
                    encounter.PatientReference = _fhirService.GetResourceReference((long)encounter.PatientId, "Patient", encounter.PatientMrn);
                }
            }

            modal.EncounterInfos = encounterList;

            modal.TotalSigned = result.Item2.FirstOrDefault();
            modal.TotalUnsigned = result.Item3.FirstOrDefault();
            return modal;
        }
        
        public async Task<EncounterInfoDto> GetEncounterById(long? Id)
        {
                      
            
            return await _encounterInfrastructure.GetEncounterById(Id);
        }
    }
}
