using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterRepository
{
    public interface IVitalsRepository
    {
        Task<List<EncounterPatientVitalDto>>
            PatientVitalsSessionViewModels(long? vitalId, long? PatientId, bool GroupCurrentEncounterVitals);
    }
}
