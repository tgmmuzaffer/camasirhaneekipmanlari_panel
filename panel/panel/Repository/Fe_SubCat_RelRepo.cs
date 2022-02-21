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
    public class Fe_SubCat_RelRepo : BaseRepo<Fe_SubCat_Relational>, IFe_SubCat_RelRepo
    {
        private readonly IHttpClientFactory httpClient;
        public Fe_SubCat_RelRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            httpClient = clientFactory;
        }

        public async Task<bool> UpdateCreateFeatureSubCatLinks(string url,List<Fe_SubCat_Relational> fe_SubCat_Relationals, string token)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                if (fe_SubCat_Relationals != null )
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(fe_SubCat_Relationals), Encoding.UTF8, "application/json");
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

                throw new Exception(e.Message);
            }
        }
    }
}
