using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Entity
{
    [Table("Products")]
    public class Product : BaseEntity
    {
        [Required(ErrorMessage="Please input a Bar Code")]
        public String BarCode { get; set; }
        [Required(ErrorMessage="Please input a Name")]
        public String Name { get; set; }
        public String Size { get; set; }
        public String Color { get; set; }
        [Required(ErrorMessage="Please input stock")]
        public long Stock { get; set; }
        [Required(ErrorMessage = "Please input Buying Price")]
        public Decimal BuyingPrice { get; set; }
        [Required(ErrorMessage="Please input selling price")]
        public Decimal SellingPrice { get; set; }
        [Required(ErrorMessage="Please select a Category")]
        public long CategoryId { get; set; }
        [Required(ErrorMessage="Please select a Brand")]
        public long BrandId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
        [NotMapped]
        public String CategoryName { get; set; }
        [NotMapped]
        public String BrandName { get; set; }
        [NotMapped]
        public long Total { get; set; }
        [NotMapped]
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public string DiscountType { get; set; }
        [NotMapped]
        public decimal DiscountAmount { get; set; }
        

    }
}
