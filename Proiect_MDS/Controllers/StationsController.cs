using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proiect_MDS.Models;
using System.Threading.Tasks;

namespace Proiect_MDS.Controllers
{
    public class StationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool VerifyRole()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Researcher")
                return false;
            return true;
        }

        // GET: Stations
        public async Task<IActionResult> Index()
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");

            var stations = await _context.Stations.ToListAsync();
            return View(stations);
        }

        // GET: Stations/Create
        public IActionResult Create()
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            return View();
        }

        // POST: Stations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Station station)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            if (ModelState.IsValid)
            {
                _context.Add(station);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(station);
        }

        // GET: Stations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            if (id == null)
                return NotFound();

            var station = await _context.Stations.FindAsync(id);
            if (station == null)
                return NotFound();

            return View(station);
        }

        // POST: Stations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Station station)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            if (id != station.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(station);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StationExists(station.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    var key = entry.Key;
                    var errors = entry.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Model error in '{key}': {error.ErrorMessage}");
                    }
                }
            }
            return View(station);
        }

        // GET: Stations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            if (id == null)
                return NotFound();

            var station = await _context.Stations.FirstOrDefaultAsync(s => s.Id == id);
            if (station == null)
                return NotFound();

            return View(station);
        }

        // POST: Stations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            var station = await _context.Stations.FindAsync(id);
            if (station != null)
            {
                _context.Stations.Remove(station);
                await _context.SaveChangesAsync();
            }
            else
                return NotFound();

            return RedirectToAction("Index");
        }

        private bool StationExists(int id)
        {
            return _context.Stations.Any(e => e.Id == id);
        }
    }
}
