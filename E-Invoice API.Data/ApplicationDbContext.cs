using E_Invoice_API.Data.EntityConfiguration;
using E_Invoice_API.Data.Models;
using E_Invoice_API.Data.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace E_Invoice_API.Data
{
    public class ApplicationDbContext : IdentityUserContext<User, int>
    {
        public DbSet<InvoiceStatus> InvoiceStatus => Set<InvoiceStatus>();
        public DbSet<User> User => Set<User>();
        public DbSet<MailHistory> MailHistory => Set<MailHistory>();
        public DbSet<Log> Log => Set<Log>();

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvoiceStatusConfiguration).Assembly);

            SeedConfiguration.Seed(modelBuilder);
        }
    }
}
