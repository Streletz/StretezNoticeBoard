using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Data.Models
{
    public class Category
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]       
        public int Id { get; set; }
        [Required]
        [Display(Name ="Категория")]
        public string CategoryName { get; set; }
    }
}
