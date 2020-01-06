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

namespace StreletzNoticeBoard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoticesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Notices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notices.OrderBy(x => x.CreatedAt).ToListAsync());
        }

        // GET: Admin/Notices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
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
                    Notice notice = viewModel.Notice;
                    notice.Id = Guid.NewGuid();
                    notice.Category = _context.Categories.First(x => x.Id == viewModel.CategoryId);
                    notice.Creator = _context.Users.First(x => x.Id == viewModel.CreatorId);
                    _context.Add(notice);
                    await _context.SaveChangesAsync();
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

            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }
            NoticeViewModel viewModel = new NoticeViewModel
            {
                Notice = notice,
                Categories = _context.Categories.OrderBy(x => x.CategoryName),
                Users = _context.Users.OrderBy(x => x.UserName)
            };
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
                    notice.Category = _context.Categories.First(x => x.Id == viewModel.CategoryId);
                    notice.Creator = _context.Users.First(x => x.Id == viewModel.CreatorId);
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
                return RedirectToAction(nameof(Index));
            }
            NoticeViewModel viewModelClear = new NoticeViewModel
            {
                Notice = new Notice(),
                Categories = _context.Categories.OrderBy(x => x.CategoryName),
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

            var notice = await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var notice = await _context.Notices.FindAsync(id);
            _context.Notices.Remove(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticeExists(Guid id)
        {
            return _context.Notices.Any(e => e.Id == id);
        }
    }
}
