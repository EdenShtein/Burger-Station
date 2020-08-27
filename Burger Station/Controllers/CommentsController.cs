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
    public class CommentsController : Controller
    {
        private readonly Burger_StationContext _context;
    
        public CommentsController(Burger_StationContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            ViewBag.FullName = HttpContext.Session.GetString("FullName");
            ViewBag.userType = HttpContext.Session.GetString("Type");

            ViewBag.joinListCounter = (from i in _context.Item
                                join c in _context.Comment on i.Id equals c.Item.Id
                                orderby c.PostDate descending
                                select new ItemComment
                                {
                                    CommentId = c.Id,
                                    ItemName = i.Name,
                                    PostTitle = c.PostTitle,
                                    PostedBy = c.PostedBy,
                                    PostBody = c.PostBody,
                                    PostDate = c.PostDate
                                }
                                ).Count();

            return View(await _context
                .Comment
                .Include(c => c.Item)
                .ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment
                .Include(c => c.Item)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (comment == null)
            {
                return NotFound();
            }

            ViewBag.Item = comment.Item.Name;

            return View(comment);
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");
            string type = HttpContext.Session.GetString("Type");

            if (type == null)
            {
                return RedirectToAction("index", "Comments");
            }

            ViewBag.Items = new SelectList(await _context.Item
                .Where(i => i.Type == ItemType.Food)
                .ToListAsync(), "Id", "Name");

            ViewBag.userName = HttpContext.Session.GetString("FullName");
            
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PostTitle,PostBody,PostDate,PostedBy")] Comment comment, int itemId)
        {
            if (ModelState.IsValid)
            {
                comment.PostDate = DateTime.Now;
                comment.Item = await _context.Item.FirstOrDefaultAsync(i => (i.Id == itemId));
                comment.PostedBy = HttpContext.Session.GetString("FullName");

                var item = await _context.Item
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Id == itemId);

                item.Comments.Add(comment);

                _context.Item.Update(item);
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            var comment = await _context.Comment.FindAsync(id);
            
            if (comment == null)
            {
                return NotFound();
            }
            
            string userName = HttpContext.Session.GetString("FullName");

            if (userName == null)
            {
                return RedirectToAction("Index", "Comments");
            }

            else if (type == "Admin")
            {

                ViewBag.Items = new SelectList(await _context.Item
                    .Where(i => i.Type == ItemType.Food)
                    .ToListAsync(), "Id", "Name");

                ViewBag.PostDate = comment.PostDate;

                return View(comment);
            }

            return RedirectToAction("Index", "Comments");
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostTitle,PostBody,PostDate,PostedBy")] Comment comment, int itemId)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    comment.Item = await _context.Item.FirstOrDefaultAsync(i => (i.Id == itemId));
                    comment.PostedBy = HttpContext.Session.GetString("FullName");

                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }
            string type = HttpContext.Session.GetString("Type");

            var comment = await _context.Comment
                .FirstOrDefaultAsync(m => m.Id == id);

            if (comment == null)
            {
                return NotFound();
            }
            
            string userName = HttpContext.Session.GetString("FullName");

            if(userName == null)
            {
                return RedirectToAction("Index", "Comments");
            }
            
            else if (userName.Equals(comment.PostedBy) || type == "Admin")
            {
                return View(comment);
            }

            return RedirectToAction("Index", "Comments");
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comment.FindAsync(id);
            
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search(string postTitle, string postBy, string PostItem)
        {

            var itemsResults = from comment in _context.Comment
                               where comment.PostTitle.Contains(postTitle) && comment.PostedBy.Contains(postBy) && comment.Item.Name.Contains(PostItem)
                               orderby comment.PostDate descending
                               select comment;


            return View("Index", await itemsResults.Include(c => c.Item).ToListAsync());
        }

    }
}
