using Bulky.DA.Repository.IRepository;
using Bulky.Models.Models;
using Bulky.Models.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace bulkywebco.Areas.Customer.Controllers
{
        [Area("Customer")]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _repo;
        public ShoppingCartVM Cart { get; set; }

        public CartController(IUnitOfWork repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            Cart = new()
            {
                ShoppingCartList = _repo.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeproperties: "Product")
            };

            foreach (var cart in Cart.ShoppingCartList)
            {
                   cart.Price = GetPriceBasedOnQuantity(cart);   
                Cart.OrderTotal += (cart.Price * cart.Count);  
            }
            return View(Cart);
            }

        public IActionResult Summary()
        {
            return View();
        }

        public IActionResult Plus(int cartId)
        {
            var CartFromDb = _repo.ShoppingCart.Get(u=>u.Id==cartId);
            CartFromDb.Count += 1;
            _repo.ShoppingCart.Update(CartFromDb);
            _repo.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var CartFromDb = _repo.ShoppingCart.Get(u => u.Id == cartId);
            if(CartFromDb.Count <=1) {
                _repo.ShoppingCart.Remove(CartFromDb);
            }
            else
            {
            CartFromDb.Count -= 1;
            _repo.ShoppingCart.Update(CartFromDb);

            }
            _repo.Save();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Remove(int cartId)
        {
            var CartFromDb = _repo.ShoppingCart.Get(u => u.Id == cartId);

            if (CartFromDb == null)
            {
                return NotFound();
            }
            _repo.ShoppingCart.Remove(CartFromDb);
            _repo.Save();
            return RedirectToAction(nameof(Index));
        }

        private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if(shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if(shoppingCart.Count <= 100){
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }

    }
}
