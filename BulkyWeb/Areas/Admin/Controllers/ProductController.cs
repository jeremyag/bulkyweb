using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var productList = _unitOfWork.Products.GetAll();
            return View(productList);
        }

        public IActionResult Create()
        {
            var productVm = new ProductVM()
            {
                CategoryList = _unitOfWork.Categories.GetAll().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            return View(productVm);
        }
        [HttpPost]
        public IActionResult Create(ProductVM productVm)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.Products.Add(productVm.Product);
                _unitOfWork.Save();

                TempData["Success"] = "Product successfully created";
                return RedirectToAction("Index");
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Categories.GetAll().Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVm);
            }  
        }

        public IActionResult Edit(int? id) 
        { 
            if(id != null)
            {
                var product = _unitOfWork.Products.Get(p => p.Id == id);
                if(product != null)
                {
                    return View(product);
                }
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Products.Update(product);
                _unitOfWork.Save();
                TempData["Success"] = "Product successfully updated";

                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id != null)
            {
                var product = _unitOfWork.Products.Get(p =>p.Id == id);
                if(product != null)
                {
                    return View(product);
                }
            }

            return NotFound();
        }
        [HttpPost]
        public IActionResult Delete(Product product)
        {
            _unitOfWork.Products.Remove(product);
            _unitOfWork.Save();

            TempData["Success"] = "Successfully deleted";
            return RedirectToAction("Index");
        }
    }
}
