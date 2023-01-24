using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public Airport DepartureAirport { get; set; }
        public int DepartureAirportId { get; set; }
        public Airport ArrivalAirport { get; set; }
        public int ArrivalAirportId { get; set; }
        public Plane Plane { get; set; }
        public int PlaneId { get; set; }
        public DateTime ActualDeparture { get; set; }
        public DateTime ActualArrival { get; set; }
        public int Cost { get; set; }
        public List<FlightTicket> FlightTickets { get; set; }
    }
}
