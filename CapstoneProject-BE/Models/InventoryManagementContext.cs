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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.HasKey(u => u.UserId);
                e.HasOne(u => u.Role)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.RoleId);
                e.Property(u => u.Email).IsRequired();
                e.Property(u => u.Password).IsRequired();
                e.Property(u => u.RoleId).HasDefaultValue(0);
                e.Property(u => u.UserId).UseIdentityColumn();
                e.Property(u => u.Status).HasDefaultValue(false);
            });
            modelBuilder.Entity<Role>(e =>
            {
                e.ToTable("Role");
                e.HasKey(r => r.RoleId);
                e.Property(r => r.RoleName).IsRequired();
                e.Property(u => u.RoleId).UseIdentityColumn();
            });
            modelBuilder.Entity<RefreshToken>(e =>
            {
                e.ToTable("RefreshToken");
                e.HasKey("TokenId");
                e.HasOne(rf => rf.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(u => u.UserId);
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
                e.Property(u => u.Token).IsRequired();
                e.Property(u => u.TokenId).UseIdentityColumn();
            });
            modelBuilder.Entity<Category>(e =>
            {
                e.ToTable("Category");
                e.HasKey(r => r.CategoryId);
                e.HasMany(r => r.Products)
                .WithOne(r => r.Category)
                .HasForeignKey(r => r.CategoryId);
                e.Property(u => u.CategoryName).IsRequired();
                e.Property(u => u.CategoryId).UseIdentityColumn();
            });
            modelBuilder.Entity<Supplier>(e =>
            {
                e.ToTable("Supplier");
                e.HasKey(r => r.SupplierId);
                e.HasMany(r => r.ImportOrders)
.WithOne(r => r.Supplier)
.HasForeignKey(r => r.SupplierId).OnDelete(DeleteBehavior.NoAction);
                e.Property(u => u.SupplierName).IsRequired();
                e.Property(u => u.SupplierPhone).IsRequired();
                e.Property(u => u.Ward).IsRequired();
                e.Property(u => u.City).IsRequired();
                e.Property(u => u.Address).IsRequired();
                e.Property(u => u.District).IsRequired();
                e.Property(u => u.SupplierId).UseIdentityColumn();
            });
            modelBuilder.Entity<MeasuredUnit>(e =>
            {
                e.ToTable("MeasuredUnit");
                e.HasKey(r => r.MeasuredUnitId);
                e.Property(u => u.MeasuredUnitName).IsRequired();
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
                e.Property(u => u.Discount).HasDefaultValue(0);
                e.Property(u => u.ImportId).UseIdentityColumn();
            });
            modelBuilder.Entity<ImportOrderDetail>(e =>
            {
                e.ToTable("ImportOrderDetail");
                e.HasKey(r => r.ImportId);
                e.HasOne(r => r.MeasuredUnit)
                .WithMany(r => r.ImportOrderDetail)
                .HasForeignKey(r => r.MeasuredUnitId);
                e.Property(u => u.Amount).IsRequired();
                e.Property(u => u.CostPrice).IsRequired();
                e.Property(u => u.ProductId).IsRequired();
                e.Property(u => u.Discount).HasDefaultValue(0);
                e.Property(u => u.Price).IsRequired();
            });
            modelBuilder.Entity<ProductHistory>(e =>
            {
                e.ToTable("ProductHistory");
                e.HasKey(r => r.HistoryId);
                e.HasOne(r => r.Product)
                .WithMany(r => r.ProductHistories)
                .HasForeignKey(r => r.ProductId);
                e.Property(u => u.ProductId).IsRequired();
                e.Property(u => u.Price).IsRequired();
            });
        }
    }
}
