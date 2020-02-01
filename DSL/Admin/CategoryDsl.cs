using DataAccess.Data;
using DataAccess.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DSL.Admin
{
    /// <summary>
    /// Бзнес-логика для категорий (админпанель).
    /// </summary>
    public class CategoryDsl : DslObject
    {
        public CategoryDsl(ApplicationDbContext context) : base(context)
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
    }
}
