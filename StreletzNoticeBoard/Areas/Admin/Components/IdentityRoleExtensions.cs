using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StreletzNoticeBoard.Areas.Admin.Components
{
    /// <summary>
    /// Расширения для объектов ролей.
    /// </summary>
    public static class IdentityRoleExtensions
    {
        /// <summary>
        /// Понятные имена для ролей
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public static string TextName(this IdentityRole role)
        {
            string result;
            switch (role.Name)
            {
                case "admin": result = "Администратор"; break;
                case "buyer": result = "Покупатель"; break;
                case "seller": result = "Продавец"; break;
                default: result = "Не установлена"; break;
            }
            return result;
        }
    }
}
