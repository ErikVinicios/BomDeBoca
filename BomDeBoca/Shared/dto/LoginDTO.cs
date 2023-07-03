using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomDeBoca.Shared.dto {
    public class LoginDTO {
        public string CpfCnpj { get; set; }
        public string Password { get; set; }
        public UserType UserType { get; set; }
    }
}
