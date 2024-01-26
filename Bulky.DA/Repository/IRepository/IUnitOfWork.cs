using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository User { get; }
        void Save();    
    }
}
