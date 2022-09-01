using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Multi_User.Models;

namespace Multi_User.Data
{
    public class DataBaseContext:IdentityDbContext<ApiUser>
    {
        public DataBaseContext(DbContextOptions options):base (options)
        {

        }
        public DbSet<ApiUser> ApiUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
