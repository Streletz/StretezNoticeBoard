using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Data.Models
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// Длина описания объявлений в списках
        /// </summary>
        [Required]
        [Display(Name = "Длина описания объявления в списках")]
        public int DescriptionLengthInList { get; set; }
    }
}
