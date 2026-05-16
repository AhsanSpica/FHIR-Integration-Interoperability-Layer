using Interface.Models.Auth;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITokenService
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateToken(List<string> scopes);
    }

}
