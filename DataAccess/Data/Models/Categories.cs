using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Data.Models
{
    public class Category
    {
        [Key]       
        public int Id { get; set; }
        [Required]
        public string CategoryName { get; set; }

    }
}
