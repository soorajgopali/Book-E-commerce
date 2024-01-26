using Bulky.DA.Data;
using Bulky.DA.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.Models;
using Bulky.Models.Models.ViewModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDBContext _db;
        public ProductRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

       /* public List<ProductListResponse> GetProductList()
        {*/
           /* try
            {
                var pList = (from product in _db.Products
                                  join category in _db.Categories on product.CategoryId equals category.Id
                                  select new ProductListResponse
                                  {
                                      ProductId = product.Id,
                                      ProductName = product.Title,
                                      CategoryName = category.Name
                                  }).ToList();

                var productList = _db.Products.Include(i => i.Category).Select(s => new ProductListResponse
                    {
                        ProductId = s.Id,
                        ProductName = s.Title,
                        CategoryName = s.Category.Name
                    }).ToList();

                string sql = @"SELECT P.Id AS ProductId, P.Title AS ProductName, C.Name AS CategoryName
                    FROM DBO.Products P
                    INNER JOIN DBO.Categories C ON C.Id = P.CategoryId
                ";

                var response = _db.Database.SqlQueryRaw<ProductListResponse>(sql).ToList();

                return response;
            }
            catch (Exception)
            {

                throw;*/
          /*  }
        }*/


    }
}

