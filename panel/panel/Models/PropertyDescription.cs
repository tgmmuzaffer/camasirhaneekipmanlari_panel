namespace panel.Models
{
    public class PropertyDescription : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductPropertyId { get; set; }

    }
}
