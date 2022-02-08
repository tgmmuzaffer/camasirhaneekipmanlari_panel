using panel.Models;
using panel.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IProductRepo : IBaseRepo<ProductDto>
    {
    }
}
