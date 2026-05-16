using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterRepository
{
    public interface IEncounterRepository
    {
        Task<Tuple<IEnumerable<EncounterInfoDto>, IEnumerable<int>, IEnumerable<int>>> GetPatientEncountersPaged(long patientId);
        Task<EncounterInfoDto> GetEncounterById(long? Id);
        Task<PatientLatestEncounter> GetPatientLatestEncounter(long PatientId);
    }
}
