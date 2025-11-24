using MagFlow.Domain.Company;
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
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<CustomParameter> CustomParameters { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<FunctionParameter> FunctionParameters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemParameter> ItemParameters { get; set; }
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
        public DbSet<ProductUnitConversion> ProductUnitConversions { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseStorage> WarehouseStorages { get; set; }

        public CompanyDbContext(string connectionString) : base(BuildOptions(connectionString))
        {
        }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        public CompanyDbContext() : base(BuildOptions(AppSettings.ConnectionStrings.CompanyDb))
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


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
            builder.Entity<Warehouse>().HasMany(w => w.Storages).WithOne(s => s.Warehouse);
            builder.Entity<Warehouse>().HasMany(w => w.Items).WithOne(i => i.Warehouse);

            
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
    }
}
