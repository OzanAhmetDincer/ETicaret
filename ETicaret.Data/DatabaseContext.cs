using ETicaret.Data.Configurations;
using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; database=ETicaret; integrated security=true;");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Veri tabanına eklemek istediğimiz bilgileri bu metot içerisine yazarız. Aşağıdaki örnek gibi

            modelBuilder.ApplyConfiguration(new ProductConfiguration());// Configuration klasörü içerisindeki ProductConfiguration'i burada tanımladık. ProductConfiguration içerisindeki özellikleri direk burayada yazabiliriz fakat DatabaseContext içerisi karmaşık olur.
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());

            modelBuilder.Seed();// ModelBuilderExtensions class'ında tanımdalığımız Seed metodunu çağırırız ve içerisindeki verileri veri tabanına ekleriz.

            base.OnModelCreating(modelBuilder);

        }
    }
}

