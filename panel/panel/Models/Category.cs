using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace panel.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public IFormFile ImageFile { get; set; }
        public List<Product> Products { get; set; }
        public List<SubCategory> SubCategories { get; set; }
    }
}