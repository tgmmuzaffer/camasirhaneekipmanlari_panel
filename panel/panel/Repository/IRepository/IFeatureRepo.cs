using panel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Repository.IRepository
{
    public interface IFeatureRepo : IBaseRepo<Feature>
    {
        Task<ICollection<Feature>> UpdateCreateFeature(string url, Feature feature, string token);
    }
}
