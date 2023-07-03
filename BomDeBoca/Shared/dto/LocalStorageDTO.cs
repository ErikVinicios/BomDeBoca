using BomDeBoca.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomDeBoca.Shared.dto {
    public class LocalStorageDTO {
        public Token? Token { get; set; }
        public object User { get; set; }
    }
}
