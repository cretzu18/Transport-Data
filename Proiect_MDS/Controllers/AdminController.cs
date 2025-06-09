using Microsoft.AspNetCore.Mvc;
using Proiect_MDS.Models;
using System.Text;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace Proiect_MDS.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool VerifyRole()
        {
            var userRole = HttpContext.Session.GetString("UserRole");

            if (userRole != "Admin")
                return false;
            return true;
        }

        public ActionResult Index()


        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");

            var utilizatori = _context.Utilizatori
                .Where(u => u.Rol != RolUtilizator.Admin)
                .ToList();
            return View(utilizatori);
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            var user = _context.Utilizatori.Find(id);
            if (user == null)
                return NotFound();

            var model = new UtilizatorEditViewModel
            {
                Id = user.Id,
                Nume = user.Nume,
                Email = user.Email,
                Rol = user.Rol
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UtilizatorEditViewModel model)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            if (!ModelState.IsValid)
                return View(model);

            var userInDb = _context.Utilizatori.Find(model.Id);
            if (userInDb == null)
                return NotFound();

            userInDb.Nume = model.Nume;
            userInDb.Email = model.Email;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            if (id == null)
            {
                return BadRequest();
            }

            var utilizator = _context.Utilizatori.Find(id);
            if (utilizator == null || utilizator.Rol == RolUtilizator.Admin)
            {
                return NotFound();
            }

            return View(utilizator);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            var utilizator = _context.Utilizatori.Find(id);
            if (utilizator == null || utilizator.Rol == RolUtilizator.Admin)
            {
                return NotFound();
            }

            _context.Utilizatori.Remove(utilizator);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
