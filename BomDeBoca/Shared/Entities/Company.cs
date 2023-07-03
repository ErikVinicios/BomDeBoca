using System.ComponentModel.DataAnnotations;

namespace BomDeBoca.Shared.Entities
{
    public class Company
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string? Description { get; set; }
        public byte[]? Logo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<Product>? Products { get; set; }
        public ICollection<CompanyFeedback>? Feedbacks { get; set; }
    }
}
