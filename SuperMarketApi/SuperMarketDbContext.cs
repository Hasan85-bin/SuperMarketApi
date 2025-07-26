using Microsoft.EntityFrameworkCore;
using SuperMarketApi.Models;

namespace SuperMarketApi
{
    public class SuperMarketDbContext : DbContext
    {
        public SuperMarketDbContext(DbContextOptions<SuperMarketDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique index for UserName
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            // Unique index for Email
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Unique index for Phone
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Phone)
                .IsUnique();

            // User-PurchaseCart: one-to-many, cascade delete
            modelBuilder.Entity<Purchase>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(pc => pc.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchasedCartItem relationships
            modelBuilder.Entity<PurchaseItem>()
                .HasOne(pci => pci.Purchase)
                .WithMany(pc => pc.Items)
                .HasForeignKey(pci => pci.PurchaseID)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem relationships (shopping cart only)
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(ci => ci.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany()
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial users (passwords are SHA256 hashed as in UserService)
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = 1,
                    UserName = "JohnDoe",
                    Email = "john.doe@example.com",
                    Phone = "123-456-7890",
                    Role = RoleEnum.Customer,
                    Password = "75K3eLr+dx6JJFuJ7LwIpEpOFmwGZZkRiB84PURz6U8=" // password123
                },
                new User
                {
                    ID = 2,
                    UserName = "JaneSmith",
                    Email = "jane.smith@example.com",
                    Phone = "098-765-4321",
                    Role = RoleEnum.Admin,
                    Password = "cTv9p4hwv50bJh9WUob4XpfuYU7+Xw+vfDTnyk9luso=" // adminpass
                },
                new User
                {
                    ID = 3,
                    UserName = "StaffUser",
                    Email = "staff@example.com",
                    Phone = "111-222-3333",
                    Role = RoleEnum.Staff,
                    Password = "R1Q4JO1IZ720I+SMoKaPJMoLeiVOkfP6iCLFlLtusxg=" // staffpass (example hash)
                }
            );

            // Seed initial products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    ProductName = "Apple",
                    Brand = "FreshFarms",
                    Category = CategoryEnum.Fruits,
                    Price = 1.99,
                    Quantity = 100
                },
                new Product
                {
                    ID = 2,
                    ProductName = "Milk",
                    Brand = "TropicalCo",
                    Category = CategoryEnum.Dairy,
                    Price = 0.99,
                    Quantity = 50
                },
                new Product
                {
                    ID = 3,
                    ProductName = "Juice",
                    Brand = "BerryBest",
                    Category = CategoryEnum.Drinks,
                    Price = 2.99,
                    Quantity = 75
                }
            );
        }
    }
} 