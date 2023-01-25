using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AviaSales.DataBase;
using AviaSales.Models;

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
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.Tickets
                .Include(t => t.Class)
                .Include(t => t.Flight).ThenInclude(x => x.Plane)
                .Include(x => x.Flight).ThenInclude(x => x.DepartureAirport)
                .Include(x => x.Flight).ThenInclude(x => x.ArrivalAirport)
                .Include(t => t.User);
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
