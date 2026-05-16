using Interface.Models.ProviderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPractitionerService
{
    public interface IProviderService
    {
        Task<GetProviderResponse?> GetProviderV2(long ProviderId);
    }
}
