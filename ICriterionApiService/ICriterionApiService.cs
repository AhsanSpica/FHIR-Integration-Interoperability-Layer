using Hl7.Fhir.Model;
using Interface.Models.Auth;
using Interface.Models.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICriterionApiService
{
    public interface ICriterionApiService
    { 
       
        Task<TokenResponse> GenerateTokenAsync();
        Task<Resource> GetResourceAsync(string resourceType, int resourceId);
        Task<CredentialResponse> RegisterClientAsync();
        Task<string> GetAuthorizationUrlAsync(CredentialResponse credentials);


    }
}
