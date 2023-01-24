using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public List<TicketFood> TicketFoods { get; set; }
        public List<FlightTicket> FlightTickets { get; set; }
    }
}
