using Microsoft.EntityFrameworkCore;

namespace GroceryWebsite.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Cart> Carts { get; set; }
        public DbSet<Models.CartDetail> CartDetails { get; set; }
        public DbSet<Models.Product> Products { get; set; }
        public DbSet<Models.Category> Categories { get; set; }
        public DbSet<Models.Order> Orders { get; set; }
        public DbSet<Models.OrderDetail> OrderDetails { get; set; }


    }
    
}
