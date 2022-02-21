using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Feature> Features { get; set; }
        public ICollection<Fe_SubCat_Relational> Fe_SubCat_Relationals { get; set; }
    }
}
