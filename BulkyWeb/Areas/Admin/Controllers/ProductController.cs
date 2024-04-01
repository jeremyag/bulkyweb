using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.ROLE_ADMIN)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var productList = _unitOfWork.Products.GetAll(includeProperties: "Category");
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
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVm.Product.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath,  productVm.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVm.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVm.Product.Id == 0)
                {
                    _unitOfWork.Products.Add(productVm.Product);
                }
                else
                {
                    _unitOfWork.Products.Update(productVm.Product);
                }
                _unitOfWork.Save();

                TempData["Success"] = "Product successfully created";
                return RedirectToAction("Index");
            }
            else
            {
                productVm.CategoryList = _unitOfWork.Categories.GetAll(includeProperties: "Category").Select(u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                return View(productVm);
            }  
        }

        #region API CALLS
        [HttpGet] 
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Products.GetAll(includeProperties: "Category");
            return Json(new {data = productList});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var tobeDeleted = _unitOfWork.Products.Get(u => u.Id == id);
            if (tobeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, tobeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Products.Remove(tobeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
