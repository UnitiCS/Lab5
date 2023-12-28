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
    public class BreadRecipesController : Controller
    {
        private readonly BakeryDBContext _context;

        public BreadRecipesController(BakeryDBContext context)
        {
            _context = context;
        }

        // GET: BreadRecipes
        public async Task<IActionResult> Index(int? quantityFilter, decimal? priceFilter)
        {
            var recipes = _context.BreadRecipes.Include(b => b.BakeryProduct).Include(b => b.Ingredient).AsQueryable();

            // Применение фильтра по QuantityPerUnit
            if (quantityFilter.HasValue)
            {
                recipes = recipes.Where(recipe => recipe.QuantityPerUnit == quantityFilter);
            }

            // Применение фильтра по Price
            if (priceFilter.HasValue)
            {
                recipes = recipes.Where(recipe => recipe.Price == priceFilter);
            }

            return View(await recipes.ToListAsync());
        }

        // GET: BreadRecipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BreadRecipes == null)
            {
                return NotFound();
            }

            var breadRecipe = await _context.BreadRecipes
                .Include(b => b.BakeryProduct)
                .Include(b => b.Ingredient)
                .FirstOrDefaultAsync(m => m.BreadRecipeId == id);
            if (breadRecipe == null)
            {
                return NotFound();
            }

            return View(breadRecipe);
        }

        // GET: BreadRecipes/Create
        public IActionResult Create()
        {
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId");
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId");
            return View();
        }

        // POST: BreadRecipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more Детали, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BreadRecipeId,IngredientId,BakeryProductId,ProductName,IngredientName,QuantityPerUnit,Price")] BreadRecipe breadRecipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(breadRecipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId", breadRecipe.BakeryProductId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", breadRecipe.IngredientId);
            return View(breadRecipe);
        }

        // GET: BreadRecipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BreadRecipes == null)
            {
                return NotFound();
            }

            var breadRecipe = await _context.BreadRecipes.FindAsync(id);
            if (breadRecipe == null)
            {
                return NotFound();
            }
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId", breadRecipe.BakeryProductId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", breadRecipe.IngredientId);
            return View(breadRecipe);
        }

        // POST: BreadRecipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more Детали, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BreadRecipeId,IngredientId,BakeryProductId,ProductName,IngredientName,QuantityPerUnit,Price")] BreadRecipe breadRecipe)
        {
            if (id != breadRecipe.BreadRecipeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(breadRecipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BreadRecipeExists(breadRecipe.BreadRecipeId))
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
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId", breadRecipe.BakeryProductId);
            ViewData["IngredientId"] = new SelectList(_context.Ingredients, "IngredientId", "IngredientId", breadRecipe.IngredientId);
            return View(breadRecipe);
        }

        // GET: BreadRecipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BreadRecipes == null)
            {
                return NotFound();
            }

            var breadRecipe = await _context.BreadRecipes
                .Include(b => b.BakeryProduct)
                .Include(b => b.Ingredient)
                .FirstOrDefaultAsync(m => m.BreadRecipeId == id);
            if (breadRecipe == null)
            {
                return NotFound();
            }

            return View(breadRecipe);
        }

        // POST: BreadRecipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BreadRecipes == null)
            {
                return Problem("Entity set 'BakeryDBContext.BreadRecipes'  is null.");
            }
            var breadRecipe = await _context.BreadRecipes.FindAsync(id);
            if (breadRecipe != null)
            {
                _context.BreadRecipes.Remove(breadRecipe);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BreadRecipeExists(int id)
        {
          return (_context.BreadRecipes?.Any(e => e.BreadRecipeId == id)).GetValueOrDefault();
        }
    }
}
