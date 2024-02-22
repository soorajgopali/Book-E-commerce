using Bulky.DA.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.Models;
using Bulky.Models.Models.ViewModel;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace bulkywebco.Areas.Customer.Controllers
{
        [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _repo;
        [BindProperty]
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
                includeproperties: "Product"),
                OrderHeader = new()
            };



            foreach (var cart in Cart.ShoppingCartList)
            {
                   cart.Price = GetPriceBasedOnQuantity(cart);   
                Cart.OrderHeader.OrderTotal += (cart.Price * cart.Count);  
            }
            return View(Cart);
            }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            Cart = new()
            {
                ShoppingCartList = _repo.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeproperties: "Product"),
                OrderHeader = new()
            };

            Cart.OrderHeader.ApplicationUser = _repo.User.Get(u => u.Id == userId);
            Cart.OrderHeader.Name = Cart.OrderHeader.ApplicationUser.Name;
            Cart.OrderHeader.PhoneNumber = Cart.OrderHeader.ApplicationUser.PhoneNumber;
            Cart.OrderHeader.StreetAddress = Cart.OrderHeader.ApplicationUser.StreetAddress;
            Cart.OrderHeader.City = Cart.OrderHeader.ApplicationUser.City;
            Cart.OrderHeader.State = Cart.OrderHeader.ApplicationUser.State;
            Cart.OrderHeader.PostalCode= Cart.OrderHeader.ApplicationUser.PostalCode;




            foreach (var cart in Cart.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                Cart.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(Cart);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            Cart.ShoppingCartList = _repo.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
            includeproperties: "Product");

            Cart.OrderHeader.OrderDate = System.DateTime.Now;
            Cart.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _repo.User.Get(u=>u.Id==userId);

            foreach (var cart in Cart.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                Cart.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault()==0) 
            {
                Cart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
                Cart.OrderHeader.OrderStatus = SD.StatusPending;
            }
            else
            {
                Cart.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                Cart.OrderHeader.OrderStatus = SD.StatusApproved;
            }
            _repo.OrderHeader.Add(Cart.OrderHeader);
            _repo.Save();
           
            foreach(var cart in Cart.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = Cart.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _repo.OrderDetails.Add(orderDetail);
                _repo.Save();
            }
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //capture payment
            }
            return RedirectToAction(nameof(OrderConfirmation), new {id=Cart.OrderHeader.Id});
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
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
