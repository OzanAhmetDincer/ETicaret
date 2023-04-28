using ETicaret.Data.Abstract;

namespace ETicaret.Data.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }
        private BrandRepository _brandRepository;
        private CartRepository _cartRepository;
        private CategoryRepository _categoryRepository;
        private ContactRepository _contactRepository;
        private OrderRepository _orderRepository;
        private ProductRepository _productRepository;
        private SliderRepository _sliderRepository;

        // BrandRepository'i alırken geriye bir _brandRepository göndericez. Eğer _brandRepository varsa direkt o gelecek ancak yoksa yani null ise BrandRepository türünde _context ile yeni bir nesne üretilip o gönderilir. Kısaca UnitOfWork mantığını kullanmadan önce olduğu gibi Program.cs de tanımladığımız gibi IBrandRepository istendiğinde BrandRepository bize gönderilecek     
        public IBrandRepository Brands => _brandRepository = _brandRepository ?? new BrandRepository(_context);

        public ICartRepository Carts => _cartRepository = _cartRepository ?? new CartRepository(_context);

        public ICategoryRepository Categories => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IContactRepository Contacts => _contactRepository = _contactRepository ?? new ContactRepository(_context);

        public IOrderRepository Orders => _orderRepository = _orderRepository ?? new OrderRepository(_context); 

        public IProductRepository Products => _productRepository = _productRepository ?? new ProductRepository(_context);

        public ISliderRepository Sliders => _sliderRepository = _sliderRepository ?? new SliderRepository(_context);
        public void Dispose()
        {
            _context.Dispose();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
