using panel.Models;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class ContactRepo :BaseRepo<Contact>, IContactRepo
    {
        public ContactRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
