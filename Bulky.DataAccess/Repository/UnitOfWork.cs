using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;

namespace Bulky.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ICategoryRepository Categories { get; private set; }
    public IProductRepository Products { get; private set; }
    public ICompanyRepository Companies { get; private set; }
    public IShoppingCartRepository ShoppingCarts { get; private set; }
    public IApplicationUserRepository ApplicationUsers { get; private set; }
    public IOrderHeaderRepository OrderHeaders { get; private set; }
    public IOrderDetailRepository OrderDetails { get; private set; }
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        ApplicationUsers = new ApplicationUserRepository(_db);
        ShoppingCarts = new ShoppingCartRepository(_db);
        Categories = new CategoryRepository(_db);
        Products = new ProductRepository(_db);
        Companies = new CompanyRepository(_db);
        OrderHeaders = new OrderHeaderRepository(_db);
        OrderDetails = new OrderDetailRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}
