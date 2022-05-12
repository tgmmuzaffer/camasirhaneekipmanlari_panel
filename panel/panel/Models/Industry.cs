using Microsoft.AspNetCore.Http;

namespace panel.Models
{
    public class Industry : IEntity
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public string ImageData { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
