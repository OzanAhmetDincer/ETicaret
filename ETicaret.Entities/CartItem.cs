namespace ETicaret.Entities
{
    public class CartItem : IEntity
    {
        //CartItem ==> Her bir cart içerisinde ürün detay bilgilerine ulaşmak için "CartItem" oluştururuz. Çünkü bir cart içerisinde birden fazla ürün eklenebilir ve eklenen her bir üründe CartItem içerisindeki tek bir kayıta karşılık geliyor.
        public int Id { get; set; }// birincil anahtar
        public int ProductId { get; set; }
        public Product Product { get; set; }// ilgili "ProductId", "Product" tablosunda hangi kayıta karşılık geliyor ve her bir product'ın bilgilerine erişim için yazdık(navigation property)
        public int CartId { get; set; }// Her bir CartItem'ın neye ait olduğunu yani hangi CartId' ye ait olduğunun bilgisini saklamamız gerek. Örneğin beş numaralı kart belli bir user'ın ve o kart altında da  hangi ürünler var, her bir ürün için CartItem olması gerekir.
        public Cart Cart { get; set; }//Hangi "CartId" hangi obje ile ilişkilendiriliyor. Bu bilgiyi direkt alabilmek için ekledik (Navigation property)
        public int Quantity { get; set; }// alınan ürünlerin miktarı

    }
}
