using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;

namespace BomDeBoca.Client.services.interfaces {
    public interface IClientService {
        Task<List<BomDeBoca.Shared.Entities.Client>> GetAll();
        Task<BomDeBoca.Shared.Entities.Client> Get(Guid id);
        Task<APIResult> Save(RegisterDTO user);
        Task<APIResult> Update(BomDeBoca.Shared.Entities.Client user);
        Task<APIResult> Delete(Guid id);
    }
}
