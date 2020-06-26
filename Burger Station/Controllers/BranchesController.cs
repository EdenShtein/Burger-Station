using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Burger_Station.Data;
using Burger_Station.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TestShop.Controllers
{
    public class BranchesController : Controller
    {
        private readonly Burger_StationContext _context;

        public BranchesController(Burger_StationContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            return View(await _context.Branch.ToListAsync());
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .Include(bi => bi.BranchItems)
                .ThenInclude(b => b.Item)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public async Task<IActionResult> Create()
        {
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Branches");
            }

            ViewBag.ItemsId = new SelectList(await _context.Item.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Address,City,District")] Branch branch, int[] ItemId)
        {
            if (ModelState.IsValid)
            {
                branch.BranchItems = new List<BranchItem>();

                foreach (var id in ItemId)
                {
                    branch.BranchItems.Add(new BranchItem() { BranchId = branch.Id, ItemId = id });
                }

                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Branches");
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }

            ViewBag.ItemsId = new SelectList(await _context.Item.ToListAsync(), "Id", "Name");
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,City,District")] Branch branch, int[] ItemId)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // NOT WORKING TODO
                    //branch.BranchItems = await _context.BranchItem.Where(bi => (bi.BranchId == branch.Id)).ToListAsync();

                    branch.BranchItems = await _context.BranchItem
                        .Where(bi => (bi.BranchId == branch.Id))
                        .Include(bi => bi.Item)
                        .ToListAsync();

                    foreach (var i in ItemId)
                    {
                        if (branch.BranchItems.Any(bi => (bi.ItemId == i)))
                        {
                            continue;
                        }

                        var item = await _context.Item.Where(item => item.Id == i).FirstOrDefaultAsync();

                        branch.BranchItems.Add(new BranchItem() { BranchId = branch.Id, ItemId = i, Item = item });
                    }
                    // NOT WORKING TODO

                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            return View(branch);
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Branches");
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }
    }
}
