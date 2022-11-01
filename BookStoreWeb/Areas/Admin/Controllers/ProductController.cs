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
            return View();
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

                productVM.Product = productFromDb;

                return View(productVM);
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

                    if (productVM.Product.ImageUrl != null)
                    {
                        string oldFilePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\products\" + fileName + fileExtension;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.ProductRepository.Add(productVM.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product successfully created.";
                }
                else
                {
                    _unitOfWork.ProductRepository.Update(productVM.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product successfully updated.";
                }

                return RedirectToAction("Index");
            }

            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _unitOfWork.ProductRepository.GetAll(includeProperties: "Category,CoverType");

            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productFromDb = _unitOfWork.ProductRepository.GetFirstOrDefault(c => c.Id == id);

            if (productFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting product!" });
            }

            if (productFromDb.ImageUrl != null)
            {
                string oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, productFromDb.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            _unitOfWork.ProductRepository.Remove(productFromDb);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Product successfully deleted." });
        }

        #endregion
    }
}
