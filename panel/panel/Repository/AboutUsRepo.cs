using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class AboutUsRepo : BaseRepo<AboutUs>, IAboutUsRepo
    {
        public AboutUsRepo(IHttpClientFactory clientFactory): base(clientFactory)
        {

        }
    }
}
