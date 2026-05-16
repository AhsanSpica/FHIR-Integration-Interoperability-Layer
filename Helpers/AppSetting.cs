using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalHelpers
{
    public class AppSetting
    {
        public int BundlePOSTTimeOut { get; set; }
        public bool IsBundlePOST { get; set; }
        public bool IsBackGroundService { get; set; }
        public string EMRBaseURL { get; set; }
    }
    public class DatabaseSettings
    {
        public string Schema { get; set; }
    }

    public class AzureAdSettings
    {
        public string Instance {  get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

    }

    public class LocalFhirSettings
    {
        public string Instance { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
    public class AzureFhirServiceSettings
    {
        public string AzureUrl { get; set; }
        public string LocalFhir {  get; set; }
        public bool IsLocalFHIR { get; set; }
    }
    public class SmartScopes
    {
        public List<string> PatientScopes { get; set; }
        public List<string> UserScopes { get; set; }
        public List<string> OtherScopes { get; set; }
    }
    public class CriterionApiSettings
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string CRITERIONDOMAINURL { get; set; }
        public string FHIRPATH { get; set; }
        public string FHIRAUTHPATH { get; set; }
         public string RedirectUri { get; set; }
        public string Scopes { get; set; }
        public string GrantType { get; set; }
    }
    public class USProxySettings
    {
        public string ProxyUrl { get; set; }
        public string ProxyPort { get; set; }
    }
    public class AzureServiceBus
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}
