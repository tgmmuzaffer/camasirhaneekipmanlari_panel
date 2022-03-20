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
    public class Cat_Fe_RelRepo : BaseRepo<Cat_Fe_Relational>, ICat_Fe_RelRepo
    {
        private readonly IHttpClientFactory httpClient;
        public Cat_Fe_RelRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            httpClient = clientFactory;
        }

        public async Task<bool> UpdateCreateFeatureCatLinks(string url, List<Cat_Fe_Relational> fe_Cat_Relationals, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                if (fe_Cat_Relationals != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(fe_Cat_Relationals), Encoding.UTF8, "application/json");
                }
                else
                {
                    return false;
                }
                var client = httpClient.CreateClient();
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
