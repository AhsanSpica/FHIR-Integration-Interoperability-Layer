using Dapper;
using Hl7.Fhir.Model;
using IEncounterRepository;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.EncounterModels;
using Interface.Models.InterfaceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class SmokingStatusRepository : ISmokingStatusRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;
        public SmokingStatusRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

           public async Task<List<SmokingStatusDTO>> GetSmokingByPatientId(long patientId, long encounterid)
        {
            var _param = new DynamicParameters(); 
            _param.Add("@PatientId", patientId);
            _param.Add("@EncounterId", encounterid);
            var result = await _dBAccessFHIR.GetAll<SmokingStatusDTO>("GetSmokingByPatientIdFHIR",
            _param, _dBAccess.GetConnectionString(),
            System.Data.CommandType.StoredProcedure);

            return result;
        }
    }
}
