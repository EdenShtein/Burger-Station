using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Burger_Station.Data;
using Burger_Station.Models;

namespace Burger_Station.Controllers
{
    public class ItemOrdersController : Controller
    {
        private readonly Burger_StationContext _context;

        public ItemOrdersController(Burger_StationContext context)
        {
            _context = context;
        }

        // GET: ItemOrders
        public async Task<IActionResult> Index()
        {
            var burger_StationContext = _context.ItemOrder.Include(i => i.Item).Include(i => i.Order);
            return View(await burger_StationContext.ToListAsync());
        }

        // GET: ItemOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemOrder = await _context.ItemOrder
                .Include(i => i.Item)
                .Include(i => i.Order)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (itemOrder == null)
            {
                return NotFound();
            }

            return View(itemOrder);
        }

        // GET: ItemOrders/Create
        public IActionResult Create()
        {
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name");
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id");
            return View();
        }

        // POST: ItemOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemId,OrderId")] ItemOrder itemOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(itemOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", itemOrder.ItemId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", itemOrder.OrderId);
            return View(itemOrder);
        }

        // GET: ItemOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemOrder = await _context.ItemOrder.FindAsync(id);
            if (itemOrder == null)
            {
                return NotFound();
            }
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", itemOrder.ItemId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", itemOrder.OrderId);
            return View(itemOrder);
        }

        // POST: ItemOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemId,OrderId")] ItemOrder itemOrder)
        {
            if (id != itemOrder.ItemId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(itemOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemOrderExists(itemOrder.ItemId))
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
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Name", itemOrder.ItemId);
            ViewData["OrderId"] = new SelectList(_context.Order, "Id", "Id", itemOrder.OrderId);
            return View(itemOrder);
        }

        // GET: ItemOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var itemOrder = await _context.ItemOrder
                .Include(i => i.Item)
                .Include(i => i.Order)
                .FirstOrDefaultAsync(m => m.ItemId == id);
            if (itemOrder == null)
            {
                return NotFound();
            }

            return View(itemOrder);
        }

        // POST: ItemOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var itemOrder = await _context.ItemOrder.FindAsync(id);
            _context.ItemOrder.Remove(itemOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemOrderExists(int id)
        {
            return _context.ItemOrder.Any(e => e.ItemId == id);
        }
    }
}
