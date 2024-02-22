using Bulky.DA.Data;
using Bulky.DA.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDBContext _db;
        public ICategoryRepository Category {  get; set; }
        public IProductRepository Product {  get; set; }
        public ICompanyRepository Company { get; set; }
        public IShoppingCartRepository ShoppingCart { get; set; }
        public IApplicationUserRepository User{ get; set; }
        public IOrderHeaderRepository OrderHeader { get; set; }
        public IOrderDetailsRepository OrderDetails { get; set; }



        public UnitOfWork(ApplicationDBContext db) 
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            User = new ApplicationUserRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
