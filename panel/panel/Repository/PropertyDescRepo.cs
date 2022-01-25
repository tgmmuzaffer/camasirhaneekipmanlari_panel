using panel.Models;
using panel.Repository.IRepository;
using System.Net.Http;

namespace panel.Repository
{
    public class PropertyDescRepo : BaseRepo<PropertyDescription>, IPropertyDescRepo
    {
        public PropertyDescRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
