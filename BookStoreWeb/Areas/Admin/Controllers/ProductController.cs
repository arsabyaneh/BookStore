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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
        public IActionResult Upsert(ProductViewModel productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString();
                    string uploads = Path.Combine(wwwRootPath, @"images\products");
                    string fileExtension = Path.GetExtension(file.FileName);
                    string filePath = Path.Combine(uploads, fileName + fileExtension);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\products\" + fileName + fileExtension;
                }

                _unitOfWork.ProductRepository.Update(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "Product successfully updated.";
                return RedirectToAction("Index");
            }

            return View(productVM);
        }
    }
}
