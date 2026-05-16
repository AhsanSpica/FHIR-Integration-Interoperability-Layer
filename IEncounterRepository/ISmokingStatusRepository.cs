using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEncounterRepository
{
    public interface ISmokingStatusRepository
    {
        Task<List<SmokingStatusDTO>> GetSmokingByPatientId(long patientId, long encounterid);
    }
}
