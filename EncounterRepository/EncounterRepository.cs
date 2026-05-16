using Dapper;
using GlobalHelpers;
using Hl7.Fhir.Utility;
using IEncounterRepository;
using Interface.Misc.Implementation;
using Interface.Misc.Interfaces;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class EncounterRepository : IEncounterRepository.IEncounterRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;
         public EncounterRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR )
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
           //  _lookUpScoped.FetchAllLookup(); no need to hit db as lookUpScoped is used here to fetch Constant Properties
        }
        public async Task<Tuple<IEnumerable<EncounterInfoDto>, IEnumerable<int>, IEnumerable<int>>> GetPatientEncountersPaged(long patientId)
        {
            var _params = new DynamicParameters();
            _params.Add("@OrderColumn", "dateOfService");
            _params.Add("@OrderDirection", "DESC");
            _params.Add("@PageNum", 1);
            _params.Add("@PageSize", 10);
            _params.Add("@PatientId", patientId);
            _params.Add("@PracticeId", (long) CommonUseProperties.PracticeId);
            _params.Add("@Auth0UserId", CommonUseProperties.UserAuth0Id);
            var result =  await _dBAccessFHIR.GetAllMultiple1<EncounterInfoDto, int, int>("GetPatientEncountersPagedFHIR", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
            return result;
        }
        public async Task<EncounterInfoDto> GetEncounterById(long? Id)
        {
            var _params = new DynamicParameters();
            _params.Add("@Id", Id);
            return await _dBAccessFHIR.Get<EncounterInfoDto>("GetEncounterByIdFHIR", _params, _dBAccess.GetConnectionString(), CommandType.StoredProcedure);
        }
        public async Task<PatientLatestEncounter> GetPatientLatestEncounter(long PatientId)
        {
            var _params = new DynamicParameters();
            _params.Add("@PatientId", PatientId);
            return await _dBAccess.Get<PatientLatestEncounter>("dbo.sp_GetPatientLatestEncounter",
                _params, _dBAccess.GetConnectionString(), CommandType.StoredProcedure);
        }
    }
}
