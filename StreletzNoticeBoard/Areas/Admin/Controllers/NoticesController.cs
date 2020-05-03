using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.AspNetCore.Identity;
using StreletzNoticeBoard.Areas.Admin.Components;
using DSL.Admin;

namespace StreletzNoticeBoard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CategoryAdminManager _categoryAdminManager;
        private readonly NoticesAdminManager _noticesAdminManager;

        public NoticesController(ApplicationDbContext context)
        {
            _context = context;
            _categoryAdminManager = new CategoryAdminManager(context);
            _noticesAdminManager = new NoticesAdminManager(context);
        }

        // GET: Admin/Notices
        public async Task<IActionResult> Index(int page = 1)
        {
            int noticesCount = _noticesAdminManager.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            IEnumerable<Notice> noticeList = await _noticesAdminManager.FindPerPage(page).ConfigureAwait(false);
            return View(noticeList);
        }

        

        // GET: Admin/Notices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _noticesAdminManager.FindById(id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // GET: Admin/Notices/Create
        public IActionResult Create()
        {
            NoticeViewModel viewModel = new NoticeViewModel
            {
                Notice = new Notice
                {
                    Category = new Category(),
                    Creator = new IdentityUser()
                },
                Categories = _categoryAdminManager.findAll(),
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
                Categories = _categoryAdminManager.findAll(),
                Users = _context.Users.OrderBy(x => x.UserName)
            };
            if (ModelState.IsValid)
            {
                if (viewModel.Notice != null)
                {
                    Notice notice = viewModel.Notice;
                    notice.Category = _categoryAdminManager.FindById(viewModel.CategoryId).Result;
                    notice.Creator = _context.Users.First(x => x.Id == viewModel.CreatorId);
                    await _noticesAdminManager.Add(notice);
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(noticeViewModel);
        }
        

        // GET: Admin/Notices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _noticesAdminManager.FindById(id);
            if (notice == null)
            {
                return NotFound();
            }
            NoticeViewModel viewModel = new NoticeViewModel();

            viewModel.Notice = notice;

            viewModel.Categories = _categoryAdminManager.findAll();
            viewModel.Users = _context.Users.OrderBy(x => x.UserName);
            viewModel.CategoryId = notice.Category.Id;
            viewModel.CreatorId = notice.Creator.Id;
            return View(viewModel);
        }

        

        // POST: Admin/Notices/Edit/5
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
                    Notice notice = viewModel.Notice;
                    notice.Category = _categoryAdminManager.FindById(viewModel.CategoryId).Result;
                    notice.Creator = _context.Users.First(x => x.Id == viewModel.CreatorId);
                    await _noticesAdminManager.Update(notice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_noticesAdminManager.NoticeExists(viewModel.Notice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            NoticeViewModel viewModelClear = new NoticeViewModel
            {
                Notice = new Notice(),
                Categories = _categoryAdminManager.findAll(),
                Users = _context.Users.OrderBy(x => x.UserName)
            };
            return View(viewModelClear);
        }
        

        // GET: Admin/Notices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Notice notice = await _noticesAdminManager.FindById(id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        

        // POST: Admin/Notices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var notice = await _noticesAdminManager.FindById(id);
            await _noticesAdminManager.Delete(notice);
            return RedirectToAction(nameof(Index));
        }

        

        

        public async Task<IActionResult> Search(string search, int page = 1)
        {
            ViewData["Search"] = search;
            IEnumerable<Notice> noticeList;
            noticeList = await _noticesAdminManager.Search(search, page).ConfigureAwait(false);
            int noticesCount = noticeList.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            return View(noticeList);
        }
        
    }
}
