using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BomDeBoca.Client.services
{
    public class CompanyFeedbackService : ICompanyFeedbackService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CompanyFeedbackService(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<APIResult> Delete(Guid id)
        {
            var httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7254/CompanyFeedback/delete?id={id}");
                var delete = await httpClient.SendAsync( request );
                result = await delete.Content.ReadFromJsonAsync<APIResult>();
                return result;
            }
            catch (Exception e)
            {
                result.Result = false;
                result.Message = e.Message;
                return result;
            }
        }

        public async Task<APIResult> Get(Guid id)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7254/CompanyFeedback/get?id={id}");
                var get = await _httpClient.SendAsync(request);
                result.Obj = await get.Content.ReadFromJsonAsync<CompanyFeedback>();
                result.Result = true;
                return result;
            }
            catch (Exception e)
            {
                result.Result = false;
                result.Message= e.Message;
                return result;
            }
        }

        public async Task<APIResult> GetAll()
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7254/CompanyFeedback/getall");
                var getall = await _httpClient.SendAsync(request);
                result.Obj =  await getall.Content.ReadFromJsonAsync<List<CompanyFeedback>>();
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

        public async Task<APIResult> GetAllByIdCompany(Guid id)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7254/CompanyFeedback/getallbyidcompany?ID={id}");
                var getall = await _httpClient.SendAsync(request);
                result.Obj = await getall.Content.ReadFromJsonAsync<List<CompanyFeedback>>();
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

        public async Task<APIResult> Save(CompanyFeedback feedback)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7254/CompanyFeedback/save");
                request.Content = new StringContent(JsonSerializer.Serialize(feedback), Encoding.UTF8,"application/json");
                var save = await _httpClient.SendAsync(request);
                result.Obj = await save.Content.ReadFromJsonAsync<CompanyFeedback>();
                result.Result = true;
                return result;
            }
            catch (Exception e)
            {
                result.Result= false;
                result.Message = e.Message;
                return result;
            }
        }

        public async Task<APIResult> Update(CompanyFeedback feedback)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7254/CompanyFeedback/update");
                request.Content = new StringContent(JsonSerializer.Serialize(feedback), Encoding.UTF8, "application/json");
                var update = await _httpClient.SendAsync(request);
                result.Obj = await update.Content.ReadFromJsonAsync<CompanyFeedback>();
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
