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
        private UserManager<IdentityUser> _userManager;
        private readonly NoticesManager _noticesManager;
        private readonly CategoryManager _categoryManager;
        private readonly SiteUserManager _siteUserManager;

        public NoticesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {            
            _userManager = userManager;
            _noticesManager = new NoticesManager(context);
            _categoryManager = new CategoryManager(context);
            _siteUserManager = new SiteUserManager(context);
        }

        // GET: Notices
        public async Task<IActionResult> Index(int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            int noticesCount = _noticesManager.CountByUser(user);
            IEnumerable<Notice> noticesList;
            noticesList = await _noticesManager.FindByUser(page, user).ConfigureAwait(false);
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
                    Creator = user
                },
                Categories = _categoryManager.FindAll().Result,
                Users = _siteUserManager.findAll()
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
                Categories = _categoryManager.FindAll().Result,
                Users = _siteUserManager.findAll()
            };
            if (ModelState.IsValid)
            {
                if (viewModel.Notice != null)
                {
                    var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
                    Notice notice = viewModel.Notice;
                    notice.Category = _categoryManager.FindById(viewModel.CategoryId);
                    await _noticesManager.Add(notice, user);
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
            viewModel.Users = _siteUserManager.findAll();
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
                    await _noticesManager.Update(notice, user);
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
                Categories = _categoryManager.FindAll().Result,
                Users = _siteUserManager.findAll()
            };
            return View(viewModelClear);
        }





        public async Task<IActionResult> Search(string searchString, int page = 1)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User).ConfigureAwait(false);
            //int noticesCount = _context.Notices.Where(x => x.Creator.Id == user.Id).Count();
            IEnumerable<Notice> noticesList = _noticesManager.Search(searchString, page, user);
            int noticesCount = noticesList.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            ViewData["Search"] = searchString;
            return View(noticesList);
        }


    }
}
