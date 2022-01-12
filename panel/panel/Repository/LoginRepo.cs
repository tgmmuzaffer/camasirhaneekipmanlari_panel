using Newtonsoft.Json;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class LoginRepo : BaseRepo<User>, ILoginRepo
    {
        private readonly IHttpClientFactory _clientFactory;

        public LoginRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<UserDto> Login(string url, UserDto userDto)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (userDto != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
            }
            else
            {
                return new UserDto();
            }

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString =  response.Content.ReadAsStringAsync().Result.ToString();
                var userdata = JsonConvert.DeserializeObject<UserDto>(jsonString);
                return userdata;
            }
            else
            {
                return new UserDto();
            }
        }


        public async Task<bool> Register(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content = new StringContent(
                    JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
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
    }
}
