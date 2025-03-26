using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
    {
    }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Discount> Discounts { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductLog> ProductLogs { get; set; }
    public DbSet<Shipment> Shipments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<OrderDiscount> OrderDiscounts { get; set; }
    public DbSet<WishlistProduct> WishlistProducts { get; set; }
    public DbSet<CartProduct> CartProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderDiscount>()
            .HasKey(od => new { od.OrderId, od.DiscountId });

        modelBuilder.Entity<OrderDiscount>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDiscounts)
            .HasForeignKey(od => od.OrderId);

        modelBuilder.Entity<OrderDiscount>()
            .HasOne(od => od.Discount)
            .WithMany(d => d.OrderDiscounts)
            .HasForeignKey(od => od.DiscountId);

        modelBuilder.Entity<WishlistProduct>()
            .HasKey(wp => new { wp.WishlistId, wp.ProductId });

        modelBuilder.Entity<WishlistProduct>()
            .HasOne(wp => wp.Wishlist)
            .WithMany(w => w.WishlistProducts)
            .HasForeignKey(wp => wp.WishlistId);

        modelBuilder.Entity<WishlistProduct>()
            .HasOne(wp => wp.Product)
            .WithMany(p => p.WishlistProducts)
            .HasForeignKey(wp => wp.ProductId);

        modelBuilder.Entity<CartProduct>()
            .HasKey(cp => new { cp.CartId, cp.ProductId });

        modelBuilder.Entity<CartProduct>()
            .HasOne(cp => cp.Cart)
            .WithMany(c => c.CartProducts)
            .HasForeignKey(cp => cp.CartId);

        modelBuilder.Entity<CartProduct>()
            .HasOne(cp => cp.Product)
            .WithMany(p => p.CartProducts)
            .HasForeignKey(cp => cp.ProductId);

        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => oi.Id);
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<Discount>()
            .Property(d => d.Amount)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 4);

        modelBuilder.Entity<Order>()
    .HasOne(o => o.Payment)
    .WithOne(p => p.Order)
    .HasForeignKey<Payment>(p => p.OrderId)  // ربط المفتاح الأجنبي
    .OnDelete(DeleteBehavior.NoAction);  // ✅ تعديل نوع الحذف

    }
}
