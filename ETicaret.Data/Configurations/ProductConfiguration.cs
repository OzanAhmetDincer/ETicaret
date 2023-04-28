using ETicaret.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ETicaret.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        // Fluent Api kullanarak oluşturduğumuz entitylerin özellikleri için belirli şartlar tanımlayabiliriz. Bu tanımlamaları DataAnnotations ile de yaparız fakat proje büyüdükçe her heş karışır. Yapacağımız Fluent Validation'ları oluşturduğumuz configuration klasöründe tanımlayıp DatabaseContext içerisinde çağırırız.
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(m => m.Id);// Id bilgisi product tablosunun primarykey(birincil anahtar) olacak

            builder.Property(m => m.Name).IsRequired().HasMaxLength(100);// Property özelliği ile Product tablosundaki istenilen property'e geçeriz. Name bilgisi IsRequired yani zorunlu olacak ve HasMaxLength yani max uzunluğu 100 karakter olacak.

            builder.Property(m => m.CreateDate).HasDefaultValueSql("getdate()");  // mssql için getdate yapısı kullanılır. Eklenme tarihini doldurmayıp boş geçersek eklenme tarihi olarak o anki zamanı alır
            
            // builder.Property(m=>m.DateAdded).HasDefaultValueSql ("date('now')");   // sqlite
        }
    }
}
