using System.ComponentModel.DataAnnotations;

namespace BomDeBoca.Shared.Entities
{
    public class Product
    {
        [Key]
        public Guid ID { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public float Price { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreationDate { get; set; }

        public Guid CompanyID { get; set; }

        public virtual Company? Company { get; set; }
    }
}
