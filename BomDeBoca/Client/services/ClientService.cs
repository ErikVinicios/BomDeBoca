using BomDeBoca.Client.services.interfaces;
using BomDeBoca.Shared.Entities;
using System.Text.Json;
using System.Text;
using System.Net.Http.Json;
using BomDeBoca.Shared.Results;
using BomDeBoca.Shared.dto;

namespace BomDeBoca.Client.services {
    public class ClientService : IClientService {
        private IHttpClientFactory _httpClientFactory;

        public ClientService(IHttpClientFactory httpClientFactory) {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<APIResult> Delete(Guid id) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try {
                var request = new HttpRequestMessage(HttpMethod.Delete, $"https://localhost:7254/User/delete?id={id}");
                //request.Content = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
                var delete = await _httpClient.SendAsync(request);
                result.Obj = id;
                result.Result = true;
                return result;
            } catch (Exception e){
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }

        public async Task<BomDeBoca.Shared.Entities.Client> Get(Guid id) {
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:7254/User/get?id={id}");
            //request.Content = new StringContent(JsonSerializer.Serialize(id), Encoding.UTF8, "application/json");
            var getClient = await _httpClient.SendAsync(request);
            return await getClient.Content.ReadFromJsonAsync<BomDeBoca.Shared.Entities.Client>();
        }

        public async Task<List<BomDeBoca.Shared.Entities.Client>> GetAll() {
            var _httpClient = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7254/User/getall");
            var getAllClient = await _httpClient.SendAsync(request);
            return await getAllClient.Content.ReadFromJsonAsync<List<BomDeBoca.Shared.Entities.Client>>();
        }

        public async Task<APIResult> Save(RegisterDTO user) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7254/User/save");
                request.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
                HttpResponseMessage save = await _httpClient.SendAsync(request);
                result = await save.Content.ReadFromJsonAsync<APIResult>();
                return result;
            }
            catch (Exception e) {
                result.Obj = null;
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }

        public async Task<APIResult> Update(BomDeBoca.Shared.Entities.Client user) {
            var _httpClient = _httpClientFactory.CreateClient();
            APIResult result = new APIResult();
            try {
                var request = new HttpRequestMessage(HttpMethod.Put, "https://localhost:7254/User/update");
                request.Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
                var update = await _httpClient.SendAsync(request);
                result.Obj = await update.Content.ReadFromJsonAsync<BomDeBoca.Shared.Entities.Client>();
                result.Message = string.Empty;
                result.Result = true;
                return result;
            }
            catch (Exception e) {
                result.Obj = null;
                result.Message = e.Message;
                result.Result = false;
                return result;
            }
        }
    }
}
