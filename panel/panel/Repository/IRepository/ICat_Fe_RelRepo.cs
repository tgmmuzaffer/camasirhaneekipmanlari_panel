using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface ICat_Fe_RelRepo :IBaseRepo<Cat_Fe_Relational>
    {
        Task<bool> UpdateCreateFeatureCatLinks(string url, List<Cat_Fe_Relational> fe_Cat_Relationals, string token);
    }
}
