using System.ComponentModel.DataAnnotations;

namespace BomDeBoca.Shared.Entities
{
    public class Client
    {
        [Key]
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public byte[]? Photo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }

        public ICollection<CompanyFeedback> Feedbacks { get; set; }
    }
}
