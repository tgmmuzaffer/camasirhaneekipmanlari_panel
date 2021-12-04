namespace panel.Models
{
    public class User : IEntity
    {
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
