using BookStore.DataAccess.Repository.Contracts;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _unitOfWork.CategoryRepository.GetAll();

            return View(categories);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOder.ToString())
            {
                //ModelState.AddModelError("CustomError", "Category Name and DisplayName cannot exaxtly match!");
                ModelState.AddModelError("Name", "Category Name and DisplayName cannot exaxtly match!");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category successfully created.";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (category.Name == category.DisplayOder.ToString())
            {
                ModelState.AddModelError("Name", "Category Name and DisplayName cannot exaxtly match!");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category successfully updated.";
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var categoryFromDb = _unitOfWork.CategoryRepository.GetFirstOrDefault(c => c.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.CategoryRepository.Remove(categoryFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Category successfully deleted.";
            return RedirectToAction("Index");
        }
    }
}
