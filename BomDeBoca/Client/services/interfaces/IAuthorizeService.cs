using BomDeBoca.Shared.dto;

namespace BomDeBoca.Client.services.interfaces
{
    public interface IAuthorizeService
    {
        Task Login(LocalStorageDTO localStorageDTO);
        Task Logout();
    }
}
