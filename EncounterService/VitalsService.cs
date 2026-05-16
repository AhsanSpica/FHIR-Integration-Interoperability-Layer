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
    public class VitalsService : IVitalsService
    {
        private readonly IVitalsInfrastructure _vitalsInfrastructure;
        private readonly IEncounterService.IEncounterService _encounterService;

        public VitalsService(IVitalsInfrastructure vitalsInfrastructure,
            IEncounterService.IEncounterService encounterService)
        {
            _vitalsInfrastructure = vitalsInfrastructure;
            _encounterService = encounterService;
        }
           
        
        public async Task<List<EncounterPatientVitalDto>> PatientVitalsSessionViewModels(long? vitalId, long patientId, bool groupCurrentEncounterVitals = false)
        {
           
                var encounterId = await _encounterService.GetPatientLatestEncounter(patientId);
               
                    var result = await _vitalsInfrastructure.PatientVitalsSessionViewModels(vitalId, encounterId, groupCurrentEncounterVitals);

                    //var sessionList = new List<PatientVitalsSession>();
                    //Guid tempSessionId = default(Guid);

                    ////For current Encounter
                    //if (GroupCurrentEncounterVitals == false)
                    //{
                    //    foreach (var epv in result.GroupBy(i => new { i.EncounterId, i.SessionId }))
                    //    {
                    //        sessionList.Add(new PatientVitalsSession
                    //        {
                    //            SessionId = epv.Key.SessionId,
                    //            EncounterId = epv.Key.EncounterId,
                    //            SessionDate = epv.First().SessionDate,
                    //            ListOfPatientVitals = epv.Where(i => i.EncounterId == epv.Key.EncounterId).ToList(),
                    //        });
                    //    }
                    //    foreach (var epv in result.GroupBy(i => i.EncounterId))
                    //    {
                    //        tempSessionId = Guid.NewGuid();
                    //        sessionList.Add(new PatientVitalsSession
                    //        {
                    //            SessionId = tempSessionId,
                    //            EncounterId = epv.Key,
                    //            SessionDate = epv.First().SessionDate,
                    //            ListOfPatientVitals = epv.Where(i => i.EncounterId == epv.Key).ToList(),
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    foreach (var epv in result.GroupBy(i => i.EncounterId))
                    //    {
                    //        tempSessionId = Guid.NewGuid();
                    //        sessionList.Add(new PatientVitalsSession
                    //        {
                    //            SessionId = tempSessionId,
                    //            EncounterId = epv.Key,
                    //            SessionDate = epv.First().SessionDate,
                    //            ListOfPatientVitals = epv.Where(i => i.EncounterId == epv.Key).ToList(),
                    //        });
                    //    }
                    //}

                    //PatientVitalsSessionViewModel sessionViewModel;
                    //var vitalString = new StringBuilder();
                    //foreach (var session in sessionList)
                    //{
                    //    sessionViewModel = new PatientVitalsSessionViewModel();
                    //    sessionViewModel.SessionId = session.SessionId;
                    //    sessionViewModel.EncounterId = session.EncounterId;
                    //    sessionViewModel.SessionDate = session.SessionDate;
                    //    sessionViewModel.PatientVitalViewModels = new List<PatientVitalViewModel>();
                    //    foreach (var vitalGroup in session.ListOfPatientVitals!.GroupBy(i => new { i.VitalTypeId, i.VitalName, i.VitalDesc }))
                    //    {
                    //        vitalString.Clear();
                    //        foreach (var vital in vitalGroup.OrderBy(i => i.VitalSubTypeId))
                    //        {
                    //            if (vitalString.Length > 0)
                    //            {
                    //                vitalString.Append(", ");
                    //            }
                    //            vitalString.Append($"{((vitalGroup.Key.VitalName == vital.VstName) ? "" : vital.VstName + " ")}{vital.Value} {vital.Unit}{(!string.IsNullOrWhiteSpace(vital.SourceText) ? " " + vital.SourceText : "")}{(!string.IsNullOrWhiteSpace(vital.PositionText) ? " " + vital.PositionText : "")}");
                    //        }
                    //        sessionViewModel.PatientVitalViewModels.Add(new PatientVitalViewModel
                    //        {
                    //            VitalName = vitalGroup.Key.VitalName!,
                    //            VitalValue = vitalString.ToString().Trim(',', ' '),
                    //            ListOfPatientVitals = vitalGroup.ToList(),
                    //        });
                    //    }
                    //    sessionListViewModel.Add(sessionViewModel);
                    //}
                
            
            return result;
        }
    }
}
