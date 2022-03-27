using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.Entity;

namespace WebAPI.Context
{
	public class WebApiDbContext : IdentityDbContext
    {
        public WebApiDbContext(DbContextOptions<WebApiDbContext> options) : base(options) { }
        public DbSet<UserInfo> Users {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
