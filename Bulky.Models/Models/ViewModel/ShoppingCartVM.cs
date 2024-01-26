using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.Models.Models.ViewModel
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> ShoppingCartList{ get; set; }
        public double OrderTotal{ get; set; }

       /* public ShoppingCart shoppingCart { get; set; }  */
    }
}

