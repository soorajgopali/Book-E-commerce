using Bulky.DA.Data;
using Bulky.DA.Repository.IRepository;
using Bulky.Models;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace bulkywebco.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles =SD.Role_Admin)]*/
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _Repo;

        public CategoryController(IUnitOfWork dbContext)
        {
            _Repo = dbContext;
        }

        public IActionResult Index()
        {
            var objCategoryList = _Repo.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _Repo.Category.Add(obj);
                _Repo.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _Repo.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Update(Category obj)
        {
            if (ModelState.IsValid)
            {
                _Repo.Category.Update(obj);
                _Repo.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var categoryFromDb = _Repo.Category.Get(u => u.Id == id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Delete(Category obj)
        {


            if (obj == null)
            {
                return NotFound();
            }

            _Repo.Category.Remove(obj);
            _Repo.Save();

            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
