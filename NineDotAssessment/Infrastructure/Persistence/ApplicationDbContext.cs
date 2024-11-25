using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Application.Interfaces;
using NineDotAssessment.Core.Entities;
using System.Reflection;

namespace NineDotAssessment.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers => Set<ApplicationUser>();

        public DbSet<AuthPin> AuthPins => Set<AuthPin>();

        public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationUser>()
    .HasIndex(u => u.ICNumber)
    .IsUnique();
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }

    }
}
