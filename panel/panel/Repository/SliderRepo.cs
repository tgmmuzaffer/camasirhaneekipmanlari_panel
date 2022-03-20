using panel.Models;
using panel.Models.Dtos;
using panel.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace panel.Repository
{
    public class SliderRepo : BaseRepo<Slider>, ISliderRepo
    {
        public SliderRepo(IHttpClientFactory clientFactory) : base(clientFactory)
        {
        }
    }
}
