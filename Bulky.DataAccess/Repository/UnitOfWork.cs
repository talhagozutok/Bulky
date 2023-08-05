using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;

namespace Bulky.DataAccess.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    private ICategoryRepository Category { get; }
    private IProductRepository Product { get; }
    private IProductImageRepository ProductImage { get; }
    private ICompanyRepository Company { get; }
    private IShoppingCartRepository ShoppingCart { get; }
    private IApplicationUserRepository ApplicationUser { get; }
    private IOrderHeaderRepository OrderHeader { get; }
    private IOrderDetailRepository OrderDetail { get; }

    public UnitOfWork(ApplicationDbContext dbContext,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        IProductImageRepository productImage,
        ICompanyRepository companyRepository,
        IShoppingCartRepository shoppingCart,
        IApplicationUserRepository applicationUser,
        IOrderHeaderRepository orderHeader,
        IOrderDetailRepository orderDetail)
        
    {
        _dbContext = dbContext;
        Category = categoryRepository;
        Product = productRepository;
        ProductImage = productImage;
        Company = companyRepository;
        ShoppingCart = shoppingCart;
        ApplicationUser = applicationUser;
        OrderHeader = orderHeader;
        OrderDetail = orderDetail;
    }

    public ICategoryRepository Categories => Category;
    public IProductRepository Products => Product;
    public ICompanyRepository Companies => Company;
    public IShoppingCartRepository ShoppingCarts => ShoppingCart;
    public IApplicationUserRepository ApplicationUsers => ApplicationUser;
    public IOrderDetailRepository OrderDetails => OrderDetail;
    public IOrderHeaderRepository OrderHeaders => OrderHeader;
    public IProductImageRepository ProductImages => ProductImage;

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
