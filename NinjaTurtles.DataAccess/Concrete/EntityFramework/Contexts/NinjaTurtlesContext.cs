﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NinjaTurtles.Core.NetCoreConfiguration;
using NinjaTurtles.Entities.Concrete;
using NinjaTurtles.Entities.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.DataAccess.Concrete.EntityFramework.Contexts
{
    public class NinjaTurtlesContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStrings = AppConfig.Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>() ??
                                    throw new Exception("Config missing key");

            optionsBuilder.UseSqlServer(connectionStrings.NinjaTurtlesConnectionString, b => b.MigrationsAssembly("NinjaTurtles.DataAccess"));

#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
#endif

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Id için otomatik artış ayarı

            modelBuilder.Entity<QrCodeMain>()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEWID()"); // Otomatik Guid atayacak

            modelBuilder.Entity<Customer>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Id için otomatik artış ayarı

            modelBuilder.Entity<QrCodeDetail>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); // Id için otomatik artış ayarı

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<QrCodeMain> QrCodeMain { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<QrCodeDetail> QrCodeDetail { get; set; }


    }
}
