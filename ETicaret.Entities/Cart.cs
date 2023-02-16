namespace ETicaret.Entities
{
    public class Cart : IEntity
    {
        public int Id { get; set; }
        public string UserId { get; set; }// Oluşturulan cart'ın(alışveriş sepeti) hangi kullanıcıya ait olduğunu tanımlamak için
        public List<CartItem> CartItems { get; set; }// "Cart" içerisine eklenen ürünlere gidebilmek için yazdık
    }
}
