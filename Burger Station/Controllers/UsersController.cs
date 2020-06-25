using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Burger_Station.Data;
using Burger_Station.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TestShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly Burger_StationContext _context;

        public UsersController(Burger_StationContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            string user = HttpContext.Session.GetString("Type");
            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }

            return View(await _context.User
                .Include(c=>c.FavoriteItem).ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");
            int userId = (int)HttpContext.Session.GetInt32("Id");

            if (userId == id || type == "Admin")
            {
                var user = await _context.User
                .Include(u => u.FavoriteItem)
                .FirstOrDefaultAsync(m => m.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                if (user.FavoriteItem == null)
                {
                    user.FavoriteItem = _context.Item.First();
                }

                ViewBag.FavoriteItem = user.FavoriteItem.Name;

                return View(user);

            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Items = new SelectList(await _context.Item.ToListAsync(), "Id", "Name");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,FirstName,LastName,Email,Password,Birthday")] User user, int ItemId)
        {
            if (ModelState.IsValid)
            {
                user.FavoriteItem = await _context.Item.FirstOrDefaultAsync(i => (i.Id == ItemId));

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");
            int userId = (int)HttpContext.Session.GetInt32("Id");

            if (userId == id || type == "Admin")
            {
                var user = await _context.User.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                ViewBag.Items = new SelectList(await _context.Item.ToListAsync(), "Id", "Name");

                return View(user);

            }

            return RedirectToAction("Index" , "Home");


        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,FirstName,LastName,Email,Password,Birthday")] User user, int itemId)
        {

            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    user.FavoriteItem = await _context.Item.FirstOrDefaultAsync(i => (i.Id == itemId));
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
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

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        // GET: Users/Signup
        public IActionResult Signup()
        {
            return View();
        }

        // POST: Users/Signup
        [HttpPost]
        public async Task<IActionResult> Signup(string firstName, string lastName, string email, string password, DateTime birthday)
        {
            if (firstName.Length > 50 || firstName.Length < 2) { return RedirectToAction("Signup", "Users"); }

            Regex regex = new Regex(@"^[a-z]+$");
            Match match = regex.Match(firstName.ToLower());

            if (!match.Success) { return RedirectToAction("Signup", "Users"); }

            match = regex.Match(lastName.ToLower());
            if (!match.Success) { return RedirectToAction("Signup", "Users"); }

            if (new System.Net.Mail.MailAddress(email).Address != email) { return RedirectToAction("Signup", "Users"); }

            User user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password,
                Birthday = birthday,
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            SignIn(user);
            return RedirectToAction("Index", "Home");
        }

        // Signin user and starts the session.
        private void SignIn(User user)
        {
            HttpContext.Session.SetString("Type", user.Type.ToString());
            HttpContext.Session.SetString("FullName", user.FirstName + " " + user.LastName );
            HttpContext.Session.SetInt32("Id", user.Id);

        }

        // GET: Users/Login
        public IActionResult Login()
        {
            string user = HttpContext.Session.GetString("Type");

            if (user != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Users/Login
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                SignIn(user);
                return RedirectToAction("Index", "Home");
            }
            return View("Index", "Users");
        }
    }
}
