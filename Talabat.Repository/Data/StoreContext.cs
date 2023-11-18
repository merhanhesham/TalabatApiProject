using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Repository.Data.Configurations;

namespace Talabat.Repository.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext>options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //call the configuration classes 
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());

            //func that apply all existing configs
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }


        //dbsets classes that will be converted to tables in db
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
    }
}
