using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string Telephone { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int Age { get; set; }
        public bool IsMale { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Field entered incorrectly")]
        public string HomeAddress { get; set; }
        public bool IsBanned { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
