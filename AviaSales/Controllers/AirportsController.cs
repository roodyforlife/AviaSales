using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AviaSales.DataBase;
using AviaSales.Models;
using AviaSales.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AviaSales.Controllers
{
    public class AirportsController : Controller
    {
        private readonly DataBaseContext _context;

        public AirportsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Airports
        public async Task<IActionResult> Index(string name, string city, string postcode, AirportSort sort = AirportSort.NameAsc)
        {
            IQueryable<Airport> airports = _context.Airports;

            if (!String.IsNullOrEmpty(name))
            {
                airports = airports.Where(x => x.Name.Contains(name));
            }

            if (!String.IsNullOrEmpty(city))
            {
                airports = airports.Where(x => x.City.Contains(city));
            }

            if (!String.IsNullOrEmpty(postcode))
            {
                airports = airports.Where(x => x.Postcode.Contains(postcode));
            }

            switch (sort)
            {
                case AirportSort.NameDesc:
                    airports = airports.OrderByDescending(x => x.Name);
                    break;
                case AirportSort.CityAsc:
                    airports = airports.OrderBy(x => x.City);
                    break;
                case AirportSort.CityDesc:
                    airports = airports.OrderByDescending(x => x.City);
                    break;
                case AirportSort.AddressAsc:
                    airports = airports.OrderBy(x => x.Address);
                    break;
                case AirportSort.AddressDesc:
                    airports = airports.OrderByDescending(x => x.Address);
                    break;
                case AirportSort.StateAsc:
                    airports = airports.OrderBy(x => x.State);
                    break;
                case AirportSort.StateDesc:
                    airports = airports.OrderByDescending(x => x.State);
                    break;
                default:
                    airports = airports.OrderBy(x => x.Name);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(AirportSort)).Cast<AirportSort>()
               .Select(x => new SelectListItem
               {
                   Text = x.GetType()
           .GetMember(x.ToString())
           .FirstOrDefault()
           .GetCustomAttribute<DisplayAttribute>()?
           .GetName(),
                   Value = x.ToString(),
                   Selected = (x == sort)
               }).ToList();

            ViewBag.Name = name;
            ViewBag.City = city;
            ViewBag.Postcode = postcode;

            return View(await airports.ToListAsync());
        }

        // GET: Airports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports
                .FirstOrDefaultAsync(m => m.AirportId == id);
            if (airport == null)
            {
                return NotFound();
            }

            return View(airport);
        }

        // GET: Airports/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Airports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AirportId,Name,State,City,Address,Postcode")] Airport airport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(airport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        // GET: Airports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports.FindAsync(id);
            if (airport == null)
            {
                return NotFound();
            }
            return View(airport);
        }

        // POST: Airports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AirportId,Name,State,City,Address,Postcode")] Airport airport)
        {
            if (id != airport.AirportId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(airport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AirportExists(airport.AirportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(airport);
        }

        // GET: Airports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var airport = await _context.Airports
                .FirstOrDefaultAsync(m => m.AirportId == id);
            if (airport == null)
            {
                return NotFound();
            }

            return View(airport);
        }

        // POST: Airports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airport = await _context.Airports.FindAsync(id);
            _context.Airports.Remove(airport);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AirportExists(int id)
        {
            return _context.Airports.Any(e => e.AirportId == id);
        }
    }
}
