using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DA
{
    public class CategoryDataAccess:GenericDataAccess<Category>
    {
        public Category GetByName(String Name)
        {
            Category category = GetAll().Where(a => a.Name == Name && a.IsDeleted == false).FirstOrDefault();
            return category;
        }
        public List<Category> GetAllExceptDeleted()
        {
            return GetAll().Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
        }
        
        public List<Category> GetCategoriesForSearch(String searchString)
        {
            return GetAll().Where(a => a.IsDeleted == false && a.Name.ToLower().Contains(searchString)).OrderBy(a => a.Name).ToList();
        }
    }
}
