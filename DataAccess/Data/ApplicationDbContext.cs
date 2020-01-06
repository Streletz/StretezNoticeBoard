using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    /// <summary>
    /// Контекст БД.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Категории объявлений.
        /// </summary>
        public DbSet<Category> Categories { get; set; }
        /// <summary>
        /// Объявления.
        /// </summary>
        public DbSet<Notice> Notices { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            /*modelBuilder.Entity<User>().HasData(
                new User[]
                {
                new User { Id=1, Name="Tom", Age=23},
                new User { Id=2, Name="Alice", Age=26},
                new User { Id=3, Name="Sam", Age=28}
                });*/
        }
    }
}
