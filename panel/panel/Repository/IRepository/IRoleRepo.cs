using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IRoleRepo
    {
        Task<ICollection<Role>> GetRoles(string url, string token = null);
    }
}
