using DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreletzNoticeBoard.Areas.Admin.Components
{
    /// <summary>
    /// Модель представления для управления пользователями через админпанель
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Логин.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// Пароль (используется только при создании и редактировании).
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Роль.
        /// </summary>
        public string Role { get; set; }

        public string RoleName
        {
            get
            {
                string result;
                switch (Role)
                {
                    case "admin": result = "Администратор"; break;
                    case "buyer": result = "Покупатель"; break;
                    case "seller": result = "Продавец"; break;
                    default: result = "Не установлена";break;
                }
                return result;
            }
        }
        /// <summary>
        /// Роли для списков в UI.
        /// </summary>
        public IEnumerable<IdentityRole> Roles { get; set; }
        /// <summary>
        /// Конвертация IdentityUser в модель представления.
        /// </summary>
        /// <param name="rawUser">Объект IdentityUser.</param>
        /// <param name="context">Контекст БД.</param>
        /// <param name="userManager">Системный менеджер пользователей.</param>
        /// <returns></returns>
        public static UserViewModel Parse(IdentityUser rawUser, ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            UserViewModel result = new UserViewModel
            {
                Id = rawUser.Id,
                UserName = rawUser.UserName,
                Role = userManager.GetRolesAsync(rawUser).Result.First(),
                Roles = context.Roles
            };
            return result;
        }
    }
}
