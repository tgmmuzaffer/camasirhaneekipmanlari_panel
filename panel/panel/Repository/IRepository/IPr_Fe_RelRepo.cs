using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IPr_Fe_RelRepo : IBaseRepo<Pr_Fe_Relational>
    {
        Task<bool> CreateMultiple(string url, List<Pr_Fe_Relational> entity, string token);
    }
}
