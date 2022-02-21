using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class Feature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SubCategoryId { get; set; }
        public string FeatureIds { get; set; }
        public bool IsChoosen { get; set; }
        public ICollection<SubCategory> SubCategories { get; set; }
        public ICollection<FeatureDescription> FeatureDescriptions { get; set; }
        public ICollection<Fe_SubCat_Relational> Fe_SubCat_Relationals { get; set; }

    }
}
