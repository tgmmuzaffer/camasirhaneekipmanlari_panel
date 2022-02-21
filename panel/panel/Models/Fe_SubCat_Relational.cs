using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class Fe_SubCat_Relational :IEntity
    {
        public int Id { get; set; }
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
