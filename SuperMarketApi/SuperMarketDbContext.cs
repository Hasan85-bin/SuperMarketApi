using Microsoft.EntityFrameworkCore;
using SuperMarketApi.Models;

namespace SuperMarketApi
{
    public class SuperMarketDbContext : DbContext
    {
        public SuperMarketDbContext(DbContextOptions<SuperMarketDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

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

            // User-Purchase: one-to-many, cascade delete
            modelBuilder.Entity<Purchase>()
                .HasOne(p => p.User)
                .WithMany(u => u.Purchases)
                .HasForeignKey(p => p.UserID)
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
                    Password = "ef92b778bafe771e89245b89ecbc08a44a4e166c06659911881f383d4473e94f" // password123
                },
                new User
                {
                    ID = 2,
                    UserName = "JaneSmith",
                    Email = "jane.smith@example.com",
                    Phone = "098-765-4321",
                    Role = RoleEnum.Admin,
                    Password = "d033e22ae348aeb5660fc2140aec35850c4da997" // adminpass
                },
                new User
                {
                    ID = 3,
                    UserName = "StaffUser",
                    Email = "staff@example.com",
                    Phone = "111-222-3333",
                    Role = RoleEnum.Staff,
                    Password = "b362b9b8b6e9e5e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2e6e2" // staffpass (example hash)
                }
            );

            // Seed initial products
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ID = 1,
                    name = "Apple",
                    brand = "FreshFarms",
                    category = CategoryEnum.Fruits,
                    price = 1.99
                },
                new Product
                {
                    ID = 2,
                    name = "Milk",
                    brand = "TropicalCo",
                    category = CategoryEnum.Dairy,
                    price = 0.99
                },
                new Product
                {
                    ID = 3,
                    name = "Juice",
                    brand = "BerryBest",
                    category = CategoryEnum.Drinks,
                    price = 2.99
                }
            );
        }
    }
} 