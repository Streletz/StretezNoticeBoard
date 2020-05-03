using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSL.Site
{
    public class CategoryManager : BaseManager
    {
        public CategoryManager(ApplicationDbContext context) : base(context)
        {
        }

        public async Task Update(Category category)
        {
            _context.Update(category);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }


        public bool CategoryExists(int? id)
        {
            Category category = FindById(id);
            return category != null;
        }

        public Category FindById(int? id)
        {
            return _context.Categories
                            .FirstOrDefaultAsync(m => m.Id == id).Result;
        }

        public async Task<List<Category>> FindAll()
        {
            return await _context.Categories.OrderBy(x => x.CategoryName).ToListAsync();
        }


        public async Task Add(Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();
        }


    }
}
