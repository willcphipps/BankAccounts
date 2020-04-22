using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BankAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers {
    public class HomeController : Controller {
        private MyContext _context { get; set; }

        public HomeController (MyContext context) {
            _context = context;
        }

        [HttpGet ("")]
        public IActionResult Index () {
            return View ();
        }

        [HttpPost ("Register")]
        public IActionResult Register (User user) {
            if (ModelState.IsValid) {
                if (_context.Users.Any (u => u.Email == user.Email)) {
                    ModelState.AddModelError ("Email", "Email already in use!");
                    return View ("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User> ();
                    user.Password = Hasher.HashPassword (user, user.Password);
                    _context.Users.Add (user);
                    _context.SaveChanges ();
                    HttpContext.Session.SetInt32 ("userid", user.UserId);
                    return Redirect ($"/account/{user.UserId}");
                }
            }
            return View ("Index");
        }

        [HttpPost ("Login")]
        public IActionResult Login (Login userLogin) {
            if (ModelState.IsValid) {
                var userInDb = _context.Users.FirstOrDefault (u => u.Email == userLogin.LoginEmail);
                if (userInDb == null) {
                    ModelState.AddModelError ("LoginEmail", "Invalid Email/Password");
                    return View ("Index");
                } else {
                    var hasher = new PasswordHasher<Login> ();
                    var result = hasher.VerifyHashedPassword (userLogin, userInDb.Password, userLogin.LoginPassword);
                    if (result == 0) {
                        ModelState.AddModelError ("LoginEmail", "Invalid Email/Password");
                        return View ("Index");
                    } else {
                        HttpContext.Session.SetInt32 ("userid", userInDb.UserId);
                        return Redirect ($"/account/{userInDb.UserId}");
                    }
                }
            } else {
                return View ("Index");
            }
        }

        [HttpGet ("account/{UserId}")]
        public IActionResult Account (int userid) {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) {
                return RedirectToAction ("Index");
            }
            ViewBag.UserAccount = _context.Users
                .FirstOrDefault (u => u.UserId == (int) IdinSession);
            ViewBag.AccountInfo = _context.BankAccounts
                .Include (u => u.Client)
                .Where (i => i.UserId == (int) IdinSession);
            return View ();
        }

        [HttpPost ("Trans")]
        public IActionResult Trans (BankAccount newtrans) {
            int? IdinSession = HttpContext.Session.GetInt32 ("userid");
            if (IdinSession == null) {
                return Redirect("/");
            }
            if (ModelState.IsValid) {
                _context.BankAccounts.Add (newtrans);
                newtrans.UserId = (int) IdinSession;
                _context.SaveChanges ();
                return Redirect ($"/account/{IdinSession}");
            }
                ViewBag.UserAccount = _context.Users.FirstOrDefault (u => u.UserId == (int) IdinSession);
                ViewBag.AccountInfo = _context.BankAccounts
                .Include (u => u.Client)
                .Where (i => i.UserId == (int) IdinSession);
                return View ("Account");
        }

    [HttpGet ("Logout")]
    public IActionResult Logout () {
        HttpContext.Session.Clear ();
        return Redirect ("Index");
    }
}
}