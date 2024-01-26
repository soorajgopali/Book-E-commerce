using Bulky.DA.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace bulkywebco.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller

    {
        private readonly IUnitOfWork _Repo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _Repo = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _Repo.Product.GetAll(includeproperties : "Category");
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new() {
                Product = _Repo.Product.Get(u => u.Id == productId, includeproperties: "Category"),
                Count = 1,
                ProductId = productId

        };
                return View(cart);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
           var claimsIdentity =(ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            ShoppingCart cart = _Repo.ShoppingCart.Get(u => u.ApplicationUserId == userId &&
            u.ProductId == shoppingCart.ProductId);

            if (cart != null)
            {
                cart.Count = shoppingCart.Count;
                _Repo.ShoppingCart.Update(cart);
            }
            else
            {
            _Repo.ShoppingCart.Add(shoppingCart);

            }

            _Repo.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
