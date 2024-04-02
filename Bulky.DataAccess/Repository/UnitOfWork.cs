using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;

namespace Bulky.DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    public ICategoryRepository Categories { get; private set; }
    public IProductRepository Products { get; private set; }
    public ICompanyRepository Companies { get; private set; }
    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Categories = new CategoryRepository(_db);
        Products = new ProductRepository(_db);
        Companies = new CompanyRepository(_db);
    }
    public void Save()
    {
        _db.SaveChanges();
    }
}
