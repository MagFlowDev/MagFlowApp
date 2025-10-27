using MagFlow.Domain.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MagFlow.EF
{
    public class CoreDbContext : IdentityDbContext<ApplicationUser,ApplicationRole,Guid,
        ApplicationUserClaim,ApplicationUserRole,ApplicationUserLogin,ApplicationRoleClaim,ApplicationUserToken>
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }
        public DbSet<ApplicationUserRole> UserRoles {  get; set; }
        public DbSet<ApplicationUserClaim> UserClaims {  get; set; }
        public DbSet<ApplicationUserLogin> UserLogins {  get; set; }
        public DbSet<ApplicationRoleClaim> RoleClaims {  get; set; }
        public DbSet<ApplicationUserToken> UserTokens {  get; set; }

        public CoreDbContext(string connectionString) : base(BuildOptions(connectionString))
        {
        }

        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "ApplicationUsers");
            });
            builder.Entity<ApplicationRole>(entity =>
            {
                entity.ToTable(name: "ApplicationRoles");
            });
            builder.Entity<ApplicationUserRole>(entity =>
            {
                entity.ToTable(name: "UserRoles");
            });
            builder.Entity<ApplicationUserClaim>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });
            builder.Entity<ApplicationUserLogin>(entity =>
            {
                entity.ToTable(name: "UserLogins");
            });
            builder.Entity<ApplicationRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims");
            });
            builder.Entity<ApplicationUserToken>(entity =>
            {
                entity.ToTable(name: "UserTokens");
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        private static DbContextOptions<CoreDbContext> BuildOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }
    }
}
