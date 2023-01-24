using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Food
    {
        public int FoodId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int Cost { get; set; }
        public List<TicketFood> TicketFoods { get; set; }
    }
}
