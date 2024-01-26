using Bulky.DA.Data;
using Bulky.DA.Repository.IRepository;
using Bulky.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DA.Repository
{
    public class CompanyRepository : Repository<Company> ,ICompanyRepository
    {
        private ApplicationDBContext _db;

        public CompanyRepository(ApplicationDBContext db) :base(db)
        {
            _db = db;
        }  
      
    }
}
