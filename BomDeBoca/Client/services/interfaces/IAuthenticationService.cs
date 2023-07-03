using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;

namespace BomDeBoca.Client.services.interfaces
{
    public interface IAuthenticationService
    {
        Task<LocalStorageDTO> Login(LoginDTO user);
        Task<APIResult> Logout();
    }
}
