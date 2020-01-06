using DataAccess.Data.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreletzNoticeBoard.Areas.Admin.Components
{
    /// <summary>
    /// Модель представления для создания и редактирования объявления.
    /// </summary>
    public class NoticeViewModel
    {
        /// <summary>
        /// Объявление.
        /// </summary>
        public Notice Notice { get; set; }
        /// <summary>
        /// Категории
        /// </summary>
        public IEnumerable<Category> Categories { get; set; }
        /// <summary>
        /// Пользователи.
        /// </summary>
        public IEnumerable<IdentityUser> Users { get; set; }
        /// <summary>
        /// Id категории.
        /// </summary>
        public int CategoryId { get; set; }
        /// <summary>
        /// Id Автора.
        /// </summary>
        public string CreatorId { get; set; }
    }
}
