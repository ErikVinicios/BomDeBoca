using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Shared.Results;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using BomDeBoca.Shared.dto;
using BomDeBoca.Shared.Entities;

namespace BomDeBoca.Company.services
{
    public class CompanyService : ICompanyService
    {
        private IHttpClientFactory _httpClientFactory;

        public CompanyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<APIResult> Delete(Guid id)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7254/Company/delete?id={id}");
                //request.Content = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                var delete = await _httpClient.SendAsync(request);
                result.Obj = id;
                result.Result = true;
                return result;
            }
            catch (Exception e)
            {
                result.Obj = null;
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }

        public async Task<BomDeBoca.Shared.Entities.Company> Get(Guid id)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7254/Company/get?id={id}");
            //request.Content = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
            var get = await _httpClient.SendAsync(request);
            return await get.Content.ReadFromJsonAsync<BomDeBoca.Shared.Entities.Company>();
        }

        public async Task<List<BomDeBoca.Shared.Entities.Company>> GetAll()
        {
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7254/Company/getall");
            var getAll = await _httpClient.SendAsync(request);
            return await getAll.Content.ReadFromJsonAsync<List<BomDeBoca.Shared.Entities.Company>>();
        }

        public async Task<APIResult> Save(RegisterDTO company)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7254/Company/save");
                request.Content = new StringContent(JsonSerializer.Serialize(company), Encoding.UTF8, "application/json");
                HttpResponseMessage save = await _httpClient.SendAsync(request);
                result = await save.Content.ReadFromJsonAsync<APIResult>();
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }

        public async Task<APIResult> Update(BomDeBoca.Shared.Entities.Company company)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7254/Company/update");
                request.Content = new StringContent(JsonSerializer.Serialize(company), Encoding.UTF8, "application/json");
                var update = await _httpClient.SendAsync(request);
                result.Obj = await update.Content.ReadFromJsonAsync<BomDeBoca.Shared.Entities.Company>();
                result.Result = true;
                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }
    }
}
