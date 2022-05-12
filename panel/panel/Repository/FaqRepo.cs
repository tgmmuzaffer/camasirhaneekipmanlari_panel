using panel.Models;
using panel.Repository.IRepository;
using System.Net.Http;

namespace panel.Repository
{
    public class FaqRepo :BaseRepo<Faq>, IFaqRepo
    {
        public FaqRepo(IHttpClientFactory clientFactory):base(clientFactory)
        {

        }
    }
}
