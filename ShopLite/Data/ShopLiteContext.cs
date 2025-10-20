using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopLite.Models;

namespace ShopLite.Data;

public class ShopLiteContext : DbContext
{
    public ShopLiteContext (DbContextOptions<ShopLiteContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Category { get; set; } = default!;
    public DbSet<Product> Product { get; set; } = default!;
    public DbSet<Order> Order { get; set; } = default!;
    public DbSet<OrderProduct> OrderProduct { get; set; } = default!;



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderProduct>()
            .HasKey(op => new { op.OrderID, op.ProductID });

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Order)
            .WithMany(o => o.OrderProducts)
            .HasForeignKey(op => op.OrderID);

        modelBuilder.Entity<OrderProduct>()
            .HasOne(op => op.Product)
            .WithMany(p => p.OrderProducts)
            .HasForeignKey(op => op.ProductID);
    }

}
