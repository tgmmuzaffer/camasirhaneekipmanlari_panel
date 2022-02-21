using Newtonsoft.Json;
using panel.Models;
using panel.Models.Dtos;
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
    public class ProductRepo : BaseRepo<Product>, IProductRepo
    {
        //private readonly IHttpClientFactory httpClient;
        public ProductRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }

        //public async Task<bool> DeleteProduct(string url, Product entity, string token)
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Post, url);
        //        if (entity != null && !string.IsNullOrEmpty(entity.Name))
        //        {
        //            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //        var client = httpClient.CreateClient();
        //        if (token != null && token.Length != 0)
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        }


        //        HttpResponseMessage response = await client.SendAsync(request);
        //        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //        throw new Exception(e.Message);
        //    }
        //}

       
    }
}
