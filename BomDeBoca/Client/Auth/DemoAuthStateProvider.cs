using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BomDeBoca.Client.Auth
{
    public class DemoAuthStateProvider : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "User"),
            });
            return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(user)));
        }
    }
}
