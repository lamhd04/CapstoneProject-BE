﻿// <auto-generated />
using System;
using CapstoneProject_BE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CapstoneProject_BE.Migrations
{
    [DbContext(typeof(InventoryManagementContext))]
    [Migration("20230302033859_addStorage")]
    partial class addStorage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CapstoneProject_BE.Models.ActionType", b =>
                {
                    b.Property<int>("ActionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ActionId"), 1L, 1);

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ActionId");

                    b.ToTable("ActionType", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId");

                    b.HasIndex("StorageId");

                    b.ToTable("Category", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.EmailToken", b =>
                {
                    b.Property<int>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TokenId"), 1L, 1);

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TokenId");

                    b.HasIndex("UserId");

                    b.ToTable("EmailToken", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ExportOrder", b =>
                {
                    b.Property<int>("ExportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExportId"), 1L, 1);

                    b.Property<DateTime?>("Approved")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Completed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Denied")
                        .HasColumnType("datetime2");

                    b.Property<string>("ExportCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.Property<float>("Total")
                        .HasColumnType("real");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<float>("TotalPrice")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ExportId");

                    b.HasIndex("StorageId");

                    b.HasIndex("UserId");

                    b.ToTable("ExportOrder", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ExportOrderDetail", b =>
                {
                    b.Property<int>("DetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DetailId"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<float>("Discount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValue(0f);

                    b.Property<int>("ExportId")
                        .HasColumnType("int");

                    b.Property<int?>("MeasuredUnitId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("DetailId");

                    b.HasIndex("ExportId");

                    b.HasIndex("MeasuredUnitId");

                    b.HasIndex("ProductId");

                    b.ToTable("ExportOrderDetail", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ImportOrder", b =>
                {
                    b.Property<int>("ImportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ImportId"), 1L, 1);

                    b.Property<DateTime?>("Approved")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Completed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Denied")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImportCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("InDebted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValue(0f);

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("OtherExpense")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValue(0f);

                    b.Property<float>("Paid")
                        .HasColumnType("real");

                    b.Property<int>("State")
                        .HasColumnType("int");

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<float>("Total")
                        .HasColumnType("real");

                    b.Property<int>("TotalAmount")
                        .HasColumnType("int");

                    b.Property<float>("TotalCost")
                        .HasColumnType("real");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ImportId");

                    b.HasIndex("StorageId");

                    b.HasIndex("SupplierId");

                    b.HasIndex("UserId");

                    b.ToTable("ImportOrder", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ImportOrderDetail", b =>
                {
                    b.Property<int>("DetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DetailId"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<float>("CostPrice")
                        .HasColumnType("real");

                    b.Property<float>("Discount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("real")
                        .HasDefaultValue(0f);

                    b.Property<int>("ImportId")
                        .HasColumnType("int");

                    b.Property<int?>("MeasuredUnitId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("DetailId");

                    b.HasIndex("ImportId");

                    b.HasIndex("MeasuredUnitId");

                    b.HasIndex("ProductId");

                    b.ToTable("ImportOrderDetail", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.MeasuredUnit", b =>
                {
                    b.Property<int>("MeasuredUnitId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MeasuredUnitId"), 1L, 1);

                    b.Property<string>("MeasuredUnitName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MeasuredUnitValue")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<float?>("SuggestedPrice")
                        .HasColumnType("real");

                    b.HasKey("MeasuredUnitId");

                    b.HasIndex("ProductId");

                    b.ToTable("MeasuredUnit", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"), 1L, 1);

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CategoryId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<float?>("CostPrice")
                        .HasColumnType("real");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("DefaultMeasuredUnit")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("InStock")
                        .HasColumnType("int");

                    b.Property<string>("ProductCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("SellingPrice")
                        .HasColumnType("real");

                    b.Property<bool?>("Status")
                        .HasColumnType("bit");

                    b.Property<float?>("StockPrice")
                        .HasColumnType("real");

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.Property<int?>("SupplierId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("ProductId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StorageId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Product", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ProductHistory", b =>
                {
                    b.Property<int>("HistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HistoryId"), 1L, 1);

                    b.Property<int>("ActionId")
                        .HasColumnType("int");

                    b.Property<int?>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("AmountDifferential")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("CostPrice")
                        .HasColumnType("real");

                    b.Property<string>("CostPriceDifferential")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Price")
                        .IsRequired()
                        .HasColumnType("real");

                    b.Property<string>("PriceDifferential")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("HistoryId");

                    b.HasIndex("ActionId");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ProductHistory", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.RefreshToken", b =>
                {
                    b.Property<int>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TokenId"), 1L, 1);

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRevoked")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("TokenId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("RefreshToken", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Role", b =>
                {
                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleId"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleId");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Storage", b =>
                {
                    b.Property<int>("StorageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StorageId"), 1L, 1);

                    b.Property<string>("StorageName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StorageId");

                    b.ToTable("Storage", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Supplier", b =>
                {
                    b.Property<int>("SupplierId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SupplierId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.Property<string>("SupplierEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupplierPhone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ward")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SupplierId");

                    b.HasIndex("StorageId");

                    b.ToTable("Supplier", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Phone")
                        .HasColumnType("bigint");

                    b.Property<int>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<bool>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("StorageId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.HasIndex("RoleId");

                    b.HasIndex("StorageId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Category", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Storage", "Storage")
                        .WithMany("Categories")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.EmailToken", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.User", "User")
                        .WithMany("EmailTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ExportOrder", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Storage", "Storage")
                        .WithMany("ExportOrders")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.User", "User")
                        .WithMany("ExportOrder")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Storage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ExportOrderDetail", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.ExportOrder", "ExportOrder")
                        .WithMany("ExportOrderDetails")
                        .HasForeignKey("ExportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.MeasuredUnit", "MeasuredUnit")
                        .WithMany("ExportOrderDetails")
                        .HasForeignKey("MeasuredUnitId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Product", "Product")
                        .WithMany("ExportOrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ExportOrder");

                    b.Navigation("MeasuredUnit");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ImportOrder", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Storage", "Storage")
                        .WithMany("ImportOrders")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Supplier", "Supplier")
                        .WithMany("ImportOrders")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.User", "User")
                        .WithMany("ImportOrder")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Storage");

                    b.Navigation("Supplier");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ImportOrderDetail", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.ImportOrder", "ImportOrder")
                        .WithMany("ImportOrderDetails")
                        .HasForeignKey("ImportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.MeasuredUnit", "MeasuredUnit")
                        .WithMany("ImportOrderDetails")
                        .HasForeignKey("MeasuredUnitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Product", "Product")
                        .WithMany("ImportOrderDetails")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ImportOrder");

                    b.Navigation("MeasuredUnit");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.MeasuredUnit", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Product", "Product")
                        .WithMany("MeasuredUnits")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Product", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Storage", "Storage")
                        .WithMany("Products")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Supplier", "Supplier")
                        .WithMany("Products")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Storage");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ProductHistory", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.ActionType", "ActionType")
                        .WithMany("ProductHistories")
                        .HasForeignKey("ActionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Product", "Product")
                        .WithMany("ProductHistories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.User", "User")
                        .WithMany("ProductHistories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ActionType");

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.RefreshToken", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.User", "User")
                        .WithOne("RefreshToken")
                        .HasForeignKey("CapstoneProject_BE.Models.RefreshToken", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Supplier", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Storage", "Storage")
                        .WithMany("Suppliers")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.User", b =>
                {
                    b.HasOne("CapstoneProject_BE.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CapstoneProject_BE.Models.Storage", "Storage")
                        .WithMany("Users")
                        .HasForeignKey("StorageId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("Storage");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ActionType", b =>
                {
                    b.Navigation("ProductHistories");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ExportOrder", b =>
                {
                    b.Navigation("ExportOrderDetails");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.ImportOrder", b =>
                {
                    b.Navigation("ImportOrderDetails");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.MeasuredUnit", b =>
                {
                    b.Navigation("ExportOrderDetails");

                    b.Navigation("ImportOrderDetails");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Product", b =>
                {
                    b.Navigation("ExportOrderDetails");

                    b.Navigation("ImportOrderDetails");

                    b.Navigation("MeasuredUnits");

                    b.Navigation("ProductHistories");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Storage", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("ExportOrders");

                    b.Navigation("ImportOrders");

                    b.Navigation("Products");

                    b.Navigation("Suppliers");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.Supplier", b =>
                {
                    b.Navigation("ImportOrders");

                    b.Navigation("Products");
                });

            modelBuilder.Entity("CapstoneProject_BE.Models.User", b =>
                {
                    b.Navigation("EmailTokens");

                    b.Navigation("ExportOrder");

                    b.Navigation("ImportOrder");

                    b.Navigation("ProductHistories");

                    b.Navigation("RefreshToken")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
