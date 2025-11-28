using MagFlow.Domain.Core;
using MagFlow.EF.Seeds.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
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
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyUser> CompanyUsers { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<CompanyModule> CompanyModules { get; set; }
        public DbSet<Module> Modules { get; set; }

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

                var roleId = (Microsoft.EntityFrameworkCore.Metadata.Internal.Property)entity.Metadata.FindProperty("RoleId");
                var roleIdFk = entity.Metadata.FindForeignKeys(roleId).Single();
                entity.Metadata.RemoveForeignKey(roleIdFk);
                var roleId1 = entity.Metadata.FindProperty("RoleId1");
                var roleId1Fk = entity.Metadata.FindForeignKeys(roleId1).Single();
                var applicationRolePk = builder.Entity<ApplicationRole>().Metadata.FindPrimaryKey();
                roleId1Fk.SetProperties(new List<Microsoft.EntityFrameworkCore.Metadata.Internal.Property> { roleId }, applicationRolePk);
                entity.Metadata.RemoveProperty(roleId1);

                var userId = (Microsoft.EntityFrameworkCore.Metadata.Internal.Property)entity.Metadata.FindProperty("UserId");
                var userIdFk = entity.Metadata.FindForeignKeys(userId).Single();
                entity.Metadata.RemoveForeignKey(userIdFk);
                var userId1 = entity.Metadata.FindProperty("UserId1");
                var userId1Fk = entity.Metadata.FindForeignKeys(userId1).Single();
                var applicationUserPk = builder.Entity<ApplicationUser>().Metadata.FindPrimaryKey();
                userId1Fk.SetProperties(new List<Microsoft.EntityFrameworkCore.Metadata.Internal.Property> { userId }, applicationUserPk);
                entity.Metadata.RemoveProperty(userId1);
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
            builder.Entity<CompanyUser>().HasKey(e => new
            {
                e.UserId, e.CompanyId
            });

            builder.Entity<CompanyUser>().HasOne(c => c.Company).WithMany(u => u.Users).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<CompanyUser>().HasOne(u => u.User).WithMany(c => c.Companies).OnDelete(DeleteBehavior.ClientCascade);
            builder.Entity<ApplicationUser>().HasMany(l => l.AuditLogs).WithOne(u => u.User);
            builder.Entity<ApplicationUser>().HasMany(l => l.EventLogs).WithOne(u => u.User);
            builder.Entity<ApplicationUser>().HasMany(n => n.Notifications).WithOne(u => u.User);
            builder.Entity<ApplicationUser>().HasMany(s => s.Sessions).WithOne(u => u.User);
            builder.Entity<ApplicationUser>().HasOne(s => s.UserSettings).WithOne(u => u.User);
            builder.Entity<ApplicationUser>().HasMany(r => r.Roles).WithOne(u => u.User);
            builder.Entity<ApplicationRole>().HasMany(u => u.Users).WithOne(r => r.Role);
            builder.Entity<UserNotification>().HasOne(n => n.Notification);
            builder.Entity<AuditLog>().HasMany(lc => lc.Changes).WithOne(l => l.AuditLog);
            builder.Entity<Company>().HasMany(m => m.Modules).WithOne(c => c.Company);
            builder.Entity<CompanyModule>().HasMany(p => p.ModulePricings).WithOne(m => m.CompanyModule);
            builder.Entity<CompanyModule>().HasOne(m => m.Module).WithMany(c => c.CompanyModules);
            builder.Entity<Module>().HasMany(p => p.Pricings).WithOne(m => m.Module);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));

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
