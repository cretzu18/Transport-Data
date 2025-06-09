using Microsoft.AspNetCore.Mvc;
using Proiect_MDS.Models;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Proiect_MDS.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext db;

        public AccountController(ApplicationDbContext _db)
        {
            db = _db;
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (!Enum.TryParse<RolUtilizator>(model.Rol, out var rol)
    || (rol != RolUtilizator.User && rol != RolUtilizator.Researcher))
                {
                    ModelState.AddModelError("Rol", "Invalid type.");
                    return View(model);
                }

                if (db.Utilizatori.Any(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View(model);
                }

                var utilizator = new Utilizator
                {
                    Nume = model.Nume,
                    Email = model.Email,
                    Rol = rol,
                    ParolaHashed = HashParola(model.Parola)
                };

                db.Utilizatori.Add(utilizator);
                db.SaveChanges();

                HttpContext.Session.SetString("UserEmail", model.Email);
                HttpContext.Session.SetString("UserRole", utilizator.Rol.ToString());

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        private string HashParola(string parola)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(parola);
                var hash = sha256.ComputeHash(bytes);
                return System.Convert.ToBase64String(hash);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parolaHashed = HashParola(model.Parola);

                var utilizator = db.Utilizatori
                    .FirstOrDefault(u => u.Email == model.Email && u.ParolaHashed == parolaHashed);

                if (utilizator != null)
                {
                    HttpContext.Session.SetString("UserEmail", utilizator.Email);
                    HttpContext.Session.SetString("UserRole", utilizator.Rol.ToString());

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                }
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
