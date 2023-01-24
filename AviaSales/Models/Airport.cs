using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Airport
    {
        public int AirportId { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
    }
}
