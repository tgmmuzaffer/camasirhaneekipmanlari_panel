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
        public string Name { get; set; }
        public string ShortDesc { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public IFormFile Imagefile { get; set; }
        public bool IsPublish { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
