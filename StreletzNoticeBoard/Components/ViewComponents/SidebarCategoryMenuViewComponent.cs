using DataAccess.Data;
using DataAccess.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreletzNoticeBoard.Components.ViewComponents
{
    /// <summary>
    /// Виджет меню категорий для сайдбара.
    /// </summary>
    public class SidebarCategoryMenuViewComponent : ViewComponent
    {
        ApplicationDbContext _context;

        public SidebarCategoryMenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            IEnumerable<Category> categoriesList = _context.Categories.OrderBy(x => x.CategoryName);
            return View("Default", categoriesList);
        }
    }
}
