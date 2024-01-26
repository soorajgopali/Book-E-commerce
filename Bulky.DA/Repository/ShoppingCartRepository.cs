using Bulky.DA.Data;
using Bulky.DA.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDBContext _db;

        public ShoppingCartRepository(ApplicationDBContext db) : base(db) 
        {
            _db = db;
        }
    }
}
