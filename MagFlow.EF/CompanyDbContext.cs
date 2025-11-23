using MagFlow.Domain.Company;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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
