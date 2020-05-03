using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.Admin
{
    /// <summary>
    /// Бзнес-логика для категорий (админпанель).
    /// </summary>
    public class CategoryAdminManager : DslObject
    {
        public CategoryAdminManager(ApplicationDbContext context) : base(context)
        {
        }
        /// <summary>
        /// Добавление новой категории.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <returns></returns>
        public async Task AddCategory(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Обновление категории.
        /// </summary>
        /// <param name="category">Категория.</param>
        /// <returns></returns>
        public async Task UpdateCategory(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление категории.
        /// </summary>
        /// <param name="id">Id категории.</param>
        /// <returns></returns>
        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Category>> Search(string search, int page)
        {
            IEnumerable<Category> categoryList;
            if (page < 1)
            {
                categoryList = await _context.Categories
                    .OrderBy(x => x.CategoryName).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                categoryList = await _context.Categories
                    .Where(x => x.CategoryName.Contains(search))
                    .Skip((page - 1) * 20).Take(20)
                    .OrderBy(x => x.CategoryName).ToListAsync().ConfigureAwait(false);
            }

            return categoryList;
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }


        public async Task<Category> FindById(int? id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
        }

        public int Count()
        {
            return _context.Categories.Count();
        }

        public async Task<IEnumerable<Category>> FindPerPage(int page)
        {
            IEnumerable<Category> categoryList;
            if (page < 1)
            {
                categoryList = await _context.Categories.OrderBy(x => x.CategoryName).ToListAsync().ConfigureAwait(false);
            }
            else
            {
                categoryList = await _context.Categories
                    .Skip((page - 1) * 20).Take(20)
                    .OrderBy(x => x.CategoryName).ToListAsync().ConfigureAwait(false);
            }
            return categoryList;
        }
    }
}
