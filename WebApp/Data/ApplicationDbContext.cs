using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApp.Data.Identity;

namespace WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppIdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Message>().Property(z => z.Id).UseIdentityColumn();
            builder.Entity<Message>().Property(z => z.SenderName).HasMaxLength(100);


            base.OnModelCreating(builder);
        }

        public DbSet<AppIdentityUser> AppIdentityUser { get; set; }

        public DbSet<Message> Messages { get; set; }

    }
}