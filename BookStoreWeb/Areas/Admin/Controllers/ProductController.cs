using BookStore.DataAccess.Repository.Contracts;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreWeb.Areas.Admin.Controllers
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
            IEnumerable<Product> products = _unitOfWork.ProductRepository.GetAll();

            return View(products);
        }

        //GET
        public IActionResult Upsert(int? id)
        {
            var productVM = new ProductViewModel()
            {
                Product = new(),
                CategoryList = _unitOfWork.CategoryRepository.GetAll().Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                }),
                CoverTypeList = _unitOfWork.CoverTypeRepository.GetAll().Select(x =>
                {
                    return new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    };
                })
            };

            if (id == null || id == 0)
            {
                //Product product = new();
                //ViewBag.categoryList = categoryList;
                //ViewData["coverTypeList"] = coverTypeList;

                return View(productVM);
            }
            else
            {
                var productFromDb = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);

                if (productFromDb == null)
                {
                    return NotFound();
                }

                return View(productFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ProductRepository.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "Product successfully updated.";
                return RedirectToAction("Index");
            }

            return View(product);
        }
    }
}
