using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

            public DbSet<Product> Products {get; set;}    
            public DbSet<ProductType> ProductTypes {get; set;}   
            public DbSet<ProductBrand> ProductBrands {get; set;}  
            public DbSet<Order> Orders {get; set;}  
            public DbSet<OrderItem> OrderItems {get; set;} 
            public DbSet<DeliveryMethod> DeliveryMethods {get; set;} 

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

                if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
                {
                        foreach(var entityType in modelBuilder.Model.GetEntityTypes())
                        {
                            var properties = entityType.ClrType.GetProperties().Where(p=>p.PropertyType == typeof(decimal));
                            var dateProperties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset));
                            foreach(var prop in properties)
                            {
                                modelBuilder.Entity(entityType.Name).Property(prop.Name).HasConversion<double>();
                            }
                            
                            foreach(var prop in dateProperties)
                            {
                                modelBuilder.Entity(entityType.Name).Property(prop.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
                            }
                        }
                }
            } 
        }
}