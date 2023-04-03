namespace ETicaret.WebUI.Models
{
    public class CartModel
    {
        public int CartId { get; set; }
        public List<CartItemModel> CartItems { get; set; }// Her bir cart içerisinde de ilişkili olan bir liste olması için ekledik 

        public double TotalPrice()
        {
            return CartItems.Sum(i => i.Price * i.Quantity);
        }
    }

    public class CartItemModel
    {
        // Hangi bilgileri cart içerisinde göstericeksek tanımlamalrını yapabiliriz
        public int CartItemId { get; set; }// cartItem ıd bilgisi
        public int ProductId { get; set; }// product ıd bilgisi
        public string Name { get; set; }// product ismi bilgisi
        public double Price { get; set; }// product fiyat bilgisi
        public string ImageUrl { get; set; }// product resim bilgisi
        public int Quantity { get; set; }// product miktar bilgisi
    }
}
