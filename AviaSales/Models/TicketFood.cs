using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class TicketFood
    {
        public int TicketFoodId { get; set; }
        public Food Food { get; set; }
        public int FoodId { get; set; }
        public Ticket Ticket { get; set; }
        public int TicketId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int Quantity { get; set; }
    }
}
