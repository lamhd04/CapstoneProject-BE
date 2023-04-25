using Microsoft.EntityFrameworkCore;

namespace CapstoneProject_BE.Models
{
    public class InventoryManagementContext : DbContext
    {
        public InventoryManagementContext(DbContextOptions<InventoryManagementContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<EmailToken> EmailTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<MeasuredUnit> MeasuredUnits { get; set; }
        public DbSet<ImportOrder> ImportOrders { get; set; }
        public DbSet<ImportOrderDetail> ImportOrderDetails { get; set; }
        public DbSet<ExportOrder> ExportOrders { get; set; }
        public DbSet<ExportOrderDetail> ExportOrderDetails { get; set; }
        public DbSet<StocktakeNote> StocktakeNotes { get; set; }
        public DbSet<StocktakeNoteDetail> StocktakeNoteDetails { get; set; }
        public DbSet<ActionType> ActionTypes { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<ProductHistory> ProductHistories { get; set; }
        public DbSet<ReturnsOrder> ReturnsOrders { get; set; }
        public DbSet<ReturnsOrderDetail> ReturnsOrderDetails { get; set; }
        public DbSet<AvailableForReturns> AvailableForReturns { get; set; }
        public DbSet<YearlyData> YearlyDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(u => u.UserId);
                e.HasOne(u => u.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId);
                e.HasMany(u => u.ProductHistories)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId);
                e.Property(u => u.Password).IsRequired().HasMaxLength(32);
                e.Property(u => u.Address).HasMaxLength(250);
                e.Property(u => u.UserCode).HasMaxLength(24);
                e.Property(u => u.UserName).HasMaxLength(100);
                e.Property(u => u.Identity).HasMaxLength(12);
                e.Property(u => u.Email).HasMaxLength(62);
                e.Property(u => u.RoleId).HasDefaultValue(0);
                e.Property(u => u.UserId).UseIdentityColumn();
                e.Property(u => u.Status).HasDefaultValue(false);
            });
            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Role");
                e.HasKey(r => r.RoleId);
                e.Property(r => r.RoleName).IsRequired().HasMaxLength(100);
                e.Property(u => u.RoleId);
            });
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.ToTable("RefreshToken");
                e.HasKey("TokenId");
                e.HasOne(rf => rf.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(u => u.UserId);
                e.Property(u => u.Token).HasMaxLength(44);
                e.Property(u => u.JwtId).HasMaxLength(36);
                e.Property(u => u.IsRevoked).HasDefaultValue(false);
                e.Property(u => u.TokenId).UseIdentityColumn();
            });
            modelBuilder.Entity<EmailToken>(e =>
            {
                e.ToTable("EmailToken");
                e.HasKey(r => r.TokenId);
                e.HasOne(t => t.User)
                .WithMany(a => a.EmailTokens)
                .HasForeignKey(u => u.UserId);
                e.Property(u => u.Token).IsRequired().HasMaxLength(64);
                e.Property(u => u.TokenId).UseIdentityColumn();
            });
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(r => r.CategoryId);
                e.HasMany(r => r.Products)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId);
                e.Property(u => u.CategoryName).IsRequired().HasMaxLength(100);
                e.Property(u => u.Description).HasMaxLength(250);
                e.Property(u => u.CategoryId).UseIdentityColumn();
            });
            modelBuilder.Entity<Supplier>(e =>
            {
                e.ToTable("Supplier");
                e.HasKey(r => r.SupplierId);
                e.HasMany(r => r.ImportOrders)
.WithOne(r => r.Supplier)
.HasForeignKey(r => r.SupplierId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.SupplierName).IsRequired().HasMaxLength(100);
                e.Property(u => u.SupplierPhone).IsRequired().HasMaxLength(15);
                e.Property(u => u.Ward).IsRequired().HasMaxLength(100);
                e.Property(u => u.City).IsRequired().HasMaxLength(100);
                e.Property(u => u.District).IsRequired().HasMaxLength(100);
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.Address).HasMaxLength(250);
                e.Property(u => u.SupplierEmail).HasMaxLength(62);
                e.Property(u => u.SupplierId).UseIdentityColumn();
            });
            modelBuilder.Entity<MeasuredUnit>(e =>
            {
                e.ToTable("MeasuredUnit");
                e.HasKey(r => r.MeasuredUnitId);
                e.Property(u => u.MeasuredUnitName).IsRequired().HasMaxLength(100);
                e.Property(u => u.MeasuredUnitValue).IsRequired();
                e.Property(u => u.MeasuredUnitId).UseIdentityColumn();
            });
            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Product");
                e.HasKey(r => r.ProductId);
                e.HasOne(t => t.Supplier)
                .WithMany(a => a.Products)
                .HasForeignKey(u => u.SupplierId);
                e.HasMany(t => t.MeasuredUnits)
                .WithOne(t => t.Product)
                .HasForeignKey(t => t.ProductId);
                e.HasMany(r => r.ImportOrderDetails)
