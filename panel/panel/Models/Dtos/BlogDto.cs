﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace panel.Models.Dtos
{
    public class BlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDesc { get; set; }
        public string Content { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public DateTime CreateDate { get; set; }
        public List<int> TagIds { get; set; } = new List<int>();
        public List<string> TagNames { get; set; } = new List<string>();
    }
}
