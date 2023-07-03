using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomDeBoca.Shared.Entities
{
    public class Token
    {
        public string Code { get; set; }
        public DateTime Expiration { get; set; }
        public string Message { get; set; }
    }
}
