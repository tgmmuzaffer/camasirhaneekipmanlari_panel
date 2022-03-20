using Newtonsoft.Json;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class RoleRepo : IRoleRepo
    {
        private readonly IHttpClientFactory _clientFactory;

        public RoleRepo(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<ICollection<Role>> GetRoles(string url, string token = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var client = _clientFactory.CreateClient();
                if (token != null && token.Length != 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<Role>>(jsonString);
                }

                return null;
            }
            catch (Exception e)
            {
                return null;

            }

        }
    }
}
