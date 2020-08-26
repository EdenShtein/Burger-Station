using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Burger_Station.Data;
using Burger_Station.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Burger_Station.Controllers
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
            ViewBag.FullName = HttpContext.Session.GetString("FullName");
            ViewBag.userType = user;

            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }

            if (user == "Member")
            {
                int userId = (int)HttpContext.Session.GetInt32("Id");
                
                return RedirectToAction("Details", "Users", new { @id = userId });
            }

            return View(await _context.User
                .Include(c=>c.FavoriteItem)
                .ToListAsync());
        }


        // GET: Users/Details/
        public async Task<IActionResult> Details()
        {
            string type = HttpContext.Session.GetString("Type");
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (type == null)
            {
                return RedirectToAction("Login", "Users");
            }

            int userId = (int)HttpContext.Session.GetInt32("Id");
            var user = await _context.User
               .Include(u => u.FavoriteItem)
               .FirstOrDefaultAsync(i=> i.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (type == "Member")
            {
                return RedirectToAction("DetailsMember", "Users", new { @id = userId });
            }

            return RedirectToAction("DetailsAdmin", "Users", new { @id = userId });
        }


        // GET: Users/DetailsMember/5
        public async Task<IActionResult> DetailsMember(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");
            int userId = (int)HttpContext.Session.GetInt32("Id");

            if(userId != id && type != "Admin")
            {
                return RedirectToAction("DetailsMember", "Users", new { @id = userId });
            }

            var user = await _context.User
               .Include(u => u.FavoriteItem)
               .FirstOrDefaultAsync(m => m.Id == id);
            
            if (user == null)
            {
                return NotFound();
            }

            if (user.FavoriteItem == null)
            {
                ViewBag.FavoriteItem = " ";
            }
            else
            {
                ViewBag.FavoriteItem = user.FavoriteItem.Name;
            }

            return View(user);
        }


        public async Task<IActionResult> DetailsAdmin(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");
            int userId = (int)HttpContext.Session.GetInt32("Id");

            if(type == "Member")
            {
                return RedirectToAction("DetailsMember", "Users", new { @id = userId });
            }

            var user = await _context.User
               .Include(u => u.FavoriteItem)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.FavoriteItem == null)
            {
                ViewBag.FavoriteItem = " ";
            }
            else
            {
                ViewBag.FavoriteItem = user.FavoriteItem.Name;
            }

            //----------- Count variables for D3------------//

            var users = await _context.User
                .ToListAsync();
            
            var usersType = users.GroupBy(u => u.Type).OrderBy(g => g.Key).Select(g => Tuple.Create(g.Key, g.Count()));
            ViewBag.userAdmin = usersType.ElementAt(1).Item2;

            ViewBag.userMember = usersType.ElementAt(0).Item2;
            return View(user);
        }

        // GET: Users/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Items = new SelectList(await _context.Item
                .Where(i => i.Type == ItemType.Food)
                .ToListAsync(), "Id", "Name");

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,FirstName,LastName,Email,Password,Birthday")] User user, int itemId)
        {
            if (ModelState.IsValid)
            {
                user.FavoriteItem = await _context.Item.FirstOrDefaultAsync(i => (i.Id == itemId));

                var item = await _context.Item
                    .Include(x => x.SatisfiedUsers)
                    .FirstOrDefaultAsync(x => x.Id == itemId);
                
                item.SatisfiedUsers.Add(user);

                _context.Item.Update(item);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.userType = HttpContext.Session.GetString("Type");

            if (id == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");
            int userId = (int)HttpContext.Session.GetInt32("Id");

            ViewBag.userType = type;

            if (userId == id || type == "Admin")
            {
                var user = await _context.User.FindAsync(id);
            
                if (user == null)
                {
                    return NotFound();
                }

                ViewBag.Items = new SelectList(await _context.Item
                    .Where(i => i.Type == ItemType.Food)
                    .ToListAsync(), "Id", "Name");

                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,FirstName,LastName,Email,Password,Birthday,FavoriteItem.Id")] User user, int itemId)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(user.FavoriteItem == null)
                    {
                        user.FavoriteItem = await _context.Item
                            .FirstOrDefaultAsync(i => (i.Id == itemId));

                        var item = await _context.Item
                            .Include(x => x.SatisfiedUsers)
                            .FirstOrDefaultAsync(i => (i.Id == itemId));

                        if (item != null)
                        {
                            item.SatisfiedUsers.Add(user);
                            _context.Item.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if (user.FavoriteItem.Id != itemId)
                        {
                            var item = await _context.Item
                                .Include(x => x.SatisfiedUsers)
                                .FirstOrDefaultAsync(i => (i.Id == user.FavoriteItem.Id));

                            item.SatisfiedUsers.Remove(user);

                            user.FavoriteItem = await _context.Item
                                .FirstOrDefaultAsync(i => (i.Id == itemId));

                            item = await _context.Item
                                .Include(x => x.SatisfiedUsers)
                                .FirstOrDefaultAsync(i => (i.Id == itemId));

                            item.SatisfiedUsers.Add(user);

                            _context.Item.Update(item);
                            await _context.SaveChangesAsync();
                        }
                    }

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

                return RedirectToAction(nameof(user.Id));
            }

            return View(user);
        }

        // GET: Users/Delete/5
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
            var user = await _context.User
                .Include(u => u.FavoriteItem)
                .FirstOrDefaultAsync(u => u.Id == id);

            if(user.FavoriteItem != null)
            {
                var item = await _context.Item
                .Include(x => x.SatisfiedUsers)
                .FirstOrDefaultAsync(i => (i.Id == user.FavoriteItem.Id));

                item.SatisfiedUsers.Remove(user);
                _context.Item.Update(item);
            }

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
            if(firstName == null || lastName == null|| email == null|| password == null|| birthday == null)
            {
                return View("Signup", "Users");
            }
            
            if (firstName.Length > 50 || firstName.Length < 2) 
            { 
                return RedirectToAction("Signup", "Users"); 
            }

            Regex regex = new Regex(@"^[a-z]+$");
            Match match = regex.Match(firstName.ToLower());

            if (!match.Success) 
            { 
                return RedirectToAction("Signup", "Users"); 
            }

            match = regex.Match(lastName.ToLower());
            if (!match.Success) { return RedirectToAction("Signup", "Users"); }

            if (new System.Net.Mail.MailAddress(email).Address != email) 
            {
                return RedirectToAction("Signup", "Users"); 
            }

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

            return View("Login", "Users");
        }

        // Facebook Login, no password needed
        [HttpPost]
        public IActionResult FacebookLogin(string data)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == data);

            if (user != null)
            {
                SignIn(user);
                return Content("Success");
            }

            return Content("Wrong");
        }

        public IActionResult Logout()
        {
            string user = HttpContext.Session.GetString("Type");

            if (user != null)
            {
                HttpContext.Session.Clear();
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
