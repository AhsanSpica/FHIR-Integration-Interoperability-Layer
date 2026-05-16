using Interface.Models.ProviderModels;
using IPractitionerInfrastructure;
using IPractitionerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractitionerService
{
    public class ProviderService : IProviderService
    {
        private readonly IProviderInfrastructure _providerInfrastructure;

        public ProviderService(IProviderInfrastructure providerInfrastructure)
        {
            _providerInfrastructure = providerInfrastructure;

        }
        public async Task< GetProviderResponse> GetProviderV2(long ProviderId)
        {
            return await _providerInfrastructure.GetProviderV2(ProviderId);
        }
    }
}
