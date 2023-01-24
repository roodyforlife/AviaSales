using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public int Age { get; set; }
        public bool IsMale { get; set; }
        public string HomeAddress { get; set; }
        public bool IsBanned { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
