using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Enums
{
    public enum FlightSort
    {
        [Display(Name = "Sort by flight number (A-Z)")]
        FlightNumberAsc,
        [Display(Name = "Sort by flight number (Z-A)")]
        FlightNumberDesc,
        [Display(Name = "Sort by departure date (new first)")]
        DepartureDateAsc,
        [Display(Name = "Sort by departure date (old first)")]
        DepartureDateDesc,
        [Display(Name = "Sort by cost (expensive first)")]
        CostAsc,
        [Display(Name = "Sort by cost (cheap first)")]
        CostDesc,
        [Display(Name = "Sort by departure airport")]
        DepartureAirportAsc,
        [Display(Name = "Sort by arrival airport")]
        ArrivalAirportAsc
    }
}
