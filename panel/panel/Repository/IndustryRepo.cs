using panel.Models;
using panel.Repository.IRepository;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class IndustryRepo : BaseRepo<Industry>, IIndustryRepo
    {
        public IndustryRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
