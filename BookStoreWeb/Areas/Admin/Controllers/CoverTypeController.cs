using BookStore.DataAccess.Repository.Contracts;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> coverTypes = _unitOfWork.CoverTypeRepository.GetAll();

            return View(coverTypes);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypeRepository.Add(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover type successfully created.";
                return RedirectToAction("Index");
            }

            return View(coverType);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverTypeFromDb = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypeRepository.Update(coverType);
                _unitOfWork.Save();
                TempData["success"] = "Cover type successfully updated.";
                return RedirectToAction("Index");
            }

            return View(coverType);
        }

        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverTypeFromDb = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDb);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var coverTypeFromDb = _unitOfWork.CoverTypeRepository.GetFirstOrDefault(c => c.Id == id);

            if (coverTypeFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverTypeRepository.Remove(coverTypeFromDb);
            _unitOfWork.Save();
            TempData["success"] = "Cover type successfully deleted.";
            return RedirectToAction("Index");
        }
    }
}
