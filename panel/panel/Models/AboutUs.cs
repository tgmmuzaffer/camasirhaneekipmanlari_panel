using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models
{
    public class AboutUs
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }
        public string Content3 { get; set; }
        public string ImagePath1 { get; set; } 
        public string ImageName1 { get; set; }
        public IFormFile ImageFile1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImageName2 { get; set; }
        public IFormFile ImageFile2 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImageName3 { get; set; }
        public IFormFile ImageFile3 { get; set; }

    }
}
