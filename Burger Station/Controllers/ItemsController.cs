using System;
using System.Linq;
using System.Threading.Tasks;
using Burger_Station.Data;
using Burger_Station.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Burger_Station.Controllers
{
    public class ItemsController : Controller
    {
        private readonly Burger_StationContext _context;

        public ItemsController(Burger_StationContext context)
        {
            _context = context;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");
            
            return View(await _context.Item.ToListAsync());

        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .Include(i => i.Comments)
                .FirstOrDefaultAsync(m => m.Id == id);

            item = await _context.Item
                .Include(bi => bi.BranchItems)
                .ThenInclude(b => b.Branch)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            ViewBag.ItemName = item.Name;
            ViewBag.itemType = item.Type.ToString();

            ViewBag.joinList = (from i in _context.Item
                            where (i.Id == id)
                            join c in _context.Comment on i.Id equals c.Item.Id
                            orderby c.PostDate descending
                            select new ItemComment
                            {
                                ItemId = i.Id,
                                ItemName = i.Name,
                                PostTitle = c.PostTitle,
                                PostedBy = c.PostedBy,
                                PostBody = c.PostBody,
                                PostDate = c.PostDate
                            }
                            );

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            string type = HttpContext.Session.GetString("Type");
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Items");
            }

            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Name,Price")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Items");
            }

            var item = await _context.Item.FindAsync(id);
            
            if (item == null)
            {
                return NotFound();
            }
            
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Name,Price")] Item item)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Items");
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
           
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item
                    .Include(x => x.SatisfiedUsers)
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (item.SatisfiedUsers.Count() != 0)
            {
                var users = await _context.User
                .Include(u => u.FavoriteItem)
                .ToListAsync();
              
                foreach (var user in users)
                {
                    if(user.FavoriteItem == null)
                    {
                        continue;
                    }

                    if (user.FavoriteItem.Id == id)
                    {
                        user.FavoriteItem = null;
                        _context.User.Update(user);

                    }
                }
            }

            item = await _context.Item
                .Include(x => x.Comments)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(item.Comments.Count() !=0)
            {
                var comments = await _context.Comment
                    .Include(x => x.Item)
                    .ToListAsync();
                
                foreach(var comment in comments)
                {
                    if (comment.Item == null)
                    {
                        continue;
                    }

                    if(comment.Item.Id == id)
                    {
                        _context.Comment.Remove(comment);
                    }

                }
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search(string name, int price, ItemType itemType)
        {
            
            var itemsResults = from item in _context.Item
                                where  (item.Price <= price) && item.Name.Contains(name) && (item.Type == itemType)
                                //orderby item.Name
                                select item;
            
            return View("Index", await itemsResults.ToListAsync());
        }
    }
}