using ICriterionApiService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using GlobalHelpers;
using Newtonsoft.Json;
using System.Net.Http;
using Interface.Models.Auth;
using Hl7.Fhir.Model;
using RestSharp;
using static System.Formats.Asn1.AsnWriter;
using Interface.Models.Criterion;
using Interface.Misc.Helpers;
using HtmlAgilityPack;

namespace CriterionApiService
{
    public class CriterionApiService : ICriterionApiService.ICriterionApiService
    {
       // private readonly RestClient _httpClient;
     
        private readonly IOptions<CriterionApiSettings> _criterionSettings;

        public CriterionApiService(
            //RestClient httpClient,
           
            IOptions<CriterionApiSettings> criterionSettings)
        {
        
            _criterionSettings = criterionSettings;

            //var proxy = new WebProxy($"{_criterionSettings.ProxyUrl}:{_criterionSettings.ProxyPort}")
            //{
            //    BypassProxyOnLocal = false,
            //    UseDefaultCredentials = false,
            //};

            //var httpClientHandler = new HttpClientHandler
            //{
            //    Proxy = proxy,
            //    UseProxy = true
            //};

            // _httpClient = new HttpClient(httpClientHandler);
           
        }

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public async Task<CredentialResponse> RegisterClientAsync()
        {
            using var handler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true };
            using var httpClient = new HttpClient(handler);
            var _restClient = new RestClient();
            var request = new RestRequest($"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRAUTHPATH}/register", Method.Post);
            request.AddHeader("Content-Type", "application/json");

            var body = new
            {
                application_type = "web",
                redirect_uris = new[] { _criterionSettings.Value.RedirectUri },
                client_name = "DataQ Health",
                logo_uri = "https://th.bing.com/th/id/OIP.rpQeDCc2zDecOw8HD-pcdwHaHa?w=1025&h=1025&rs=1&pid=ImgDetMain",
                subject_type = "pairwise",
                token_endpoint_auth_method = "client_secret_basic",
                userinfo_encrypted_response_alg = "RSA-OAEP-256",
                userinfo_encrypted_response_enc = "A128CBC-HS256",
                contacts = new[] { "mailto:Ahsan.Siddiqui@dataqhealth.com", "Muhammad.Ashraf@wisemani.com" }
            };
            request.AddJsonBody(body);

