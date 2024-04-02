using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("customer")]
[Authorize]
public class CartController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public ShoppingCartVM ShoppingCartVM { get; set; }
    public CartController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        ShoppingCartVM = new ShoppingCartVM()
        {
            ShoppingCartList = _unitOfWork.ShoppingCarts.GetAll(c => c.ApplicationUserId == userId, 
            includeProperties: "Product")
        };

        foreach(var cart in ShoppingCartVM.ShoppingCartList)
        {
            cart.Price = GetPriceBasedOnQuantity(cart);
            ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
        }

        return View(ShoppingCartVM);
    }

    public IActionResult Plus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCarts.Get(c => c.Id == cartId);
        cart.Count++;
        _unitOfWork.ShoppingCarts.Update(cart);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Minus(int cartId)
    {
        var cart = _unitOfWork.ShoppingCarts.Get(c => c.Id == cartId);
        if(cart.Count <= 1)
        {
            _unitOfWork.ShoppingCarts.Remove(cart);
        }
        else
        {
            cart.Count--;
            _unitOfWork.ShoppingCarts.Update(cart);
        }

        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Remove(int cartId)
    {
        var cart = _unitOfWork.ShoppingCarts.Get(c => c.Id == cartId);
        _unitOfWork.ShoppingCarts.Remove(cart);
        _unitOfWork.Save();
        return RedirectToAction(nameof(Index));
    }

    private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
    {
        if(shoppingCart.Count <= 50)
        {
            return shoppingCart.Product.Price;
        }
        else
        {
            if(shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
    }
}
