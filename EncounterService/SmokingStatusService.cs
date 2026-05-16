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
    public class SmokingStatusService : ISmokingStatusService
    {
        private readonly ISmokingStatusInfrastructure _smokingStatusInfrastructure;
        public SmokingStatusService(ISmokingStatusInfrastructure smokingStatusInfrastructure)
        {
            _smokingStatusInfrastructure = smokingStatusInfrastructure;
        }

        public async Task<List<SmokingStatusDTO>>  GetSmokingByPatientId(long patientId, long encounterid)
        {
            return await _smokingStatusInfrastructure.GetSmokingByPatientId( patientId,  encounterid);
        }
    }
}
