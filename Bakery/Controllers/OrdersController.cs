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
    public class OrdersController : Controller
    {
        private readonly BakeryDBContext _context;

        public OrdersController(BakeryDBContext context)
        {
            _context = context;
        }

        // GET: Orders/Не выполненные заказы
        public async Task<IActionResult> UncompletedOrders([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return View(new List<Order>()); // Возвращайте пустой список, если дата не указана
            }

            var uncompletedOrders = await _context.Orders
                .Include(o => o.Employee)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && !o.IsCompleted)
                .ToListAsync();

            return View(uncompletedOrders);
        }

        // GET: Orders/Испорченные заказы
        public async Task<IActionResult> DamagedOrders([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return View(new List<Order>()); // Возвращайте пустой список, если дата не указана
            }

            var damagedOrders = await _context.Orders
                .Include(o => o.Employee)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate && o.IsDamaged)
                .ToListAsync();

            return View(damagedOrders);
        }

        // GET: Orders
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var orders = _context.Orders.AsQueryable();

            // Примените фильтр по дате заказа, если он указан
            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.OrderDate <= endDate.Value);
            }

            // Примените фильтр по дате доставки, если он указан
            if (startDate.HasValue)
            {
                orders = orders.Where(o => o.DeliveryDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                orders = orders.Where(o => o.DeliveryDate <= endDate.Value);
            }

            return View(await orders.ToListAsync());
        }

        // GET: Orders/Детали/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.BakeryProduct)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId");
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more Детали, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,EmployeeId,BakeryProductId,CustomerName,ProductName,ProductType,Quantity,Price,OrderDate,DeliveryDate,IsCompleted,IsDamaged")] Order order)
        {
            // Проверка существования EmployeeId
            if (!_context.Employees.Any(e => e.EmployeeId == order.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Invalid EmployeeId");
            }

            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId", order.BakeryProductId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId", order.BakeryProductId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more Детали, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,EmployeeId,BakeryProductId,CustomerName,ProductName,ProductType,Quantity,Price,OrderDate,DeliveryDate,IsCompleted,IsDamaged")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (!_context.Employees.Any(e => e.EmployeeId == order.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "Invalid EmployeeId");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeId", order.EmployeeId);
            ViewData["BakeryProductId"] = new SelectList(_context.BakeryProducts, "BakeryProductId", "BakeryProductId", order.BakeryProductId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.BakeryProduct)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'BakeryDBContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
