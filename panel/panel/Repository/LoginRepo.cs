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
            try
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
                    var jsonString = response.Content.ReadAsStringAsync().Result.ToString();
                    var userdata = JsonConvert.DeserializeObject<UserDto>(jsonString);
                    return userdata;
                }
                else
                {
                    return new UserDto();
                }
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
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


        public async Task<UserDto> GetUserDataByName(string url, string name)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url + name);

                var client = _clientFactory.CreateClient();
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    if (string.IsNullOrEmpty(jsonString))
                    {
                        return new UserDto();
                    }
                    var userdata = JsonConvert.DeserializeObject<UserDto>(jsonString);
                    return userdata;
                }

                return new UserDto();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<bool> GetByResetPass(string url, User user)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                if (user != null)
                {
                    request.Content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                }
                else
                {
                    return false;
                }
                var client = _clientFactory.CreateClient();

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
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
