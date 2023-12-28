using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bakery.Data;
using Bakery.Models;

namespace Bakery.Controllers
{
    public class SuppliesController : Controller
    {
        private readonly BakeryDBContext _context;

        public SuppliesController(BakeryDBContext context)
        {
            _context = context;
        }

        // GET: Supplies
        public async Task<IActionResult> Index(string supplier)
        {
            var supplies = _context.Supplies.Include(s => s.Ingredient).AsQueryable();

            // Примените фильтр по поставщику, если он указан
            if (!string.IsNullOrEmpty(supplier))
            {
                supplies = supplies.Where(s => s.Supplier.Contains(supplier));
            }

            return View(await supplies.ToListAsync());
        }

        // GET: Supplies/Детали/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Supplies == null)
            {
                return NotFound();
            }

            var supply = await _context.Supplies
                .Include(s => s.Ingredient)
                .FirstOrDefaultAsync(m => m.SupplyId == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // GET: Supplies/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId");
            return View();
        }

        // POST: Supplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more Детали, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplyId,IngredientId,Supplier,ProductName,Quantity,Price,SupplyDate")] Supply supply)
        {
            if (ModelState.IsValid)
            {
                _context.Add(supply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", supply.IngredientId);
            return View(supply);
        }

        // GET: Supplies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Supplies == null)
            {
                return NotFound();
            }

            var supply = await _context.Supplies.FindAsync(id);
            if (supply == null)
            {
                return NotFound();
            }
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", supply.IngredientId);
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more Детали, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplyId,IngredientId,Supplier,ProductName,Quantity,Price,SupplyDate")] Supply supply)
        {
            if (id != supply.SupplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyExists(supply.SupplyId))
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
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", supply.IngredientId);
            return View(supply);
        }

        // GET: Supplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Supplies == null)
            {
                return NotFound();
            }

            var supply = await _context.Supplies
                .Include(s => s.Ingredient)
                .FirstOrDefaultAsync(m => m.SupplyId == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // POST: Supplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Supplies == null)
            {
                return Problem("Entity set 'BakeryDBContext.Supplies'  is null.");
            }
            var supply = await _context.Supplies.FindAsync(id);
            if (supply != null)
            {
                _context.Supplies.Remove(supply);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyExists(int id)
        {
          return (_context.Supplies?.Any(e => e.SupplyId == id)).GetValueOrDefault();
        }
    }
}
