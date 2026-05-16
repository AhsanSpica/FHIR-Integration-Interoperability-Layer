using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterService
{
    public interface IEncounterService
    {
        Task<EncounterPagedWrapperModel> GetPatientEncountersPaged(long patientId);
        Task<EncounterInfoDto> GetEncounterById(long? Id);
        Task<long> GetPatientLatestEncounter(long PatientId);
    }
}
