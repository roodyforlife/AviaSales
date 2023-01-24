using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class FlightTicket
    {
        public int FlightTicketId { get; set; }
        public Flight Flight { get; set; }
        public int FlightId { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }
    }
}
