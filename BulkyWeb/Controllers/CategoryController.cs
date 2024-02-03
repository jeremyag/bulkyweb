using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;
        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            var objCategoryList = _categoryRepo.GetAll().ToList();
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
                _categoryRepo.Add(obj);
                _categoryRepo.Save();

                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();            
        }
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0) 
            {
                return NotFound();
            }

            var objCategory = _categoryRepo.Get(u => u.Id == id);
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
                _categoryRepo.Update(obj);
                _categoryRepo.Save();

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

            var objCategory = _categoryRepo.Get(u => u.Id == id);
            if (objCategory == null)
            {
                return NotFound();
            }


            return View(objCategory);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            var objCategory = _categoryRepo.Get(u => u.Id == id);
            if(objCategory == null) { return NotFound(); }

            _categoryRepo.Remove(objCategory);
            _categoryRepo.Save();

            TempData["Success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
