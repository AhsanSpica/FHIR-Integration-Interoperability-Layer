using GlobalHelpers;
using IEncounterRepository;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.EncounterModels;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class VitalsRespository : IVitalsRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;
     
        public VitalsRespository(IDBAccess dBAccess,
            
            DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
        
            _dBAccessFHIR = dBAccessFHIR;
        }
       public async Task<List<EncounterPatientVitalDto>> 
            PatientVitalsSessionViewModels(long? vitalId, long? encounterId, bool GroupCurrentEncounterVitals)
        {
            var _params = new Dapper.DynamicParameters();
             _params.Add("@EncounterId", encounterId);
            _params.Add("@VitalId", vitalId);
            //   _params.Add("@GroupCurrentEncounterVitals", GroupCurrentEncounterVitals); 
            return await _dBAccessFHIR.GetAll<EncounterPatientVitalDto>
               ("GetLastNVitalsSessionsFHIR", _params, _dBAccess.GetConnectionString());
        }
    }
}
