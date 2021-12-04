using Newtonsoft.Json;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class BaseRepo<T> : IBaseRepo<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public BaseRepo(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> Create(string url, T entity, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (entity != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            }
            else
            {
                return null;
            }

            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }


            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Delete(string url, int Id, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            return false;
        }

        public async Task<T> Get(string url, string Id, string token = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var client = _clientFactory.CreateClient();
            if (token != null && token.Length != 0)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonString);
            }

            return null;
        }

        public Task<ICollection<T>> GetList(string url, string token = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsExist(string url,string name)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(string url, T entity, string token)
        {
            throw new NotImplementedException();
        }
    }
}
