using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Criterion
{
    public class CredentialResponse
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public int client_secret_expires_at { get; set; }
        public string[] redirect_uris { get; set; }
        public string token_endpoint_auth_method { get; set; }
        public string[] grant_types { get; set; }
        public string[] response_types { get; set; }
        public string client_name { get; set; }
        public string logo_uri { get; set; }
        public string[] contacts { get; set; }
    }
}
