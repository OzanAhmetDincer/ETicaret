using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Data.Configurations
{
    public static class ModelBuilderExtensions
    {
        // Burada ModelBuilder metodunu genişlettik ve içerisinde kayıtlı olmasını istediğimiz test verileri ekledik. Aşağıdaki her Entity için tanımladığımız verileri kendi Configuration class'ı içerisinde tanımla yani category entity'sini CategoryConfifuration içerisinde yada aşağıda olduğu gibi tüm tanımlamaları tek bir yerde yapabiliriz
        public static void Seed(this ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Kategori 1", Image = "circled-1.png", IsActive = true, IsTopMenu = true },
                new Category() { Id = 2, Name = "Kategori 2", Image = "circled-2.png", IsActive = true, IsTopMenu = true },
                new Category() { Id = 3, Name = "Kategori 3", Image = "circled-3.png", IsActive = true, IsTopMenu = true },
                new Category() { Id = 4, Name = "Kategori 4", Image = "circled-4.png", IsActive = true, IsTopMenu = false },
                new Category() { Id = 5, Name = "Kategori 5", Image = "circled-5.png", IsActive = true, IsTopMenu = false },
                new Category() { Id = 6, Name = "Kategori 6", Image = "circled-6.png", IsActive = true, IsTopMenu = false }
                );
            builder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Ürün 1", Image = "1-key.png", Description = "Ürün 1", Price = 1, Stock = 1, CategoryId = 1, BrandId = 1, IsHome = true },
                new Product() { Id = 2, Name = "Ürün 2", Image = "2-key.png", Description = "Ürün 2", Price = 2, Stock = 2, CategoryId = 2, BrandId = 2, IsHome = true },
                new Product() { Id = 3, Name = "Ürün 3", Image = "3-key.png", Description = "Ürün 3", Price = 3, Stock = 3, CategoryId = 3, BrandId = 3, IsHome = true },
                new Product() { Id = 4, Name = "Ürün 4", Image = "4-key.png", Description = "Ürün 4", Price = 4, Stock = 4, CategoryId = 4, BrandId = 1, IsHome = true },
                new Product() { Id = 5, Name = "Ürün 5", Image = "5-key.png", Description = "Ürün 5", Price = 5, Stock = 5, CategoryId = 5, BrandId = 2, IsHome = true },
                new Product() { Id = 6, Name = "Ürün 6", Image = "6-key.png", Description = "Ürün 6", Price = 6, Stock = 6, CategoryId = 6, BrandId = 3, IsHome = true }
                );
            builder.Entity<Brand>().HasData(
                new Brand() { Id = 1, Name = "Marka 1", Description = "Marka 1", Logo = "1-Brand.png", IsActive = true },
                new Brand() { Id = 2, Name = "Marka 2", Description = "Marka 2", Logo = "2-Brand.png", IsActive = true },
                new Brand() { Id = 3, Name = "Marka 3", Description = "Marka 3", Logo = "3-Brand.png", IsActive = true }
                );

        }
    }
}
