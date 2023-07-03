using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomDeBoca.Shared.Entities {
    public class CompanyFeedback {
        [Key]
        public Guid ID { get; set; }
        public Guid ClientID { get; set; }
        public Guid CompanyID { get; set; }

        public string ClientName { get; set; }
        public byte[]? ClientPhoto { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public RatingType Rating { get; set; }


        public virtual Client? Client { get; set; }
        public virtual Company? Company { get; set; }
    }
}
