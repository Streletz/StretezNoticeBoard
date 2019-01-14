using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Data.Models
{
    /// <summary>
    /// Категории объявлений.
    /// </summary>
    public class Category
    {
        /// <summary>
        /// ID.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public int Id { get; set; }
        /// <summary>
        /// Название категории.
        /// </summary>
        [Required]
        [Display(Name ="Категория")]
        public string CategoryName { get; set; }
    }
}
