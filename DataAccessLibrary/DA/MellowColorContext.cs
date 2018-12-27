using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using ModelLibrary;
using ModelLibrary.Entity;

namespace DataAccessLibrary.DA
{
    public class MellowColorContext:DbContext
    {
        public MellowColorContext()
            : base("name=MellowColorContext")
        {
            this.Configuration.LazyLoadingEnabled = true;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
