using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Data.Models
{
    /// <summary>
    /// Объявление
    /// </summary>
    public class Notice
    {
        /// <summary>
        /// ID.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        /// <summary>
        /// Тема (название).
        /// </summary>
        [Required]
        [Display(Name = "Название")]
        public string Subject { get; set; }
        /// <summary>
        /// Текст объявления.
        /// </summary>       
        [Required]
        [Display(Name = "Текст объявления")]
        public string Description { get; set; }
        /// <summary>
        /// Категория.
        /// </summary>
       // [Required]
        [Display(Name = "Категория")]
        public Category Category { get; set; }
        /// <summary>
        /// Пользователь, создавший объявление.
        /// </summary>
       // [Required]
        [Display(Name = "Разместил")]
        public IdentityUser Creator { get; set; }
        /// <summary>
        /// Дата создания.
        /// </summary>
        [Required]
        [Display(Name = "Создано")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        /// <summary>
        /// Объявление активно (т.е. отображается в публичном доступе).
        /// </summary>
        [Required]
        [Display(Name = "Активно")]
        public bool IsActive { get; set; } = true;
    }
}
