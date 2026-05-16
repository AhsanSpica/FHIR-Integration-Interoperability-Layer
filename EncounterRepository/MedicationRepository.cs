using Dapper;
using IEncounterRepository;
using Interface.Misc.Implementation;
using Interface.Misc.Interfaces;
using Interface.Models.Medication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncounterRepository
{
    public class MedicationRepository : IMedicationRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;
        public MedicationRepository(IDBAccess dBAccess,
            DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }
        public async Task<List<ORMChartPrescriptionView>> GetChartPrescriptionView(long encounterId)
        {
            var _param = new DynamicParameters();
            _param.Add("@encounterId", encounterId);

            var patMeds = await _dBAccessFHIR.GetAll<ORMChartPrescriptionView>("GetChartPrescriptionViewFHIR",
         
            _param, _dBAccess.GetConnectionString(),
            System.Data.CommandType.StoredProcedure);

            return patMeds;

        }
    }
}
