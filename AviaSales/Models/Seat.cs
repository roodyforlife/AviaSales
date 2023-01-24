﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Models
{
    public class Seat
    {
        public int SeatId { get; set; }
        [Required(ErrorMessage = "This is a required field")]
        public int SeatNumber { get; set; }
        public Plane Plane { get; set; }
        public int PlaneId { get; set; }
        public Class Class { get; set; }
        public int ClassId { get; set; }
    }
}
