using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public SubCategory SubCategory{ get; set; }
        public int SubCategoryId { get; set; }
        public List<Feature> Feature { get; set; }
        public string FeatureIds { get; set; }
        public List<FeatureDescription> FeatureDescriptions { get; set; }
        public string FeatureDescriptionIds { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public bool IsPublish { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
