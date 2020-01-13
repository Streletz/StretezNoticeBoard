﻿using System;
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
        public async Task<IActionResult> Index()
        {
            return View(await _context.Notices.ToListAsync());
        }

        // GET: Notices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.Include(x => x.Category).Include(x => x.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
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
                    Notice notice = viewModel.Notice;
                    notice.Id = Guid.NewGuid();
                    notice.Category = _context.Categories.First(x => x.Id == viewModel.CategoryId);
                    //notice.Creator = _context.Users.First(x => x.Id == viewModel.CreatorId);
                    notice.CreatedAt = DateTime.Now;
                    _context.Add(notice);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index","Home");
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

            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }

        // POST: Notices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Subject,Description,CreatedAt,IsActive")] Notice notice)
        {
            if (id != notice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(notice.Id))
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
            return View(notice);
        }

        // GET: Notices/Delete/5
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

        // POST: Notices/Delete/5
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
