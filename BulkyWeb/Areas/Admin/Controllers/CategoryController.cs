using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.RoleAdmin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var objCategoryList = _unitOfWork.Categories.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Add(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var objCategory = _unitOfWork.Categories.Get(u => u.Id == id);
            if (objCategory == null)
            {
                return NotFound();
            }


            return View(objCategory);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(obj);
                _unitOfWork.Save();

                TempData["Success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var objCategory = _unitOfWork.Categories.Get(u => u.Id == id);
            if (objCategory == null)
            {
                return NotFound();
            }


            return View(objCategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var objCategory = _unitOfWork.Categories.Get(u => u.Id == id);
            if (objCategory == null) { return NotFound(); }

            _unitOfWork.Categories.Remove(objCategory);
            _unitOfWork.Save();

            TempData["Success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
