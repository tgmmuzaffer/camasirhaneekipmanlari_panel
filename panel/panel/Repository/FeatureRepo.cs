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
    public class FeatureRepo : BaseRepo<Feature>, IFeatureRepo
    {
        private readonly IHttpClientFactory httpClient;
        public FeatureRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            httpClient = clientFactory;
        }

        public async Task<ICollection<Feature>> UpdateCreateFeature(string url, Feature feature, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                if (feature != null && !string.IsNullOrEmpty(feature.Name))
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(feature), Encoding.UTF8, "application/json");
                }
                else
                {
                    return null;
                }
                var client = httpClient.CreateClient();
                if (token != null && token.Length != 0)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }


                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ICollection<Feature>>(jsonString);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
    }
}
