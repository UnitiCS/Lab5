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
    public class BakeryProductsController : Controller
    {
        private readonly BakeryDBContext _context;

        public BakeryProductsController(BakeryDBContext context)
        {
            _context = context;
        }

        // GET: BakeryProducts
        public async Task<IActionResult> Index(string typeFilter)
        {
            var products = _context.BakeryProducts.AsQueryable();

            // Применение фильтра по типу
            if (!string.IsNullOrEmpty(typeFilter))
            {
                products = products.Where(product => product.Type.Contains(typeFilter));
            }

            return _context.BakeryProducts != null ?
                      View(await products.ToListAsync()) :
                      Problem("Entity set 'BakeryDBContext.BakeryProducts' is null.");
        }

        // GET: BakeryProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BakeryProducts == null)
            {
                return NotFound();
            }

            var bakeryProduct = await _context.BakeryProducts
                .FirstOrDefaultAsync(m => m.BakeryProductId == id);
            if (bakeryProduct == null)
            {
                return NotFound();
            }

            return View(bakeryProduct);
        }

        // GET: BakeryProducts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BakeryProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BakeryProductId,Name,Type,Description")] BakeryProduct bakeryProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bakeryProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bakeryProduct);
        }

        // GET: BakeryProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BakeryProducts == null)
            {
                return NotFound();
            }

            var bakeryProduct = await _context.BakeryProducts.FindAsync(id);
            if (bakeryProduct == null)
            {
                return NotFound();
            }
            return View(bakeryProduct);
        }

        // POST: BakeryProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BakeryProductId,Name,Type,Description")] BakeryProduct bakeryProduct)
        {
            if (id != bakeryProduct.BakeryProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bakeryProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BakeryProductExists(bakeryProduct.BakeryProductId))
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
            return View(bakeryProduct);
        }

        // GET: BakeryProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BakeryProducts == null)
            {
                return NotFound();
            }

            var bakeryProduct = await _context.BakeryProducts
                .FirstOrDefaultAsync(m => m.BakeryProductId == id);
            if (bakeryProduct == null)
            {
                return NotFound();
            }

            return View(bakeryProduct);
        }

        // POST: BakeryProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BakeryProducts == null)
            {
                return Problem("Entity set 'BakeryDBContext.BakeryProducts' is null.");
            }
            var bakeryProduct = await _context.BakeryProducts.FindAsync(id);
            if (bakeryProduct != null)
            {
                _context.BakeryProducts.Remove(bakeryProduct);
                await _context.SaveChangesAsync();
            }

            
            return RedirectToAction(nameof(Index));
        }

        private bool BakeryProductExists(int id)
        {
            return (_context.BakeryProducts?.Any(e => e.BakeryProductId == id)).GetValueOrDefault();
        }
    }
}
