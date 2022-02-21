using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IFe_SubCat_RelRepo : IBaseRepo<Fe_SubCat_Relational>
    {
        Task<bool> UpdateCreateFeatureSubCatLinks(string url, List<Fe_SubCat_Relational> fe_SubCat_Relationals, string token);
    }
}
