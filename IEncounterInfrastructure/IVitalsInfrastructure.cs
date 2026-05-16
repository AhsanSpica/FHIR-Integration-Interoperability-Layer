using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterInfrastructure
{
    public interface IVitalsInfrastructure
    {
        Task<List<EncounterPatientVitalDto>>
            PatientVitalsSessionViewModels(long? vitalId, long? encounterid, bool GroupCurrentEncounterVitals = false);
    }
}
