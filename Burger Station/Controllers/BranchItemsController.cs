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
    public class BranchItemsController : Controller
    {
        private readonly Burger_StationContext _context;

        public BranchItemsController(Burger_StationContext context)
        {
            _context = context;
        }

        // GET: BranchItems
        public async Task<IActionResult> Index()
        {
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var testShopContext = _context.BranchItem.Include(b => b.Branch).Include(b => b.Item);

            return View(await testShopContext.ToListAsync());
        }

        // GET: BranchItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var branchItem = await _context.BranchItem
                .Include(b => b.Branch)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(m => m.BranchId == id);

            if (branchItem == null)
            {
                return NotFound();
            }

            return View(branchItem);
        }

        // GET: BranchItems/Create
        public IActionResult Create()
        {
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["BranchId"] = new SelectList(_context.Set<Branch>(), "Id", "Id");
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id");

            return View();
        }

        // POST: BranchItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,ItemId")] BranchItem branchItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branchItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["BranchId"] = new SelectList(_context.Set<Branch>(), "Id", "Id", branchItem.BranchId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", branchItem.ItemId);
            
            return View(branchItem);
        }

        // GET: BranchItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var branchItem = await _context.BranchItem.FindAsync(id);
            if (branchItem == null)
            {
                return NotFound();
            }
            ViewData["BranchId"] = new SelectList(_context.Set<Branch>(), "Id", "Id", branchItem.BranchId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", branchItem.ItemId);
            
            return View(branchItem);
        }

        // POST: BranchItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BranchId,ItemId")] BranchItem branchItem)
        {
            if (id != branchItem.BranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branchItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchItemExists(branchItem.BranchId))
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

            ViewData["BranchId"] = new SelectList(_context.Set<Branch>(), "Id", "Id", branchItem.BranchId);
            ViewData["ItemId"] = new SelectList(_context.Item, "Id", "Id", branchItem.ItemId);
            
            return View(branchItem);
        }

        // GET: BranchItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            var branchItem = await _context.BranchItem
                .Include(b => b.Branch)
                .Include(b => b.Item)
                .FirstOrDefaultAsync(m => m.BranchId == id);
           
            if (branchItem == null)
            {
                return NotFound();
            }

            return View(branchItem);
        }

        // POST: BranchItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branchItem = await _context.BranchItem.FindAsync(id);
            
            _context.BranchItem.Remove(branchItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchItemExists(int id)
        {
            return _context.BranchItem.Any(e => e.BranchId == id);
        }
    }
}
