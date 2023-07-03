using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;

namespace BomDeBoca.Client.services.interfaces
{
    public interface ICompanyFeedbackService
    {
        Task<APIResult> GetAll();
        Task<APIResult> GetAllByIdCompany(Guid id);
        Task<APIResult> Get(Guid id);
        Task<APIResult> Update(CompanyFeedback feedback);
        Task<APIResult> Delete(Guid id);
        Task<APIResult> Save(CompanyFeedback feedback);
    }
}
