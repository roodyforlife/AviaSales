using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public List<TicketFood> TicketFoods { get; set; }
    }
}
