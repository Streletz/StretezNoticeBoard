using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Data.Models;
using StreletzNoticeBoard.Components.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace StreletzNoticeBoard.Controllers
{
    public class NoticesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;

        public NoticesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notices
        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            int noticesCount = _context.Notices.Where(x => x.Creator.Id == user.Id).Count();
            IEnumerable<Notice> noticesList;
            if (page < 1)
            {
                noticesList = await _context.Notices.Include(x => x.Category)
                    .Where(x => x.Creator.Id == user.Id).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                noticesList = await _context.Notices.Include(x => x.Category)
                    .Where(x => x.Creator.Id == user.Id).Skip((page - 1) * 20).Take(20).ToListAsync().ConfigureAwait(false);
            }
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            return View(noticesList);
        }

        // GET: Notices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.Include(x => x.Category).Include(x => x.Creator)
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(true);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // GET: Admin/Notices/Create
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            NoticeViewModel viewModel = new NoticeViewModel
            {
                Notice = new Notice
                {
                    Category = new Category(),
                    Creator = _context.Users.First(x => x.Id == user.Id)
                },
                Categories = _context.Categories.OrderBy(x => x.CategoryName),
                Users = _context.Users.OrderBy(x => x.UserName)
            };
            return View(viewModel);
        }

        // POST: Admin/Notices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("Notice.Id,Notice.Subject,Notice.Description,Notice.CreatedAt,Notice.IsActive")]*/ NoticeViewModel viewModel)
        {
            NoticeViewModel noticeViewModel = new NoticeViewModel
            {
                Categories = _context.Categories.OrderBy(x => x.CategoryName),
                Users = _context.Users.OrderBy(x => x.UserName)
            };
            if (ModelState.IsValid)
            {
                if (viewModel.Notice != null)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
                    Notice notice = viewModel.Notice;
                    notice.Id = Guid.NewGuid();
                    notice.Category = _context.Categories.First(x => x.Id == viewModel.CategoryId);
                    notice.Creator = _context.Users.First(x => x.Id == user.Id);
                    notice.CreatedAt = DateTime.Now;
                    _context.Add(notice);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(noticeViewModel);
        }


        // GET: Notices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.Include(x => x.Category).Include(x => x.Creator).FirstAsync(x => x.Id == id).ConfigureAwait(true);
            if (notice == null)
            {
                return NotFound();
            }
            NoticeViewModel viewModel = new NoticeViewModel();

            viewModel.Notice = notice;

            viewModel.Categories = _context.Categories.OrderBy(x => x.CategoryName);
            viewModel.Users = _context.Users.OrderBy(x => x.UserName);
            viewModel.CategoryId = notice.Category.Id;
            viewModel.CreatorId = notice.Creator.Id;

            return View(viewModel);
        }

        // POST: Notices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NoticeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IdentityUser user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
                    Notice notice = viewModel.Notice;
                    notice.Category = _context.Categories.First(x => x.Id == viewModel.CategoryId);
                    notice.Creator = _context.Users.First(x => x.Id == user.Id);
                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(viewModel.Notice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            NoticeViewModel viewModelClear = new NoticeViewModel
            {
                Notice = new Notice(),
                Categories = _context.Categories.OrderBy(x => x.CategoryName),
                Users = _context.Users.OrderBy(x => x.UserName)
            };
            return View(viewModelClear);
        }


        private bool NoticeExists(Guid id)
        {
            return _context.Notices.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Search(string search, int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            //int noticesCount = _context.Notices.Where(x => x.Creator.Id == user.Id).Count();
            IEnumerable<Notice> noticesList;
            if (page < 1)
            {
                noticesList = await _context.Notices.Include(x => x.Category)
                    .Where(x => (x.Creator.Id == user.Id) &&
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    )).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                noticesList = await _context.Notices.Include(x => x.Category)
                    .Where(x => (x.Creator.Id == user.Id) &&
                    (
                    x.Subject.ToUpper().Contains(search)
                    || x.Description.ToUpper().Contains(search)
                    )).Skip((page - 1) * 20).Take(20).ToListAsync().ConfigureAwait(false);
            }
            int noticesCount = noticesList.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            ViewData["Search"] = search;
            return View(noticesList);
        }
    }
}
