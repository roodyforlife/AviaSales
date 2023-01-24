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
    public class TicketFoodsController : Controller
    {
        private readonly DataBaseContext _context;

        public TicketFoodsController(DataBaseContext context)
        {
            _context = context;
        }

        // GET: TicketFoods
        public async Task<IActionResult> Index()
        {
            var dataBaseContext = _context.TicketFoods.Include(t => t.Food).Include(t => t.Ticket);
            return View(await dataBaseContext.ToListAsync());
        }

        // GET: TicketFoods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketFood = await _context.TicketFoods
                .Include(t => t.Food)
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.TicketFoodId == id);
            if (ticketFood == null)
            {
                return NotFound();
            }

            return View(ticketFood);
        }

        // GET: TicketFoods/Create
        public IActionResult Create()
        {
            ViewData["FoodId"] = new SelectList(_context.Foods, "FoodId", "Name");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId");
            return View();
        }

        // POST: TicketFoods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketFoodId,FoodId,TicketId,Quantity")] TicketFood ticketFood)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ticketFood);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "FoodId", "Name", ticketFood.FoodId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", ticketFood.TicketId);
            return View(ticketFood);
        }

        // GET: TicketFoods/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketFood = await _context.TicketFoods.FindAsync(id);
            if (ticketFood == null)
            {
                return NotFound();
            }
            ViewData["FoodId"] = new SelectList(_context.Foods, "FoodId", "Name", ticketFood.FoodId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", ticketFood.TicketId);
            return View(ticketFood);
        }

        // POST: TicketFoods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketFoodId,FoodId,TicketId,Quantity")] TicketFood ticketFood)
        {
            if (id != ticketFood.TicketFoodId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketFood);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketFoodExists(ticketFood.TicketFoodId))
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
            ViewData["FoodId"] = new SelectList(_context.Foods, "FoodId", "Name", ticketFood.FoodId);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", ticketFood.TicketId);
            return View(ticketFood);
        }

        // GET: TicketFoods/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketFood = await _context.TicketFoods
                .Include(t => t.Food)
                .Include(t => t.Ticket)
                .FirstOrDefaultAsync(m => m.TicketFoodId == id);
            if (ticketFood == null)
            {
                return NotFound();
            }

            return View(ticketFood);
        }

        // POST: TicketFoods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticketFood = await _context.TicketFoods.FindAsync(id);
            _context.TicketFoods.Remove(ticketFood);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketFoodExists(int id)
        {
            return _context.TicketFoods.Any(e => e.TicketFoodId == id);
        }
    }
}
