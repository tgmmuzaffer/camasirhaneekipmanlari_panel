using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class ProductPropertyRepo : BaseRepo<ProductProperty>, IProductPropertyRepo
    {
        public ProductPropertyRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
