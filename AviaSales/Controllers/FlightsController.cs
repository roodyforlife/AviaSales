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
    public class FlightsController : Controller
    {
        private readonly DataBaseContext _context;

        public FlightsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Flights
        public async Task<IActionResult> Index(string flightNumber, string planeName, int costFrom, int costTo, DateTime departureDateFrom,
            DateTime departureDateTo, FlightSort sort = FlightSort.FlightNumberAsc)
        {
            IQueryable<Flight> dataBaseContext = _context.Flights.Include(f => f.ArrivalAirport).Include(f => f.DepartureAirport).Include(f => f.Plane);

            if (!String.IsNullOrEmpty(flightNumber))
            {
                dataBaseContext = dataBaseContext.Where(x => x.FlightNumber.Contains(flightNumber));
            }

            if (!String.IsNullOrEmpty(planeName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Plane.Name.Contains(planeName));
            }

            if (costTo == 0)
            {
                if(dataBaseContext.Count() > 0)
                costTo = dataBaseContext.Max(x => x.Cost);
            }

            dataBaseContext = dataBaseContext.Where(x => x.Cost >= costFrom);
            dataBaseContext = dataBaseContext.Where(x => x.Cost <= costTo);

            if (departureDateTo.Year == 1)
            {
                departureDateTo = DateTime.Now;
            }

            dataBaseContext = dataBaseContext.Where(x => x.DepartureDate >= departureDateFrom);
            dataBaseContext = dataBaseContext.Where(x => x.DepartureDate <= departureDateTo);

            switch (sort)
            {
                case FlightSort.FlightNumberDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.FlightNumber);
                    break;
                case FlightSort.DepartureDateAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.DepartureDate);
                    break;
                case FlightSort.DepartureDateDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.DepartureDate);
                    break;
                case FlightSort.CostAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Cost);
                    break;
                case FlightSort.CostDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Cost);
                    break;
                case FlightSort.DepartureAirportAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.DepartureAirport.Name);
                    break;
                case FlightSort.ArrivalAirportAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.ArrivalAirport.Name);
                    break;
                default:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.FlightNumber);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(FlightSort)).Cast<FlightSort>()
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

            ViewBag.FlightNumber = flightNumber;
            ViewBag.PlaneName = planeName;
            ViewBag.CostFrom = costFrom;
            ViewBag.CostTo = costTo;
            ViewBag.DepartureDateFrom = departureDateFrom;
            ViewBag.DepartureDateTo = departureDateTo;

            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .Include(f => f.Plane)
                .ThenInclude(x => x.Tickets)
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // GET: Flights/Create
        public IActionResult Create()
        {
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "AirportId", "Name");
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "AirportId", "Name");
            ViewData["PlaneId"] = new SelectList(_context.Planes, "PlaneId", "Name");
            return View();
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightId,FlightNumber,DepartureDate,ArrivalDate,DepartureAirportId,ArrivalAirportId,PlaneId,ActualDeparture,ActualArrival,Cost")] Flight flight)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flight);
                await _context.SaveChangesAsync();
                List<Ticket> newTickets = new List<Ticket>();
                for (int i = 1; i <= 30; i++)
                {
                    newTickets.Add(new Ticket()
                    {
                        SeatNumber = i,
                        PurchaseDate = DateTime.Now,
                        Class = _context.Classes.FirstOrDefault(),
                        Flight = _context.Flights.FirstOrDefault(x => x.DepartureDate == flight.DepartureDate)
                    });
                }

                await _context.AddRangeAsync(newTickets);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "AirportId", "Name", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "AirportId", "Name", flight.DepartureAirportId);
            ViewData["PlaneId"] = new SelectList(_context.Planes, "PlaneId", "Name", flight.PlaneId);
            return View(flight);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound();
            }
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "AirportId", "Name", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "AirportId", "Name", flight.DepartureAirportId);
            ViewData["PlaneId"] = new SelectList(_context.Planes, "PlaneId", "Name", flight.PlaneId);
            return View(flight);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightId,FlightNumber,DepartureDate,ArrivalDate,DepartureAirportId,ArrivalAirportId,PlaneId,ActualDeparture,ActualArrival,Cost")] Flight flight)
        {
            if (id != flight.FlightId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flight.FlightId))
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
            ViewData["ArrivalAirportId"] = new SelectList(_context.Airports, "AirportId", "Name", flight.ArrivalAirportId);
            ViewData["DepartureAirportId"] = new SelectList(_context.Airports, "AirportId", "Name", flight.DepartureAirportId);
            ViewData["PlaneId"] = new SelectList(_context.Planes, "PlaneId", "Name", flight.PlaneId);
            return View(flight);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flights
                .Include(f => f.ArrivalAirport)
                .Include(f => f.DepartureAirport)
                .Include(f => f.Plane)
                .FirstOrDefaultAsync(m => m.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flights.FindAsync(id);
            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flights.Any(e => e.FlightId == id);
        }
    }
}
