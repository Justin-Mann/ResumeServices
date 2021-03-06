using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonAPI.Models {
    public class Address {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
    }
}
