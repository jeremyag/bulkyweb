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

        public IActionResult Upsert(int? id)
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

            if(id == null || id == 0)
            {
                return View(productVm);
            }
            else
            {
                // Update
                productVm.Product = _unitOfWork.Products.Get(u => u.Id == id);
                return View(productVm);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVm, IFormFile? file)
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
