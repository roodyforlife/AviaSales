using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Class
    {
        public int ClassId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
