using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Flight
    {
        public int FlightId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Field entered incorrectly")]
        public string FlightNumber { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public DateTime DepartureDate { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public DateTime ArrivalDate { get; set; }
        [ForeignKey("DepartureAirportId")]
        public Airport DepartureAirport { get; set; }
        public int DepartureAirportId { get; set; }
        [ForeignKey("ArrivalAirportId")]
        public Airport ArrivalAirport { get; set; }
        public int ArrivalAirportId { get; set; }
        public Plane Plane { get; set; }
        public int PlaneId { get; set; }
        public DateTime ActualDeparture { get; set; }
        public DateTime ActualArrival { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int Cost { get; set; }
        public List<FlightTicket> FlightTickets { get; set; }
    }
}
