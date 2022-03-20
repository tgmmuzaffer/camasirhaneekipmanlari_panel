using Newtonsoft.Json;
using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class Pr_Fe_RelRepo : BaseRepo<Pr_Fe_Relational>, IPr_Fe_RelRepo
    {
        private readonly IHttpClientFactory _clientFactory;

        public Pr_Fe_RelRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> CreateMultiple(string url, List<Pr_Fe_Relational> entity, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                if (entity != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                }
                else
                {
                    return false;
                }
                var client = _clientFactory.CreateClient();
                if (token != null && token.Length != 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;

            }
        }
    }
}
