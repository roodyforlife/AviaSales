using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Enums
{
    public enum TicketSort
    {
        [Display(Name = "Sort by seat number")]
        SeatNumberAsc,
        [Display(Name = "Sort by user email (A-Z)")]
        UserEmailAsc,
        [Display(Name = "Sort by user email (Z-A)")]
        UserEmailDesc,
        [Display(Name = "Sort by flight number (A-Z)")]
        FlightNumberAsc,
        [Display(Name = "Sort by flight number (Z-A)")]
        FlightNumberDesc,
        [Display(Name = "Sort by class name (A-Z)")]
        ClassNameAsc,
        [Display(Name = "Sort by class name (Z-A)")]
        ClassNameDesc,
        [Display(Name = "Sort by purchase date (new first)")]
        DateAsc,
        [Display(Name = "Sort by purchase date (old first)")]
        DateDesc
    }
}
