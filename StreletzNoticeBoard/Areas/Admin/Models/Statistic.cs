using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StreletzNoticeBoard.Areas.Admin.Models
{
    public class Statistic
    {

        [Display(Name = "Всего пользователей")]
        public int AllUsersCount { get; set; }
        [Display(Name = "Администраторов")]
        public int Admins { get; set; }
        [Display(Name = "Покупателей")]
        public int Buyers { get; set; }
        [Display(Name = "Продавцов")]
        public int Sellers { get; set; }
        [Display(Name = "Объявлений")]
        public int Notices { get; set; }
    }
}
