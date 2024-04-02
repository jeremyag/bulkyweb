namespace Bulky.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    ICompanyRepository Companies { get; }
    void Save();
}
