using Interface.Models.ProviderModels;
using IPractitionerInfrastructure;
using IPractitionerRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PractitionerInfrastructure
{
    public class ProviderInfrastructure : IProviderInfrastructure
    {
        private readonly IProviderRepository _providerRepository;
        public ProviderInfrastructure(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }
        public async Task<GetProviderResponse?> GetProviderV2(long ProviderId)
        {
             
                return await _providerRepository.GetProviderV2(ProviderId);

            
        }
    }
}
