using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;
using IEncounterInfrastructure;
using IEncounterRepository;
using IEncounterService;
using Interface.Misc.Interfaces;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterInfrastructure
{
    public class EncounterInfrastructure : IEncounterInfrastructure.IEncounterInfrastructure
    {
        private readonly IEncounterRepository.IEncounterRepository _encounterRepository;
        private readonly IFhirService _fhirService;

        public EncounterInfrastructure(IFhirService fhirService,
            IEncounterRepository.IEncounterRepository encounterRepository
          )
        {
            _encounterRepository = encounterRepository;
            _fhirService = fhirService;

        }
        public async Task<Tuple<IEnumerable<EncounterInfoDto>, IEnumerable<int>, IEnumerable<int>>> GetPatientEncountersPaged(long patientId)
        {
            return await _encounterRepository.GetPatientEncountersPaged(patientId);
        }

        public async Task<EncounterInfoDto>  GetEncounterById(long? Id)
        {
            var encounter = await _encounterRepository.GetEncounterById(Id);
          //  var condition = await _problemConditionService.GetPatientProblemById(null,null,null,encounter.Id);

            if (encounter.AppointmentId.HasValue )
            {
                encounter.AppointmentReference = _fhirService.GetResourceReference((long)encounter.AppointmentId, "Appointment", "");
            }

            if (encounter.LocationId.HasValue)
            {
                encounter.LocationReference = _fhirService.GetResourceReference((long)encounter.LocationId, "Location", "");
            }

            if (!string.IsNullOrEmpty(encounter.PatientMrn ))
            {
                encounter.PatientReference = _fhirService.GetResourceReference((long)encounter.PatientId, "Patient", encounter.PatientMrn);
            }

            //::TODO Update : foolowing reference ot be addded when encounter update is working 
            //::Not Described by Product Requirement to update ENcoutner with condtions after they are created

            //var encounter-GUID = await _fhirService.GetEncounterGUID(encounter.Id);
            //var conditions = await _fhirService.GetCOnditionReferencingEncounter(encounter-GUID)
            //List<Encounter.DiagnosisComponent> diagnosis = new List<Encounter.DiagnosisComponent> ()  ;         
            //foreach(var condition in conditions )
            //{
            //    diagnosis.Add(new Encounter.DiagnosisComponent
            //    {
            //        Condition = new 
            //    }
            //    );    

            return encounter;
        }

        public async Task<PatientLatestEncounter> GetPatientLatestEncounter(long PatientId)
        {
            return await _encounterRepository.GetPatientLatestEncounter(PatientId);
        }
    }
}
