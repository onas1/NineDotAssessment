using Microsoft.EntityFrameworkCore;
using NineDotAssessment.Core.Entities;

namespace NineDotAssessment.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; }
        public DbSet<AuthPin> AuthPins { get;}
        public DbSet<OtpVerification> OtpVerifications { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
