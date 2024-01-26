using Bulky.Models;
using Bulky.Models.Models;
using Bulky.Models.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product> 
    {
        /*List<ProductListResponse> GetProductList();*/
    }
}
