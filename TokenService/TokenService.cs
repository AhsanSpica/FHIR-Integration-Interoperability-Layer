using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Newtonsoft.Json.Linq;
using Interface.Models.Auth;
using TokenResponse = Interface.Models.Auth.TokenResponse;

namespace TokenService
{
    public class TokenService : ITokenService.ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRestClient _restClient;
        private readonly string _auth0ClientId;
        private readonly string _auth0ClientSecret;
        private readonly string _auth0Audience;
        private readonly string _domain;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _audience;
        private string _accessToken;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _restClient = new RestClient("https://dev-y4uwv4rv1d453czg.us.auth0.com");
            
            _audience = configuration["Auth0:Audience"];
            _domain = configuration["Auth0:Domain"];
            _clientId = configuration["Auth0:ClientId"];
            _clientSecret = configuration["Auth0:ClientSecret"];
        }
       
        public async Task<TokenResponse> GenerateToken(List<string> scopes)
        {
           // var userId = "auth0|663dae08fd5f2614e1480774";
            try
            {
                var client = new RestClient($"https://{_domain}");
                var request = new RestRequest("/oauth/token", Method.Post);
                request.AddHeader("content-type", "application/json");

                var requestBody = new
                {
                    client_id = _clientId,
                    client_secret = _clientSecret,
                    audience = _audience,
                    grant_type = "client_credentials",
                     //, password = "!Harbinger2024"   
                    scope = string.Join(" ", scopes)
                };
                request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody), ParameterType.RequestBody);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    var errorContent = response.Content;
                    return new TokenResponse { Error = new Error { IsError = true, Message = errorContent } };
                }

                var tokenJson = JObject.Parse(response.Content);
                _accessToken = tokenJson.Value<string>("access_token");
                var expiresIn = tokenJson.Value<string>("expires_in");
                var tokenType = tokenJson.Value<string>("token_type");

                return new TokenResponse { AccessToken = _accessToken, ExpiresIn = expiresIn, TokenType = tokenType, Error = null };
            }
            catch (Exception ex)
            {
                return new TokenResponse { Error = new Error { IsError = true, Message = ex.Message } };
            }
        }

   //   request = new RestRequest($" api/v2/users/{userId}/roles", Method.Post);
               // request.AddHeader("content-type", "application/json");
               // request.AddHeader("authorization", $"{_accessToken}");

               //  List<string> roles = new List<string>();
               // roles.Add("rol_YChxPbn9jM7LXgl8");

               //var requestBody2 = new
               // {  roles = string.Join(" ", roles)
               //};

               // request.AddParameter("application/json", JsonConvert.SerializeObject(requestBody2), ParameterType.RequestBody);
               // var roleResponse = client.Execute(request);
        //{ 
        //    var client = new RestClient();
        //    var request = new RestRequest("/oauth/token", Method.Post);
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", "{\"client_id\":\"Jbqjy0n29FvFgUANWBPgsysKVcwo0I8c\",\"client_secret\":\"A1F3ssquFdAjr5iPL8Z7al4_fyBqt-3zC42KrVTr1FErIHa5cSW2XR7wcYPsN2CI\",\"audience\":\"https://FHIRMiddlewareAPI/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
        //    var response = client.Execute(request);

        //    if (response.IsSuccessful)
        //    {
        //        var content = response.Content;
        //        var tokenJson = JsonConvert.DeserializeObject<JObject>(content);
        //        var accessToken = tokenJson["access_token"].ToString();
        //        var expiresIn = tokenJson["expires_in"].ToString();
        //        var tokenType = tokenJson["token_type"].ToString();
        //        return new TokenResponse { AccessToken = accessToken, ExpiresIn = expiresIn,TokenType = tokenType, Error = null };
        //    }
        //    else
        //    {
        //        // Handle error
        //        var errorContent = response.Content;
        //        return new TokenResponse { Error = new Error { IsError = true, Message = errorContent } };
        //    }

        //}

        //  {
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.UTF8.GetBytes(clientSecret);
        //    var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Issuer = clientId,
        //        Audience = clientId,
        //        Expires = DateTime.UtcNow.AddMinutes(60),
        //        SigningCredentials = credentials,
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //    new Claim(JwtRegisteredClaimNames.Iss, clientId),
        //    new Claim(JwtRegisteredClaimNames.Aud, clientId)
        //}.Union(scopes.Select(scope => new Claim("scope", scope))))
        //    };

        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var accessToken = tokenHandler.WriteToken(token);

        //    return new TokenResponse { AccessToken = accessToken, Error = null };

        //  }



    }
}
