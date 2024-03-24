using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MyBlogger.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Post> Post { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Menu> Menu { get; set; }
    }
}
