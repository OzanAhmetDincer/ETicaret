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
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; database=ETicaret; integrated security=true;");

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Phone = "",
                    Email = "admin@ETicaret.com",
                    IsActive = true,
                    IsAdmin = true,
                    Name = "admin",
                    SurName = "admin",
                    Password = "123"
                }
                );

            base.OnModelCreating(modelBuilder);

        }
    }
}

