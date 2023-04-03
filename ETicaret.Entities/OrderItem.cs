namespace ETicaret.Entities
{
    public class OrderItem : IEntity
    {
        public int Id { get; set; }// Herbir OrderItem'ın Id'si
        public int OrderId { get; set; }// Hangi order'a yani siparişe ait olduğunu bulmak için cartitem'da tanımladığımız gibi
        public Order Order { get; set; }// Order(sipariş) detaylarını görebilmek için navigation property tanımladık
        public int ProductId { get; set; }
        public Product Product { get; set; }// OrderItem bir ürünü temsil eden bir kayıt ürün bilgilerine Product navigation property'si üzerinden ulaşırız
        public double Price { get; set; }// Fiyat bilgisine ürünün o anki id'si üzerinden gitmemek gerekir. Siparişi verdiğimiz andaki fiyatını almak için fiyat bilgisi tanımladık. Buna Product üzerinden ulaşmak doğru olmaz. O an belki fiyat artabilir. 
        public int Quantity { get; set; }// Üründen kaç tane aldığı
    }
}
