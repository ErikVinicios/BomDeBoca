using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;

namespace BomDeBoca.Client.services.interfaces {
    public interface IProductService {
        Task<List<Product>> GetAll();
        Task<List<Product>> GetAllByCompanyID(Guid ID);
        Task<APIResult> Get(Guid id);
        Task<APIResult> Save(Product product);
        Task<APIResult> Update(Product product);
        Task<APIResult> Delete(Guid id);
    }
}
