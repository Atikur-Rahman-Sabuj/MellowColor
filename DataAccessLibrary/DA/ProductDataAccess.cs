using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DA
{
    public class ProductDataAccess:GenericDataAccess<Product>
    {
        public List<Product> GetAllExceptDeleted()
        {
            List<Product> products = GetAll().Where(a => a.IsDeleted == false).OrderBy(a => a.Name).ToList();
            products.ForEach(a =>
            {
                a.CategoryName = a.Category.Name;
                a.BrandName = a.Brand.Name;
            });
            return products;
        }
        
        public bool UpdateListOfProducts(List<Product> Products, long UserId)
        {
            bool saved = true;
            foreach (Product item in Products)
	        {
                saved = saved & Save(item, UserId);
	        }
            return saved;
        }
    }
}
