using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Proiect_MDS.Models;
using System.Web;

namespace Proiect_MDS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public bool VerifyRole()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            if (userRole != "Researcher" && userRole != "Admin" && userRole != "User")
                return false;
            return true;
        }


        public IActionResult Index()
        {
            var email = HttpContext.Session.GetString("UserEmail");
            if (!string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Welcome back, " + email + "!";
            }
            else
            {
                ViewBag.Message = "You are not logged in.";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult Map()
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");
            var stations = _context.Stations.ToList();
            return View(stations);
        }

        public IActionResult Statistics()
        {
            if (VerifyRole() == false)
                return RedirectToAction("AccessDenied", "Home");

            var stats = new StationStatisticsViewModel
            {
                TotalStations = _context.Stations.Count(),
                AverageCongestion = Math.Round(_context.Stations.Average(s => s.CongestionLevel), 2),
                Distribution = _context.Stations
                    .GroupBy(s => s.CongestionLevel)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(stats);
        }
    }
}
