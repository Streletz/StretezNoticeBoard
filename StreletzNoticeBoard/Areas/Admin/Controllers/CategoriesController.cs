using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using DataAccess.Data.Models;
using DSL.Admin;

namespace StreletzNoticeBoard.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {       
        private readonly CategoryAdminManager _categoryDsl;

        public CategoriesController(ApplicationDbContext context)
        {            
            _categoryDsl = new CategoryAdminManager(context);
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index(int page = 1)
        {
            int categoriesCount = _categoryDsl.Count();
            ViewData["PageCount"] = categoriesCount <= 20 ? 1 : (categoriesCount / 20) + 1;
            IEnumerable<Category> categoryList = await _categoryDsl.FindPerPage(page).ConfigureAwait(false);
            return View(categoryList);
        }
        

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryDsl.FindById(id).ConfigureAwait(false);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _categoryDsl.AddCategory(category).ConfigureAwait(false);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }



        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _categoryDsl.FindById(id).ConfigureAwait(false);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _categoryDsl.UpdateCategory(category).ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_categoryDsl.CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await _categoryDsl.FindById(id).ConfigureAwait(false);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryDsl.DeleteCategory(id).ConfigureAwait(false);
            return RedirectToAction(nameof(Index));
        }



        
        public async Task<IActionResult> Search(string search, int page = 1)
        {
            IEnumerable<Category> categoryList;
            categoryList = await _categoryDsl.Search(search, page).ConfigureAwait(false);
            int noticesCount = categoryList.Count();
            ViewData["PageCount"] = noticesCount <= 20 ? 1 : (noticesCount / 20) + 1;
            ViewData["Search"] = search;
            return View(categoryList);
        }

        
    }
}
