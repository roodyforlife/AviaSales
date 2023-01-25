using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AviaSales.Enums
{
    public enum AirportSort
    {
        [Display(Name = "Sort by name (A-Z)")]
        NameAsc,
        [Display(Name = "Sort by name (Z-A)")]
        NameDesc,
        [Display(Name = "Sort by city (A-Z)")]
        CityAsc,
        [Display(Name = "Sort by city (Z-A)")]
        CityDesc,
        [Display(Name = "Sort by address (A-Z)")]
        AddressAsc,
        [Display(Name = "Sort by address (Z-A)")]
        AddressDesc,
        [Display(Name = "Sort by state (A-Z)")]
        StateAsc,
        [Display(Name = "Sort by state (Z-A)")]
        StateDesc
    }
}
