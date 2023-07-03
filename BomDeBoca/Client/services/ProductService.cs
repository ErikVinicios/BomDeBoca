using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Shared.Entities;
using BomDeBoca.Shared.Results;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BomDeBoca.Client.services {
    public class ProductService : IProductService {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClient) {
            _httpClientFactory = httpClient;
        }

        public async Task<List<Product>> GetAll() {
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7254/Product/getall");
            var getall = await _httpClient.SendAsync(request);
            return await getall.Content.ReadFromJsonAsync<List<Product>>();
        }

        public async Task<List<Product>> GetAllByCompanyID(Guid companyID)
        {
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7254/Product/getallbycompanyid?id={companyID}");
            var getall = await _httpClient.SendAsync(request);
            return await getall.Content.ReadFromJsonAsync<List<Product>>();
        }

        public async Task<APIResult> Get(Guid id) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7254/Product/get?id={id}");
                var get = await _httpClient.SendAsync(request);
                result.Obj = await get.Content.ReadFromJsonAsync<Product>();
                result.Message = string.Empty;
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

        public async Task<APIResult> Save(Product product) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7254/Product/save");
                request.Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
                HttpResponseMessage save = await _httpClient.SendAsync(request);
                result.Obj = await save.Content.ReadFromJsonAsync<Product>();
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

        public async Task<APIResult> Delete(Guid id) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7254/Product/delete?id={id}");
                var delete = await _httpClient.SendAsync(request);
                result.Obj = id;
                result.Message = string.Empty;
                result.Result = true;
                return result;
            } catch (Exception e)
            {
                result.Obj = null;
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }

        public async Task<APIResult> Update(Product product) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Put, $"https://localhost:7254/Product/update");
                request.Content = new StringContent(JsonSerializer.Serialize(product), Encoding.UTF8, "application/json");
                var update = await _httpClient.SendAsync(request);
                result.Obj = await update.Content.ReadFromJsonAsync<Product>();
                result.Message = string.Empty;
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
    }
}
