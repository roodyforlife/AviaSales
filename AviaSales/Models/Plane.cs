using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Plane
    {
        public int PlaneId { get; set; }
        public string Name { get; set; }
        public List<Flight> Flights { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
