using Azure;
using Bulky.DA.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.Models.ViewModel;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;


namespace bulkywebco.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles =SD.Role_Admin)]*/
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _Repo;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public ProductController(IUnitOfWork dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _Repo = dbContext;
            _WebHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            //List<Product> productList = _Repo.Product.GetAll(includeproperties: "Category").ToList();
            return View();
        }

        public IActionResult Upsert(int? id)
        {

            ProductVM productvm = new()
            {
                CategoryList = _Repo.Category
                .GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                }),
                Product = new Product()
            };
            if (id == null || id == 0)
            {
                return View(productvm);
            }
            else
            {
                productvm.Product = _Repo.Product.Get(u => u.Id == id);
                return View(productvm);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _WebHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Images\Product");

                    if (!string.IsNullOrEmpty(obj.Product.ImageUrl))
                    {
                        //delete the existing image
                        var oldimage = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldimage))
                        {
                            System.IO.File.Delete(oldimage);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.Product.ImageUrl = @"\Images\Product\" + filename;
                }
                if (obj.Product.Id == 0)
                {
                    _Repo.Product.Add(obj.Product);
                }
                else
                {
                    _Repo.Product.Update(obj.Product);
                }

                _Repo.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {

                obj.CategoryList = _Repo.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),
                });

                return View(obj);
            }
        }

        /*public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _Repo.Product.GetAll().Select(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Update(Product obj)
        {
            if (ModelState.IsValid)
            {
                _Repo.Product.Update(obj);
                _Repo.Save();
                TempData["success"] = "Product Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }*/

       /* public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _Repo.Product.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Product obj)
        {


            if (obj == null)
            {
                return NotFound();
            }

            _Repo.Product.Remove(obj);
            _Repo.Save();

            TempData["success"] = "Product Deleted Successfully";
            return RedirectToAction("Index");
        }*/

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> productList = _Repo.Product.GetAll(includeproperties: "Category").ToList();
            return Json(new { data = productList });
        } 
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _Repo.Product.Get(u=>u.Id== id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

            var oldimage = Path.Combine(_WebHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldimage))
            {
                System.IO.File.Delete(oldimage);
            }

                _Repo.Product.Remove(productToBeDeleted);
                _Repo.Save();
            return Json(new { success = true, message = "Successfully Deleted!!" });

        }
        #endregion
    }
}


