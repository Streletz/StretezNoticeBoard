using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using StreletzNoticeBoard.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreletzNoticeBoard.Areas.Admin.Components.ViewComponents
{
    /// <summary>
    /// Компонент виджета статистики для админпанели.
    /// </summary>
    public class StatisticViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public StatisticViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(int maxPriority, bool isDone)
        {
            Statistic model = new Statistic
            {
                AllUsersCount = _context.Users.Count(),
                Admins = 0,
                Buyers = 0,
                Sellers = 0,
                Notices = _context.Notices.Count()
            };
            return View("Default",model);
        }
    }
}
