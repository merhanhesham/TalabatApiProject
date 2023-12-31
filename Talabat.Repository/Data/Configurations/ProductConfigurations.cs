﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(b => b.ProductBrand)
                   .WithMany()
                   .HasForeignKey(b => b.ProductBrandId);

            builder.HasOne(t => t.ProductType)
                   .WithMany()
                   .HasForeignKey(t => t.ProductTypeId);
            //make name,desc,url required
            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Description)
                   .IsRequired();

            builder.Property(p => p.PictureUrl)
                  .IsRequired();
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        }
    }
}
