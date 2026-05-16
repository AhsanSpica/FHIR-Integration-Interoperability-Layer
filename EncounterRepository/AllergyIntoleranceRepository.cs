using Dapper;
using IEncounterRepository;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.EncounterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class AllergyIntoleranceRepository : IAllergyIntoleranceRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;    
        // private readonly DBAccessFHIR _dBAccessFHIR;
        public AllergyIntoleranceRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }

        public async Task<List<ORMChartAllergyView>> GetAllergiesView(long? encounterId = null, long? chartallergiesid = null)
        {
            var _param = new DynamicParameters();
            _param.Add("@EncounterId", encounterId);
            _param.Add("@allergychartid", chartallergiesid);

            var patAllergs = await _dBAccessFHIR.GetAll<ORMChartAllergyView>("getChartAllergyViewFHIR",
               _param, _dBAccess.GetConnectionString(),
               System.Data.CommandType.StoredProcedure);

            return patAllergs;

        }
    }
}
