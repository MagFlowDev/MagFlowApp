using MagFlow.Domain.CompanyScope;
using MagFlow.Shared.Models.Interfaces;
using MagFlow.Shared.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagFlow.EF
{
    public class CompanyDbContext : DbContext
    {
        private readonly int _companyNumber;

        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<CustomParameter> CustomParameters { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<FunctionParameter> FunctionParameters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemParameter> ItemParameters { get; set; }
        public DbSet<ItemComponent> ItemComponents { get; set; }
        public DbSet<Machine> Machines { get; set; }
        public DbSet<MachineFunction> MachineFunctions { get; set; }
        public DbSet<MachineFunctionParameter> MachineFunctionParameters { get; set; }
        public DbSet<MachineFunctionProduct> MachineFunctionProducts { get; set; }
        public DbSet<MachineModel> MachineModels { get; set; }
        public DbSet<MachineModelFunction> MachineModelFunctions { get; set; }
        public DbSet<MachineModelParameter> MachineModelParameters { get; set; }
        public DbSet<MachineParameter> MachineParameters { get; set; }
        public DbSet<MachineParameterImpact> MachineParameterImpacts  { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDelivery> OrderDeliveries { get; set; }
        public DbSet<OrderDeliveryItem> OrderDeliveryItems { get; set; }
        public DbSet<OrderDocument> OrderDocuments { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderType> OrderTypes { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessDocument> ProcessDocuments { get; set; }
        public DbSet<ProcessStep> ProcessSteps { get; set; }
        public DbSet<ProcessStepIO> ProcessStepIO { get; set; }
        public DbSet<ProcessStepParameter> ProcessStepParameters { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductComponent> ProductComponents { get; set; }
        public DbSet<ProductParameter> ProductParameters { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductUnitConversion> ProductUnitConversions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<UnitConversion> UnitConversions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<Claim> Claims { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseSector> WarehouseSectors { get; set; }
        public DbSet<WarehouseSectorRow> WarehouseSectorRows { get; set; }
        public DbSet<WarehouseSectorRowSlot> WarehouseSectorRowSlots { get; set; }
        public DbSet<DefaultWorkingHour> DefaultWorkingHours { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        public DbSet<EntityHistory> EntitiesHistory { get; set; }

        public CompanyDbContext(string connectionString, int companyNumber) : base(BuildOptions(connectionString))
        {
            _companyNumber = companyNumber;
        }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options, int companyNumber) : base(options)
        {
            _companyNumber = companyNumber;
        }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
            _companyNumber = 0;
        }

        public CompanyDbContext() : base(BuildOptions(AppSettings.ConnectionStrings.CompanyDb))
        {
            _companyNumber = 0;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<DefaultWorkingHour>()
                .HasIndex(x => new { x.DayOfWeek })
                .IsUnique();
            
            builder.Entity<WorkDay>()
                .HasIndex(x => new { x.Date })
                .IsUnique();

            builder.Entity<RoleClaim>().HasKey(e => new
            {
                e.RoleId,
                e.ClaimId
            });

            builder.Entity<EntityHistory>().Property(x => x.EntityType).HasConversion<string>();
            builder.Entity<EntityHistory>().HasIndex(x => new
            {
                x.EntityType,
                x.EntityId
            });

            builder.Entity<Contractor>().HasMany(c => c.Orders).WithOne(o => o.Contractor);
            builder.Entity<Contractor>().HasMany(c => c.Documents).WithOne(d => d.Contractor);
            builder.Entity<Document>().HasMany(d => d.Orders).WithOne(o => o.Document);
            builder.Entity<Document>().HasMany(d => d.Items).WithOne(i => i.DocumentHeader);
            builder.Entity<Document>().HasMany(d => d.Processes).WithOne(p => p.Document);
            builder.Entity<Item>().HasMany(i => i.Parameters).WithOne(p => p.Item);
            builder.Entity<MachineFunction>().HasMany(m => m.Impacts).WithOne(i => i.MachineFunction);
            builder.Entity<MachineModel>().HasMany(m => m.Machines).WithOne(x => x.MachineModel);
            builder.Entity<MachineModel>().HasMany(m => m.Functions).WithOne(f => f.MachineModel);
            builder.Entity<MachineModel>().HasMany(m => m.Parameters).WithOne(p => p.MachineModel);
            builder.Entity<MachineModelFunction>().HasMany(m => m.Parameters).WithOne(p => p.MachineModelFunction);
            builder.Entity<MachineModelFunction>().HasMany(m => m.Products).WithOne(p => p.MachineModelFunction);
            builder.Entity<MachineParameter>().HasMany(m => m.Impacts).WithOne(i => i.MachineParameter);
            builder.Entity<Order>().HasMany(o => o.Deliveries).WithOne(d => d.Order);
            builder.Entity<Order>().HasMany(o => o.Documents).WithOne(d => d.Order);
            builder.Entity<Order>().HasMany(o => o.Items).WithOne(i => i.Order);
            builder.Entity<Process>().HasMany(p => p.Documents).WithOne(d => d.Process);
            builder.Entity<Process>().HasMany(p => p.Steps).WithOne(s => s.Process);
            builder.Entity<Product>().HasMany(c => c.Components).WithOne(p => p.Product);
            builder.Entity<Product>().HasMany(c => c.Parameters).WithOne(p => p.Product);
            builder.Entity<Product>().HasMany(c => c.Conversions).WithOne(p => p.Product);
            builder.Entity<Warehouse>().HasMany(w => w.Sectors).WithOne(s => s.Warehouse);
            builder.Entity<Warehouse>().HasMany(w => w.Items).WithOne(i => i.Warehouse);
            builder.Entity<WarehouseSector>().HasMany(w => w.Rows).WithOne(s => s.Sector);
            builder.Entity<WarehouseSector>().HasMany(w => w.Items).WithOne(i => i.Sector);
            builder.Entity<WarehouseSectorRow>().HasMany(w => w.Slots).WithOne(s => s.Row);
            builder.Entity<WarehouseSectorRow>().HasMany(w => w.Items).WithOne(i => i.Row);
            builder.Entity<WarehouseSectorRowSlot>().HasMany(w => w.Items).WithOne(i => i.Slot);

            builder.Entity<CustomParameter>().HasOne(x => x.Unit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FunctionParameter>().HasOne(x => x.Unit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineParameter>().HasOne(x => x.Unit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineParameterImpact>().HasOne(x => x.Unit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Product>().HasOne(x => x.Unit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductUnitConversion>().HasOne(x => x.FromUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductUnitConversion>().HasOne(x => x.ToUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UnitConversion>().HasOne(x => x.FromUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<UnitConversion>().HasOne(x => x.ToUnit).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineModelFunction>().HasOne(x => x.MachineModel).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineFunctionParameter>().HasOne(x => x.MachineModelFunction).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineFunctionParameter>().HasOne(x => x.FunctionParameter).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineFunctionProduct>().HasOne(x => x.MachineModelFunction).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Machine>().HasOne(x => x.MachineModel).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(x => x.OrderType).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(x => x.Contractor).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderDelivery>().HasOne(x => x.Order).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderDelivery>().HasOne(x => x.Document).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderDeliveryItem>().HasOne(x => x.OrderDelivery).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderItem>().HasOne(x => x.Order).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Document>().HasOne(x => x.DocumentType).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductComponent>().HasOne(x => x.Product).WithMany(x => x.Components).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductComponent>().HasOne(x => x.Component).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Product>().HasOne(x => x.Type).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ProductType>().HasOne(x => x.Category).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.Product).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.Warehouse).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.Sector).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.Row).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.Slot).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Item>().HasOne(x => x.RemovedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ItemComponent>().HasOne(x => x.Parent).WithMany(x => x.Components).HasForeignKey(p => p.ParentId).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ItemComponent>().HasOne(i => i.Component).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Contractor>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Document>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Machine>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineFunction>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MachineModel>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderDelivery>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<OrderType>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Process>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Product>().HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Document>().HasOne(x => x.ConfirmedBy).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Order>().HasOne(x => x.ConfirmedBy).WithMany().OnDelete(DeleteBehavior.NoAction);

            builder.Ignore<Shared.Models.StatusEntity>();
            foreach(var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IStatusEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var property = entityType.ClrType.GetProperty(nameof(IStatusEntity.Status));
                    if (property != null)
                    {
                        builder.Entity(entityType.ClrType).Property(property.Name).HasField("_status").UsePropertyAccessMode(PropertyAccessMode.Field);
                    }
                }
                if (typeof(ICodeEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var property = entityType.ClrType.GetProperty(nameof(ICodeEntity.Code));
                    if(property != null)
                    {
                        builder.Entity(entityType.ClrType).Property(property.Name).IsRequired(false);
                    }
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.ConfigureWarnings(warnings => warnings.Log(RelationalEventId.PendingModelChangesWarning));
        }

        private static DbContextOptions<CompanyDbContext> BuildOptions(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return optionsBuilder.Options;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var addedEntries = ChangeTracker.Entries<ICodeEntity>()
                .Where(e => e.State == EntityState.Added)
                .ToList();

            if(!addedEntries.Any())
            {
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }

            using var transaction = await Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                string currentYear = DateTime.UtcNow.ToString("yy");

                foreach (var entry in addedEntries)
                {
                    var config = Domain.Configs.GetConfig(entry.Entity, _companyNumber);

                    var id = entry.Property("Id").CurrentValue;
                    var idFormat = $"D{config.MinDigits}";
                    string formattedId = Convert.ToInt32(id).ToString(idFormat);

                    string generatedCode = config.IncludeYear
                        ? $"{config.Prefix}-{currentYear}-{formattedId}"
                        : $"{config.Prefix}-{formattedId}";

                    entry.State = EntityState.Unchanged;
                    entry.Property(nameof(ICodeEntity.Code)).CurrentValue = generatedCode;
                    entry.Property(nameof(ICodeEntity.Code)).IsModified = true;
                }

                await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return result;
            }
            catch(Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
