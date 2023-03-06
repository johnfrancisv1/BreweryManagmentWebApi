﻿using System;
using System.Collections.Generic;
using BreweryWholesaleService.Core.EntityModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BreweryWholesaleService.Infrastructure.EntityModels
{
    public partial class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
        {
        }

      
        public virtual DbSet<Beer> Beers { get; set; } = null!;
        public virtual DbSet<Stock> Stocks { get; set; } = null!;

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Beer>(entity =>
            {
                entity.ToTable("Beer");

                entity.HasIndex(e => e.Name, "UNQ_Beer_Name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BreweryId).HasMaxLength(450);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Price).HasColumnType("money");

                entity.HasOne(d => d.Brewery)
                    .WithMany(p => p.Beers)
                    .HasForeignKey(d => d.BreweryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Beer_AspNetUsers");
            });

            modelBuilder.Entity<Stock>(entity =>
            {
                entity.ToTable("Stock");

                entity.HasIndex(e => new { e.WholeSalerId, e.BeerId }, "UC_WholeSaler_Bear")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BeerId).HasColumnName("BeerID");

                entity.Property(e => e.WholeSalerId).HasColumnName("WholeSalerID");

                entity.HasOne(d => d.Beer)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.BeerId)
                    .HasConstraintName("FK_Stock_Beer");

                entity.HasOne(d => d.WholeSaler)
                    .WithMany(p => p.Stocks)
                    .HasForeignKey(d => d.WholeSalerId)
                    .HasConstraintName("FK_Stock_AspNetUsers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
