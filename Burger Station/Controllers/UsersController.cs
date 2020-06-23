using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Burger_Station.Data;
using Burger_Station.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

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
            if (user == null)
            {
                return RedirectToAction("Login", "Users");
            }

            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Password,Birthday,PhoneNumber")] User user)
        {
            if (ModelState.IsValid)
            {
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

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Password,Birthday,PhoneNumber,Type")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            string type = HttpContext.Session.GetString("Type");

            if (type != "Admin")
            {
                return RedirectToAction("Index", "Home");
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

        // Returns TRUE if user exists in the DB.
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
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
            return View();
        }

        // Signin user and starts the session.
        private void SignIn(User user)
        {
            HttpContext.Session.SetString("Type", user.Type.ToString());
        }

        // GET: Users/Signup
        public IActionResult Signup()
        {
            return View();
        }

        // POST: Users/Signup
        [HttpPost]
        public async Task<IActionResult> Signup(string firstname, string lastname, string email, string password, DateTime birthday, string phoneNumber)
        {
            if (firstname.Length > 50 || firstname.Length < 2) { return RedirectToAction("Signup", "Users"); }

            Regex regex = new Regex(@"^[a-z]+$");
            Match match = regex.Match(firstname.ToLower());

            if(!match.Success) { return RedirectToAction("Signup", "Users"); }

            if (lastname.Length > 50 || lastname.Length < 2) { return RedirectToAction("Signup", "Users"); }

            match = regex.Match(lastname.ToLower());
            if (!match.Success) { return RedirectToAction("Signup", "Users"); }

            if (new System.Net.Mail.MailAddress(email).Address != email) { return RedirectToAction("Signup", "Users"); }

            if (password.Length > 10 || password.Length < 5) { return RedirectToAction("Signup", "Users"); }

            regex = new Regex(@"(^[0-9]{10}$)|(^\+[0-9]{2}\s+[0-9]{ 2}[0 - 9]{ 8}$)| (^[0 - 9]{ 3}-[0 - 9]{ 4}-[0 - 9]{ 4}$)");
            // Class Regex Repesents an 
            // immutable regular expression. 
            //   Format                Pattern 
            // xxxxxxxxxx           ^[0 - 9]{ 10}$ 
            // +xx xx xxxxxxxx     ^\+[0 - 9]{ 2}\s +[0 - 9]{ 2}\s +[0 - 9]{ 8}$ 
            // xxx - xxxx - xxxx   ^[0 - 9]{ 3} -[0 - 9]{ 4}-[0 - 9]{ 4

            if (!regex.IsMatch(phoneNumber)) { return RedirectToAction("Signup", "Users"); }

            User user = new User()
            {
                FirstName = firstname,
                LastName = lastname,
                Email = email,
                Password = password,
                Birthday = birthday,
                PhoneNumber = phoneNumber
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            SignIn(user);
            return RedirectToAction("Index", "Home");
        }
    }
}
