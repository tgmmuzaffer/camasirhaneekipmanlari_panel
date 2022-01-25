using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IBaseRepo<T> where T : class
    {
        Task<ICollection<T>> GetList(string url, string token=null);
        Task<T> Get(string url, string token=null);
        Task<int> Create(string url, T entity, string token);
        Task<bool> Update(string url, T entity, string token);
        Task<bool> Delete(string url, string token);
        Task<bool> IsExist(string url, string name, string token = null);
    }
}
