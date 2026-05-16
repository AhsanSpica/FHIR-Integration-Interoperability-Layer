using Dapper;
using GlobalHelpers;
using Interface.Misc.Interfaces;
using Interface.Models.ProviderModels;
using IPractitionerRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractitionerRepository
{
    public class ProviderRepository :IProviderRepository
    {
        private readonly IDBAccess _dBAccess;        

        public ProviderRepository(IDBAccess dBAccess)
        {
            _dBAccess = dBAccess;
        }

        public async Task<GetProviderResponse?> GetProviderV2(long ProviderId)
        {
            var _param = new DynamicParameters();
            _param.Add("@ProviderId", ProviderId);

            var result = await _dBAccess.Get<GetProviderResponse?>("sp_GetProvidersV2",
         _param, _dBAccess.GetConnectionString(),
         System.Data.CommandType.StoredProcedure);
            return result;
        }
    }
}
