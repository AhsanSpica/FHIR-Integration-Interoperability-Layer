using GlobalHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace Interface.Misc.Helpers
{
  
    public class AzureAdTokenService
    {
        private readonly IConfidentialClientApplication _app;
        private readonly string _scope;
        private readonly IOptions<AzureAdSettings> _aadSettings;
        private readonly IOptions<AzureFhirServiceSettings> _afsSettings;
        private readonly IOptions<SmartScopes> _smartScopes;

        public AzureAdTokenService(
            IOptions<AzureAdSettings> aadSettings,
           IOptions<AzureFhirServiceSettings> afsSettings,
           IOptions<SmartScopes> smartScopes)
        {
            _aadSettings = aadSettings;
            _afsSettings = afsSettings;
            _smartScopes = smartScopes;

 
            _scope = $"{_afsSettings.Value.AzureUrl}/.default";

            _app = ConfidentialClientApplicationBuilder.Create(_aadSettings.Value.ClientId)
                .WithClientSecret(_aadSettings.Value.ClientSecret  )
                .WithAuthority ( new Uri( $"{ _aadSettings.Value.Instance }{ _aadSettings.Value.TenantId }" ) )
                .Build();
        }

        public async Task<string> GetTokenAsync()
        {  
            var scopes = new List<string>
        {
            $"{_afsSettings.Value.AzureUrl}/.default"
        };

            scopes.AddRange(_smartScopes.Value.PatientScopes.Select(scope => $"{_afsSettings.Value.AzureUrl}/{scope}"));
            scopes.AddRange(_smartScopes.Value.UserScopes.Select(scope => $"{_afsSettings.Value.AzureUrl}/{scope}"));
            scopes.AddRange(_smartScopes.Value.OtherScopes.Select(scope => $"{_afsSettings.Value.AzureUrl}/{scope}"));
            // var allScopes = scopes;

             var  result = await _app.AcquireTokenForClient(new[] { _scope }).ExecuteAsync();
                       
            //  var   result = await _app.AcquireTokenForClient(scopes).ExecuteAsync();
            
            //catch(Exception ex)
            //{
            //    HelperMethods.CreateConsoleLog($"{ex.Message}");
            //}
            return result.AccessToken;
        }
    }

}
