using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface ILogRepo 
    {
        Task<bool> Delete(string url, string token);
        Task<ICollection<Log>> GetList(string url,int count,  string token);
    }
}
