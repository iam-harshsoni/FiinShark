using api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    { 
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
       // public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Stock> Stocks { get; set; } 
        public DbSet<Comment> Comments { get; set; }
    }
}
