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
    public class TicketsController : Controller
    {
        private readonly DataBaseContext _context;

        public TicketsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: Tickets
        public async Task<IActionResult> Index(string flightNumber, int seatNumber, string userEmail, string className,
            string planeName, DateTime dateFrom, DateTime dateTo, TicketSort sort = TicketSort.SeatNumberAsc)
        {
            IQueryable<Ticket> dataBaseContext = _context.Tickets
                .Include(t => t.Class)
                .Include(t => t.Flight).ThenInclude(x => x.Plane)
                .Include(x => x.Flight).ThenInclude(x => x.DepartureAirport)
                .Include(x => x.Flight).ThenInclude(x => x.ArrivalAirport)
                .Include(t => t.User);

            if (!String.IsNullOrEmpty(flightNumber))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Flight.FlightNumber.Contains(flightNumber));
            }

            if (seatNumber != 0)
            {
                dataBaseContext = dataBaseContext.Where(x => x.SeatNumber == seatNumber);
            }

            if (!String.IsNullOrEmpty(userEmail))
            {
                dataBaseContext = dataBaseContext.Where(x => x.User.Email.Contains(userEmail));
            }

            if (!String.IsNullOrEmpty(className))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Class.Name.Contains(className));
            }

            if (!String.IsNullOrEmpty(planeName))
            {
                dataBaseContext = dataBaseContext.Where(x => x.Flight.Plane.Name.Contains(planeName));
            }

            if (dateTo.Year == 1)
            {
                dateTo = DateTime.Now.AddDays(1);
            }

            dataBaseContext = dataBaseContext.Where(x => x.PurchaseDate >= dateFrom);
            dataBaseContext = dataBaseContext.Where(x => x.PurchaseDate <= dateTo);

            switch (sort)
            {
                case TicketSort.UserEmailAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.User.Email);
                    break;
                case TicketSort.UserEmailDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.User.Email);
                    break;
                case TicketSort.FlightNumberAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Flight.FlightNumber);
                    break;
                case TicketSort.FlightNumberDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Flight.FlightNumber);
                    break;
                case TicketSort.ClassNameAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.Class.Name);
                    break;
                case TicketSort.ClassNameDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.Class.Name);
                    break;
                case TicketSort.DateAsc:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.PurchaseDate);
                    break;
                case TicketSort.DateDesc:
                    dataBaseContext = dataBaseContext.OrderByDescending(x => x.PurchaseDate);
                    break;
                default:
                    dataBaseContext = dataBaseContext.OrderBy(x => x.SeatNumber);
                    break;
            }

            ViewBag.Sort = (List<SelectListItem>)Enum.GetValues(typeof(TicketSort)).Cast<TicketSort>()
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
            ViewBag.SeatNumber = seatNumber;
            ViewBag.UserEmail = userEmail;
            ViewBag.ClassName = className;
            ViewBag.PlaneName = planeName;
            ViewBag.DateFrom = dateFrom;
            ViewBag.DateTo = dateTo;

            return View(await dataBaseContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Class)
                .Include(t => t.Flight).ThenInclude(x => x.Plane)
                .Include(x => x.Flight).ThenInclude(x => x.DepartureAirport)
                .Include(x => x.Flight).ThenInclude(x => x.ArrivalAirport)
                .Include(t => t.User)
                .Include(x => x.TicketFoods)
                .ThenInclude(x => x.Food)
                .FirstOrDefaultAsync(m => m.TicketId == id);


            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name");
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,PurchaseDate,SeatNumber,UserId,FlightId,ClassId")] Ticket ticket)
        {
            if (_context.Users.FirstOrDefault(x => x.UserId == ticket.UserId).IsBanned)
            {
                ModelState.AddModelError("UserId", "This user has been banned");
            }

            if (ModelState.IsValid)
            {
                _context.Add(ticket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", ticket.ClassId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", ticket.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", ticket.ClassId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", ticket.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", ticket.UserId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,PurchaseDate,SeatNumber,UserId,FlightId,ClassId")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (_context.Users.FirstOrDefault(x => x.UserId == ticket.UserId).IsBanned)
            {
                ModelState.AddModelError("UserId", "This user has been banned");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
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
            ViewData["ClassId"] = new SelectList(_context.Classes, "ClassId", "Name", ticket.ClassId);
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", ticket.FlightId);
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", ticket.UserId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Class)
                .Include(t => t.Flight).ThenInclude(x => x.Plane)
                .Include(x => x.Flight).ThenInclude(x => x.DepartureAirport)
                .Include(x => x.Flight).ThenInclude(x => x.ArrivalAirport)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
