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
    public class FlightTicketsController : Controller
    {
        private readonly DataBaseContext _context;

        public FlightTicketsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: FlightTickets
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.FlightTickets.Include(f => f.Flight).Include(f => f.Ticket);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: FlightTickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightTicket = await _context.FlightTickets
                .Include(f => f.Flight)
                .Include(f => f.Ticket)
                .FirstOrDefaultAsync(m => m.FlightTicketId == id);
            if (flightTicket == null)
            {
                return NotFound();
            }

            return View(flightTicket);
        }

        // GET: FlightTickets/Create
        public IActionResult Create()
        {
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId");
            return View();
        }

        // POST: FlightTickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FlightTicketId,FlightId,TicketId")] FlightTicket flightTicket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(flightTicket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", flightTicket.FlightId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", flightTicket.TicketId);
            return View(flightTicket);
        }

        // GET: FlightTickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightTicket = await _context.FlightTickets.FindAsync(id);
            if (flightTicket == null)
            {
                return NotFound();
            }
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", flightTicket.FlightId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", flightTicket.TicketId);
            return View(flightTicket);
        }

        // POST: FlightTickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FlightTicketId,FlightId,TicketId")] FlightTicket flightTicket)
        {
            if (id != flightTicket.FlightTicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flightTicket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightTicketExists(flightTicket.FlightTicketId))
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
            ViewData["FlightId"] = new SelectList(_context.Flights, "FlightId", "FlightNumber", flightTicket.FlightId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", flightTicket.TicketId);
            return View(flightTicket);
        }

        // GET: FlightTickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flightTicket = await _context.FlightTickets
                .Include(f => f.Flight)
                .Include(f => f.Ticket)
                .FirstOrDefaultAsync(m => m.FlightTicketId == id);
            if (flightTicket == null)
            {
                return NotFound();
            }

            return View(flightTicket);
        }

        // POST: FlightTickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flightTicket = await _context.FlightTickets.FindAsync(id);
            _context.FlightTickets.Remove(flightTicket);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightTicketExists(int id)
        {
            return _context.FlightTickets.Any(e => e.FlightTicketId == id);
        }
    }
}
