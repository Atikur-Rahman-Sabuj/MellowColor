using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DA
{
    public class BrandDataAccess:GenericDataAccess<Brand>
    {
        public Brand GetByName(String Name)
        {
            Brand brand = GetAll().Where(a => a.Name == Name && a.IsDeleted == false).FirstOrDefault();
            return brand;
        }
        public List<Brand> GetAllExceptDeleted()
        {
            return GetAll().Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
        }
        public List<Brand> GetBrandsForSearch(String searchString)
        {
            return GetAll().Where(a => a.IsDeleted == false && a.Name.ToLower().Contains(searchString)).OrderBy(a => a.Name).ToList();
        }
    }
}
