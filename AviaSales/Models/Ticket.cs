using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public DateTime PurchaseDate { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int SeatNumber { get; set; }
        public User User { get; set; }
        public int? UserId { get; set; }
        public Plane Plane { get; set; }
        public int PlaneId { get; set; }
        public Class Class { get; set; }
        public int ClassId { get; set; }
        public List<TicketFood> TicketFoods { get; set; }
    }
}
