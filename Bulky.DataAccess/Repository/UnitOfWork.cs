using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.Contracts;

namespace Bulky.DataAccess.Repository;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;

    private ICategoryRepository Category { get; }
    private IProductRepository Product { get; }
    private ICompanyRepository Company { get; }
    private IShoppingCartRepository ShoppingCart { get; }
    private IApplicationUserRepository ApplicationUser { get; }

    public UnitOfWork(ApplicationDbContext dbContext,
        ICategoryRepository categoryRepository,
        IProductRepository productRepository,
        ICompanyRepository companyRepository,
        IShoppingCartRepository shoppingCart,
        IApplicationUserRepository applicationUser)
    {
        _dbContext = dbContext;
        Category = categoryRepository;
        Product = productRepository;
        Company = companyRepository;
        ShoppingCart = shoppingCart;
        ApplicationUser = applicationUser;
    }

    public ICategoryRepository CategoryRepository => Category;
    public IProductRepository ProductRepository => Product;
    public ICompanyRepository CompanyRepository => Company;
    public IShoppingCartRepository ShoppingCartRepository => ShoppingCart;
    public IApplicationUserRepository ApplicationUserRepository => ApplicationUser;

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
