using Newtonsoft.Json;
using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class UserRepo : BaseRepo<UserDto>, IUserRepo
    {
        public UserRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }        
    }
}
