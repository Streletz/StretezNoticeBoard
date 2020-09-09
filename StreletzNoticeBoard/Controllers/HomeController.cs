using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Data.Models;
using DSL;
using DSL.Site;
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
        private readonly ISettingsManager settingsManager;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
            settingsManager = new SettingsManager(context);
        }
        public IActionResult Index(int page = 1)
        {
            IEnumerable<Notice> noticesList;
            int noticesCount = _context.Notices.Where(x => x.IsActive).Count();
            if (page < 1)
            {
                noticesList = _context.Notices.Where(x => x.IsActive).OrderByDescending(x => x.CreatedAt);
            }
            else
            {
                noticesList = _context.Notices.Where(x => x.IsActive).Skip((page - 1) * 20).Take(20).OrderByDescending(x => x.CreatedAt);
            }
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            ViewData["DescriptionLenght"] = settingsManager.GetSettings().Result.DescriptionLengthInList;
            return View(noticesList);
        }        

        public IActionResult Contact()
        {
            ViewData["Message"] = "Вопросы, замечания и предложения просьба направлять по следующему адресу.";

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

        public IActionResult Search(string search, int page = 1)
        {
            IEnumerable<Notice> noticesList;
            //int noticesCount = _context.Notices.Where(x => x.IsActive).Count();
            if (page < 1)
            {
                noticesList = _context.Notices
                    .Where(x =>
                    x.IsActive
                    &&
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    )
                    )
                    .OrderByDescending(x => x.CreatedAt);
            }
            else
            {
                noticesList = _context.Notices
                    .Where(x =>
                    x.IsActive
                    &&
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    )
                    )
                    .Skip((page - 1) * 20).Take(20).OrderByDescending(x => x.CreatedAt);
            }
            int noticesCount = noticesList.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            ViewData["DescriptionLenght"] = settingsManager.GetSettings().Result.DescriptionLengthInList;
            ViewData["Search"] = search;
            return View(noticesList);
        }
    }
}
