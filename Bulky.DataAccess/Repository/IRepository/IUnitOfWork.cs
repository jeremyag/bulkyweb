﻿namespace Bulky.DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    ICompanyRepository Companies { get; }
    IShoppingCartRepository ShoppingCarts { get; }
    IApplicationUserRepository ApplicationUsers { get; }
    void Save();
}
