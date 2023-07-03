using BomDeBoca.Client.Auth;
using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BomDeBoca.Client.services
{
    public class AuthenticationService : IAuthenticationService
    {
        private IHttpClientFactory _httpClientFacory;
        private TokenAuthenticationProvider _tokenAuthProvider;

        public AuthenticationService(IHttpClientFactory httpClient, TokenAuthenticationProvider tokenAuthProvider)
        {
            _httpClientFacory = httpClient;
            _tokenAuthProvider = tokenAuthProvider;
        }

        public async Task<LocalStorageDTO> Login(LoginDTO user)
        {
            var _httpClient = _httpClientFacory.CreateClient();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7254/Authentication/login");
                request.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
                HttpResponseMessage login = await _httpClient.SendAsync(request);
                if (login.IsSuccessStatusCode)
                {
                    var responseAsString = await login.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LocalStorageDTO>(responseAsString,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    await _tokenAuthProvider.Login(loginResult);
                    return loginResult;
                }
                return new LocalStorageDTO() {
                    Token = null,
                    User = null
                };
            }
            catch
            {
                return new LocalStorageDTO()
                {
                    Token = null,
                    User = null
                };
            }
        }

        public async Task<APIResult> Logout()
        {
            APIResult result = new APIResult();
            try
            {
                await _tokenAuthProvider.Logout();
                result.Result = true;
                return result;
            }
            catch (Exception e)
            {
                result.Result = false;
                result.Message = e.Message;
                return result;
            }
        }
    }
}
