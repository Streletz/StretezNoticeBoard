using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.AspNetCore.Mvc;
using StreletzNoticeBoard.Models;

namespace StreletzNoticeBoard.Controllers
{
    /// <summary>
    /// Контроллер главной страницы.
    /// </summary>
    public class HomeController : Controller
    {
        private ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Notice> noticesList = _context.Notices.Where(x => x.IsActive).OrderByDescending(x => x.CreatedAt);
            return View(noticesList);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
    }
}
