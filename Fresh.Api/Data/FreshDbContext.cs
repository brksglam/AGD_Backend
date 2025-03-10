using Fresh.Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fresh.Api.Data
{
    public class FreshDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public FreshDbContext(DbContextOptions<FreshDbContext> options) : base(options)
        {
        }

        // Kullanıcı işlemleri
        public DbSet<UserDeposit> UserDeposits { get; set; }
        public DbSet<UserWithdraw> UserWithdraws { get; set; }

        // Yatırım işlemleri
        public DbSet<Investment> Investments { get; set; }
    }
}
