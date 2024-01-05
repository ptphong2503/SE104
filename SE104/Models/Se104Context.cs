using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SE104.Models;

public partial class Se104Context : DbContext
{
    public Se104Context()
    {
    }

    public Se104Context(DbContextOptions<Se104Context> options)
        : base(options)
    {
    }

    public virtual DbSet<TbAttributesPrice> TbAttributesPrices { get; set; }

    public virtual DbSet<TbColor> TbColors { get; set; }

    public virtual DbSet<TbCustomer> TbCustomers { get; set; }

    public virtual DbSet<TbLocation> TbLocations { get; set; }

    public virtual DbSet<TbOrder> TbOrders { get; set; }

    public virtual DbSet<TbOrderDetail> TbOrderDetails { get; set; }

    public virtual DbSet<TbProduct> TbProducts { get; set; }

    public virtual DbSet<TbProductCategory> TbProductCategories { get; set; }

    public virtual DbSet<TbProductImage> TbProductImages { get; set; }

    public virtual DbSet<TbSize> TbSizes { get; set; }

    public virtual DbSet<TbTransactStatus> TbTransactStatuses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=THANHPHONG\\SQLEXPRESS; Database = SE104; User ID=sa;Password=123456;Integrated Security=true;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbAttributesPrice>(entity =>
        {
            entity.HasKey(e => e.AttributeId);

            entity.ToTable("tb_AttributesPrices");

            entity.Property(e => e.AttributeId).HasColumnName("AttributeID");
            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SizeId).HasColumnName("SizeID");
        });

        modelBuilder.Entity<TbColor>(entity =>
        {
            entity.HasKey(e => e.ColorId);

            entity.ToTable("tb_Color");

            entity.Property(e => e.ColorId).HasColumnName("ColorID");
            entity.Property(e => e.ColorName).HasMaxLength(50);
        });

        modelBuilder.Entity<TbCustomer>(entity =>
        {
            entity.HasKey(e => e.CustomerId);

            entity.ToTable("tb_Customers");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .IsFixedLength();
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.Phone)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.Salt)
                .HasMaxLength(8)
                .IsFixedLength();
        });

        modelBuilder.Entity<TbLocation>(entity =>
        {
            entity.HasKey(e => e.LocationId);

            entity.ToTable("tb_Locations");

            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.NameWithType).HasMaxLength(100);
            entity.Property(e => e.Slug).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(10);
        });

        modelBuilder.Entity<TbOrder>(entity =>
        {
            entity.HasKey(e => e.OrderId);

            entity.ToTable("tb_Orders");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.LocationId).HasColumnName("LocationID");
            entity.Property(e => e.OrderDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(12)
                .IsUnicode(false);
            entity.Property(e => e.ShipDate).HasColumnType("datetime");
            entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");

            entity.HasOne(d => d.Customer).WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_tb_Orders_tb_Customers");

            entity.HasOne(d => d.TransactStatus).WithMany(p => p.TbOrders)
                .HasForeignKey(d => d.TransactStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tb_Orders_tb_TransactStatus");
        });

        modelBuilder.Entity<TbOrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId);

            entity.ToTable("tb_OrderDetails");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.ColorName).HasMaxLength(100);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.SizeValue)
                .HasMaxLength(10)
                .IsFixedLength();

            entity.HasOne(d => d.Order).WithMany(p => p.TbOrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_tb_OrderDetails_tb_Orders");
        });

        modelBuilder.Entity<TbProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId);

            entity.ToTable("tb_Products");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.Alias).HasMaxLength(255);
            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.ShortDesc).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Cat).WithMany(p => p.TbProducts)
                .HasForeignKey(d => d.CatId)
                .HasConstraintName("FK_tb_Products_tb_ProductCategories");
        });

        modelBuilder.Entity<TbProductCategory>(entity =>
        {
            entity.HasKey(e => e.CatId);

            entity.ToTable("tb_ProductCategories");

            entity.Property(e => e.CatId).HasColumnName("CatID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Alias).HasMaxLength(250);
            entity.Property(e => e.CatName).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DateModified).HasColumnType("datetime");
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.ParentId).HasColumnName("ParentID");
            entity.Property(e => e.RootId).HasColumnName("RootID");
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TbProductImage>(entity =>
        {
            entity.ToTable("tb_ProductImages");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
        });

        modelBuilder.Entity<TbSize>(entity =>
        {
            entity.HasKey(e => e.SizeId);

            entity.ToTable("tb_Size");

            entity.Property(e => e.SizeId).HasColumnName("SizeID");
            entity.Property(e => e.SizeValue)
                .HasMaxLength(10)
                .IsFixedLength();
        });

        modelBuilder.Entity<TbTransactStatus>(entity =>
        {
            entity.HasKey(e => e.TransactStatusId);

            entity.ToTable("tb_TransactStatus");

            entity.Property(e => e.TransactStatusId).HasColumnName("TransactStatusID");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
