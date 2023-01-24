using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Airport
    {
        public int AirportId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string State { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string City { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Postcode { get; set; }
        public List<Flight> Departures { get; set; }
        public List<Flight> Arrivals { get; set; }
    }
}