            var response = await _restClient.ExecuteAsync<CredentialResponse>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"Error registering client: {response.ErrorMessage}");
            }
            return response.Data;
        }

        public async Task<string> GetAuthorizationUrlAsync(CredentialResponse credentials)
        {
            credentials = new CredentialResponse();

           //  var authRequest = new RestRequest($"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRAUTHPATH}/authz", Method.Post);
           // authRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

            //take client id and client secret from appsettings
            credentials.client_id = _criterionSettings.Value.ClientId;
            credentials.client_secret = _criterionSettings.Value.ClientSecret;


            // Basic authentication header
            var authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{credentials.client_id}:{credentials.client_secret}"));
            // authRequest.AddParameter("Authorization", $"Basic {authHeader}", ParameterType.HttpHeader);

            var handler = new HttpClientHandler { UseCookies = true };
            var client = new HttpClient(handler);
          //  var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRAUTHPATH}/authz");
            request.Headers.Add("Accept", "text/html");
            request.Headers.Add("Authorization", $"Basic {authHeader}");
           // request.Headers.Add("Cookie", "RSESSID=sXxWIS17piPDozEmG-zzA7L1WIp8qmOo");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("client_id", _criterionSettings.Value.ClientId));
            collection.Add(new("response_type", "code"));
            collection.Add(new("redirect_uri", _criterionSettings.Value.RedirectUri));
            collection.Add(new("scope", _criterionSettings.Value.Scopes));
            collection.Add(new("state", "true"));
            collection.Add(new("contacts", "[\"Muhammad.Ashraf@wisemani.com\",\"mailto:Ahsan.Siddiqui@dataqhealth.com\"]"));

            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);

            var asyncContent = await response.Content.ReadAsStringAsync();

           var success = await sendLoginRequest(asyncContent, handler);
            
                return response.RequestMessage.RequestUri.ToString();
            
        }

        private async Task<bool> sendLoginRequest(string loginPageHtml, HttpClientHandler handler)
        {
            var loginPageDoc = new HtmlDocument();
            loginPageDoc.LoadHtml(loginPageHtml);
            string username = "Ahsan4546";
            string password = "582alien";

            var form = loginPageDoc.DocumentNode.SelectSingleNode("//form");
            var actionUrl = form.GetAttributeValue("action", string.Empty);

                var baseUri = new Uri($"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRAUTHPATH}");


            var loginUrl = new Uri(baseUri, actionUrl).ToString();

            var inputs = form.SelectNodes("//input");
            var formData = new Dictionary<string, string>();

            foreach (var input in inputs)
            {
                var name = input.GetAttributeValue("name", string.Empty);
                if (!string.IsNullOrEmpty(name))
                {
                    formData[name] = input.GetAttributeValue("value", string.Empty);
                }
            }

            formData["username"] = username;
            formData["password"] = password;

            var loginContent = new FormUrlEncodedContent(formData);

            var loginRequest = new HttpRequestMessage(HttpMethod.Post, loginUrl)
            {
                Content = loginContent
            };

            var client = new HttpClient(handler);
            HttpResponseMessage loginResponse = new HttpResponseMessage();
           
            try
            {
                loginResponse = await client.SendAsync(loginRequest); 
            }
                                                                     
            catch (Exception ex)
            {
                HelperMethods.CreateConsoleLog($"Exception during login reuqest to /login : {ex.Message}");
                return false;
            }

            return loginResponse.IsSuccessStatusCode;
        }



        //public async Task<bool> sendLoginRequest(string htmlContent, HttpClientHandler handler)
        //{
        //    var _httpClient = new HttpClient(handler);
        //    var doc = new HtmlDocument();
        //    doc.LoadHtml(htmlContent);
        //    string username = "Ahsan4546";
        //    string password = "582alien";
        //    var baseUri = new Uri($"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRAUTHPATH}");

        //    try
        //    {
        //        var form = doc.DocumentNode.SelectSingleNode("//form[@id='form']");
        //        var usernameInput = form.SelectSingleNode("//input[@id='username']");
        //        var passwordInput = form.SelectSingleNode("//input[@id='password']");

        //        if (usernameInput != null && passwordInput != null)
        //        {
        //            usernameInput.SetAttributeValue("value", username);
        //            passwordInput.SetAttributeValue("value", password);

        //            // Simulate form submission
        //            var submitButton = form.SelectSingleNode("//button[@id='button2']");
        //            if (submitButton != null)
        //            {
        //                var formData = new Dictionary<string, string>
        //            {
        //                { "username", username },
        //                { "password", password },
        //                { "cancel", "" } // Simulate "Authorize" action
        //            };

        //                var formAction = form.GetAttributeValue("action", string.Empty);
        //                var actionUri = new Uri(baseUri, formAction);

        //                var postContent = new FormUrlEncodedContent(formData);
        //                var postResponse = _httpClient.PostAsync(actionUri, postContent).GetAwaiter().GetResult();

        //                if (postResponse.IsSuccessStatusCode)
        //                {
        //                    var result = await postResponse.Content.ReadAsStringAsync();
        //                    HelperMethods.CreateConsoleLog($"Authorization successful: {result}");
        //                    return true;
        //                }
        //                else
        //                {
        //                    HelperMethods.CreateConsoleLog($"Failed to authorize: {postResponse.StatusCode}");
        //                    return false;
        //                }
        //            }
        //        }

        //        HelperMethods.CreateConsoleLog("Failed to locate form elements.");
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperMethods.CreateConsoleLog($"Exception during authorization: {ex.Message}");
        //        return false;
        //    }
        //}



        public async Task<TokenResponse> GenerateTokenAsync()
        { 
            var _httpClient = new RestClient();
            var request = new RestRequest($"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRAUTHPATH}/token", Method.Post);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            string base64Auth = Base64Encode($"{_criterionSettings.Value.ClientId}:{_criterionSettings.Value.ClientSecret}");
            request.AddHeader("Authorization", $"Basic {base64Auth}");
            request.AddParameter("grant_type", _criterionSettings.Value.GrantType); 

            var response = await _httpClient.ExecuteAsync(request);
            var responseContent = response.Content;
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);


            //var responseContent = await response.Content.ReadAsStringAsync();
            //var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            return tokenResponse;
        }

        public async Task<Resource> GetResourceAsync(string resourceType, int resourceId)
        {
            var request = new RestRequest($"{_criterionSettings.Value.CRITERIONDOMAINURL}/{_criterionSettings.Value.FHIRPATH}/{ resourceType }/{ resourceId}", Method.Get);
            var _httpClient = new RestClient();

            //  var request = new HttpRequestMessage(HttpMethod.Get, $"{_criterionSettings.Value.FHIRURL}/{resourceType}/{resourceId}");

            var token = await GenerateTokenAsync();
            Resource resource = null;

            if (token.IsSuccessStatusCode)

            {
                request.AddHeader("Authorization",$"Bearer {token.AccessToken}");
              //  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

                var response = await _httpClient.ExecuteAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Resource request failed with status code {response.StatusCode}");
                }

                var responseString =  response.Content;
                resource = JsonConvert.DeserializeObject<Resource>(responseString);
            }
            return resource;
        }

        //private class TokenResponse
        //{
        //    public string AccessToken { get; set; }
        //    public string TokenType { get; set; }
        //    public int ExpiresIn { get; set; }
        //    public string error_description { get; set; }
        //    public string error { get; set; }
        //}
    }
}
