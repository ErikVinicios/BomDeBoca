﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BomDeBoca.Shared.Results {
    public class APIResult {
        public bool Result { get; set; }
        public string? Message { get; set; }
        public object? Obj { get; set; }
    }
}
