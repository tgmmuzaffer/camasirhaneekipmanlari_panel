using panel.Models;
using panel.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IProductRepo : IBaseRepo<Product>
    {
        //Task<bool> DeleteProduct(string url, Product entity, string token);
    }
}