.WithOne(r => r.Product)
.HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.ExportOrderDetails)
.WithOne(r => r.Product)
.HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.NoAction);
                e.Property(p => p.CostPrice).HasDefaultValue(0);
                e.Property(p => p.InStock).HasDefaultValue(0);
                e.Property(p => p.SellingPrice).HasDefaultValue(0);
                e.Property(p => p.StockPrice).HasDefaultValue(0);
                e.Property(u => u.Description).HasMaxLength(250);
                e.Property(u => u.ProductCode).HasMaxLength(24);
                e.Property(u => u.Barcode).HasMaxLength(24);
                e.Property(u => u.DefaultMeasuredUnit).HasMaxLength(100);
                e.Property(u => u.ProductName).HasMaxLength(100);
                e.Property(u => u.ProductId).UseIdentityColumn();
            });

            modelBuilder.Entity<ImportOrder>(e =>
            {
                e.ToTable("ImportOrder");
                e.HasKey(r => r.ImportId);
                e.HasMany(r => r.ImportOrderDetails)
                .WithOne(r => r.ImportOrder)
                .HasForeignKey(r => r.ImportId);
                e.HasOne(r => r.User)
                .WithMany(r => r.ImportOrder)
                .HasForeignKey(r => r.UserId);
                e.Property(u => u.Paid).IsRequired();
                e.Property(u => u.TotalCost).IsRequired();
                e.Property(u => u.TotalAmount).IsRequired();
                e.Property(u => u.OtherExpense).HasDefaultValue(0);
                e.Property(u => u.InDebted).HasDefaultValue(0);
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.ImportCode).HasMaxLength(50);
                e.Property(u => u.ImportId).UseIdentityColumn();
            });
            modelBuilder.Entity<ImportOrderDetail>(e =>
            {
                e.ToTable("ImportOrderDetail");
                e.HasKey(r => r.DetailId);
                e.HasOne(r => r.MeasuredUnit)
                .WithMany(r => r.ImportOrderDetails)
                .HasForeignKey(r => r.MeasuredUnitId);
                e.Property(u => u.Amount).IsRequired();
                e.Property(u => u.CostPrice).IsRequired();
                e.Property(u => u.ProductId).IsRequired();
                e.Property(u => u.Discount).HasDefaultValue(0);
                e.Property(u => u.DetailId).UseIdentityColumn();
            });
            modelBuilder.Entity<ExportOrder>(e =>
            {
                e.ToTable("ExportOrder");
                e.HasKey(r => r.ExportId);
                e.HasMany(r => r.ExportOrderDetails)
                .WithOne(r => r.ExportOrder)
                .HasForeignKey(r => r.ExportId);
                e.HasOne(r => r.User)
                .WithMany(r => r.ExportOrder)
                .HasForeignKey(r => r.UserId);
                e.Property(u => u.TotalPrice).IsRequired();
                e.Property(u => u.TotalAmount).IsRequired();
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.ExportCode).HasMaxLength(50);
                e.Property(u => u.ExportId).UseIdentityColumn();
            });
            modelBuilder.Entity<ExportOrderDetail>(e =>
            {
                e.ToTable("ExportOrderDetail");
                e.HasKey(r => r.DetailId);
                e.HasOne(r => r.MeasuredUnit)
                .WithMany(r => r.ExportOrderDetails)
                .HasForeignKey(r => r.MeasuredUnitId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.Amount).IsRequired();
                e.Property(u => u.ProductId).IsRequired();
                e.Property(u => u.Discount).HasDefaultValue(0);
                e.Property(u => u.Price).IsRequired();
                e.Property(u => u.DetailId).UseIdentityColumn();
            });
            modelBuilder.Entity<StocktakeNote>(e =>
            {
                e.ToTable("StocktakeNote");
                e.HasKey(r => r.StocktakeId);
                e.HasMany(r => r.StocktakeNoteDetails)
                .WithOne(r => r.StocktakeNote)
                .HasForeignKey(r => r.StocktakeId);
                e.HasOne(r => r.CreatedBy)
                .WithMany(r => r.CreatedStocktakeNotes)
                .HasForeignKey(r => r.CreatedId);
                e.HasOne(r => r.UpdatedBy)
.WithMany(r => r.UpdatedStocktakeNotes)
.HasForeignKey(r => r.UpdatedId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.StocktakeCode).HasMaxLength(50);
                e.Property(u => u.StocktakeId).UseIdentityColumn();
            });
            modelBuilder.Entity<StocktakeNoteDetail>(e =>
            {
                e.ToTable("StocktakeNoteDetail");
                e.HasKey(r => r.DetailId);
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.DetailId).UseIdentityColumn();
            });
            modelBuilder.Entity<AvailableForReturns>(e =>
            {
                e.ToTable("AvailableForReturns");
                e.HasKey(r => r.Id);
                e.HasOne(r => r.ImportOrder)
                .WithMany(r => r.AvailableForReturns)
                .HasForeignKey(r => r.ImportId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(r => r.ExportOrder)
.WithMany(r => r.AvailableForReturns)
.HasForeignKey(r => r.ExportId).OnDelete(DeleteBehavior.NoAction);
                e.HasOne(r => r.Product)
.WithMany(r => r.AvailableForReturns)
.HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.Id).UseIdentityColumn();
            });
            modelBuilder.Entity<ReturnsOrder>(e =>
            {
                e.ToTable("ReturnsOrder");
                e.HasKey(r => r.ReturnsId);
                e.HasMany(r => r.ReturnsOrderDetails)
                .WithOne(r => r.ReturnsOrder)
                .HasForeignKey(r => r.ReturnsId);
                e.HasOne(r => r.ImportOrder)
.WithMany(r => r.ReturnsOrders)
.HasForeignKey(r => r.ImportId);
                e.HasOne(r => r.ExportOrder)
.WithMany(r => r.ReturnsOrders)
.HasForeignKey(r => r.ExportId);
                e.HasOne(r => r.User)
                .WithMany(r => r.ReturnsOrders)
                .HasForeignKey(r => r.UserId);
                e.HasOne(r => r.Storage)
.WithMany(r => r.ReturnsOrders)
.HasForeignKey(r => r.StorageId);
                e.HasOne(r => r.Supplier)
.WithMany(r => r.ReturnsOrders)
.HasForeignKey(r => r.SupplierId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.ReturnsCode).HasMaxLength(50);
                e.Property(u => u.ReturnsId).UseIdentityColumn();
            });
            modelBuilder.Entity<ReturnsOrderDetail>(e =>
            {
                e.ToTable("ReturnsOrderDetail");
                e.HasKey(r => r.DetailId);
                e.HasOne(r => r.MeasuredUnit)
                .WithMany(r => r.ReturnsOrderDetails)
                .HasForeignKey(r => r.MeasuredUnitId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.DetailId).UseIdentityColumn();
            });
            modelBuilder.Entity<YearlyData>(e =>
            {
                e.ToTable("YearlyData");
                e.HasKey(r => r.Id);
                e.HasOne(r => r.Storage)
                .WithMany(r => r.YearlyDatas)
                .HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.Id).UseIdentityColumn();
            });
            modelBuilder.Entity<ProductHistory>(e =>
            {
                e.ToTable("ProductHistory");
                e.HasKey(r => r.HistoryId);
                e.HasOne(r => r.Product)
                .WithMany(r => r.ProductHistories)
                .HasForeignKey(r => r.ProductId);
                e.Property(u => u.Note).HasMaxLength(250);
                e.Property(u => u.AmountDifferential).HasMaxLength(11);
                e.Property(u => u.CostPriceDifferential).HasMaxLength(50);
                e.Property(u => u.PriceDifferential).HasMaxLength(50);
                e.Property(u => u.ActionCode).HasMaxLength(50);
                e.Property(u => u.HistoryId).UseIdentityColumn();
            });
            modelBuilder.Entity<ActionType>(e =>
            {
                e.ToTable("ActionType");
                e.HasKey(r => r.ActionId);
                e.HasMany(r => r.ProductHistories)
                .WithOne(r => r.ActionType)
                .HasForeignKey(r => r.ActionId);
                e.Property(u => u.Action).IsRequired();
                e.Property(u => u.Action).HasMaxLength(100);
                e.Property(u => u.Description).HasMaxLength(250);
                e.Property(u => u.ActionId);
            });
            modelBuilder.Entity<Storage>(e =>
            {
                e.ToTable("Storage");
                e.HasKey(r => r.StorageId);
                e.HasMany(r => r.Users)
                .WithOne(r => r.Storage)
                .HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.Suppliers)
.WithOne(r => r.Storage)
.HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.ImportOrders)
.WithOne(r => r.Storage)
.HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.ExportOrders)
.WithOne(r => r.Storage)
.HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.Products)
.WithOne(r => r.Storage)
.HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.Categories)
.WithOne(r => r.Storage)
.HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.HasMany(r => r.StocktakeNotes)
.WithOne(r => r.Storage)
.HasForeignKey(r => r.StorageId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.StorageName).HasMaxLength(100);
                e.Property(u => u.StorageId).UseIdentityColumn();
            });
        }
    }
}
