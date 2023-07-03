using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Results;

namespace BomDeBoca.Client.services.interfaces
{
    public interface ICompanyService
    {
        Task<List<BomDeBoca.Shared.Entities.Company>> GetAll();
        Task<BomDeBoca.Shared.Entities.Company> Get(Guid id);
        Task<APIResult> Save(RegisterDTO company);
        Task<APIResult> Update(BomDeBoca.Shared.Entities.Company company);
        Task<APIResult> Delete(Guid id);
    }
}
