using Bulky.DA.Data;
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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _Repo;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _Repo = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companyList = _Repo.Company.GetAll().ToList(); 
            return View(companyList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id==null | id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company company = _Repo.Company.Get(u=>u.Id==id);
                return View(company);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if(ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _Repo.Company.Add(company);
                }
                else
                {
                    _Repo.Company.Update(company);
                }
                _Repo.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            return View(company);
        }


         #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companyList = _Repo.Company.GetAll().ToList();
            return Json(new { data = companyList });
        } 
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _Repo.Company.Get(u=>u.Id== id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

                _Repo.Company.Remove(productToBeDeleted);
                _Repo.Save();
            return Json(new { success = true, message = "Successfully Deleted!!" });

        }
        #endregion
    }
}
