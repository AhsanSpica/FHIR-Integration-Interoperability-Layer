using IImmunizationRepository;
using Interface.Misc.Interfaces;
using Interface.Misc.Implementation;
using Interface.Models.ImmunizationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImmunizationRepository
{
    public class ImmunizationRepository : IImmunizationRepository.IImmunizationRepository
    {
        private readonly IDBAccess _dBAccess;
        private readonly DBAccessFHIR _dBAccessFHIR;

        public ImmunizationRepository(IDBAccess dBAccess, DBAccessFHIR dBAccessFHIR)
        {
            _dBAccess = dBAccess;
            _dBAccessFHIR = dBAccessFHIR;
        }
        public async Task<List<ImmunizationDTO>> GetAllImmunization(long? PatientId = null, long? ImmunizationId = null)
        
        {
            var _params = new Dapper.DynamicParameters();
            _params.Add("@PatientId", PatientId);
            _params.Add("@ImmunizationId", ImmunizationId);
            return await _dBAccessFHIR.GetAll<ImmunizationDTO>("GetAllImmunizationFHIR", _params, _dBAccess.GetConnectionString(), System.Data.CommandType.StoredProcedure);
        }
    }
}
