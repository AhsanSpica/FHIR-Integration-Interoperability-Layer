using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface.Models.Auth
{

    public class LoginRequest
    {
     //   public string ClientId { get; set; }
      //  public string ClientSecret { get; set; }
        public List<string> Scopes { get; set; }
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public Error Error { get; set; }
        public string ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string error_description { get; set; }
        public int status_code { get; set; }
        public bool IsSuccessStatusCode { get; set; }
    }
    public class ErrorResponse
    {
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }
    public class Error
    {
        public bool IsError { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Permissions { get; set; }
    }
}
