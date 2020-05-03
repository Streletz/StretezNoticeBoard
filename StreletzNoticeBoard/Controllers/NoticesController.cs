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
using DSL.Site;

namespace StreletzNoticeBoard.Controllers
{
    public class NoticesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<IdentityUser> _userManager;
        private readonly NoticesManager _noticesManager;
        private readonly CategoryManager _categoryManager;

        public NoticesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _noticesManager = new NoticesManager(context);
            _categoryManager = new CategoryManager(context);
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
            Notice notice = await _noticesManager.FindById(id);
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
                Categories = _categoryManager.FindAll().Result,
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
                    notice.Category = _categoryManager.FindById(viewModel.CategoryId);
                    notice.Creator = _context.Users.First(x => x.Id == user.Id);
                    notice.CreatedAt = DateTime.Now;
                    await _noticesManager.Add(notice);
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

            var notice = _noticesManager.FindById(id).Result;
            if (notice == null)
            {
                return NotFound();
            }
            NoticeViewModel viewModel = new NoticeViewModel();

            viewModel.Notice = notice;

            viewModel.Categories = _categoryManager.FindAll().Result;
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
                    notice.Category = _categoryManager.FindById(viewModel.CategoryId);
                    notice.Creator = _context.Users.First(x => x.Id == user.Id);
                    await _noticesManager.Update(notice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_noticesManager.NoticeExists(viewModel.Notice.Id))
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

        

        

        public async Task<IActionResult> Search(string searchString, int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            //int noticesCount = _context.Notices.Where(x => x.Creator.Id == user.Id).Count();
            IEnumerable<Notice>  noticesList =_noticesManager.Search(searchString, page, user);
            int noticesCount = noticesList.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            ViewData["Search"] = searchString;
            return View(noticesList);
        }

        
    }
}
