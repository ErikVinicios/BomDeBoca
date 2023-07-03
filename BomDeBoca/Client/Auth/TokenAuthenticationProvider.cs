using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Client.Utils;
using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Results;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace BomDeBoca.Client.Auth
{
    public class TokenAuthenticationProvider : AuthenticationStateProvider, IAuthorizeService
    {
        private readonly IJSRuntime _js;
        private readonly HttpClient _httpClient;

        public static readonly string tokenKey = "tokenKey";
        public TokenAuthenticationProvider(IJSRuntime js, HttpClient httpClient)
        {
            _js = js;
            _httpClient = httpClient;
        }

        private AuthenticationState notAuthenticate =>
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.GetFromLocalStorage(tokenKey);

            if (string.IsNullOrEmpty(token)) {
                return notAuthenticate;
            }
            return CreateAuthenticationState(token);
        }

        public AuthenticationState CreateAuthenticationState(string token)
        {
            // colocar o token obtido no localstorage no header do request na sessão Authorization assim podemos estar autenticando cada requisição HTTP enviada ao servidor por este cliente
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            // extrair as claims
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(parseClaimsFromJwt(token), "jwt")));
        }

        private IEnumerable<Claim> parseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = parseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            return claims;
        }

        private byte[] parseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        public async Task Login(LocalStorageDTO localStorageDTO)
        {
            await _js.SetInLocalStorage(tokenKey, localStorageDTO.Token.Code);
            var authState = CreateAuthenticationState(localStorageDTO.Token.Code);
            NotifyAuthenticationStateChanged(Task.FromResult(authState));
        }

        public async Task Logout()
        {
            await _js.RemoveItem(tokenKey);
            _httpClient.DefaultRequestHeaders.Authorization = null;
            //NotifyAuthenticationStateChanged(Task.FromResult(notAuthenticate));
        }
    }
}
