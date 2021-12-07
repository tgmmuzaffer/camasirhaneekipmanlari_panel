using panel.Repository.IRepository;
using System.Net.Http;

namespace panel.Repository
{
    public class PropertyTabRepo : BaseRepo<PropertyTabRepo>, IPropertyTabRepo
    {
        public PropertyTabRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
