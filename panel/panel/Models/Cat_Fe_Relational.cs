using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class Cat_Fe_Relational : IEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int FeatureId { get; set; }
        public Feature Feature { get; set; }
    }
}
