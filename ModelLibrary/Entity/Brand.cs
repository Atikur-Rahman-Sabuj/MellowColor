using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Entity
{
    [Table("Brands")]
    public class Brand:BaseEntity
    {
        [Required(ErrorMessage = "Please enter a name!")]
        public String Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        
    }
}
