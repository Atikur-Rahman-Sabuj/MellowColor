using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Entity
{
    [Table("Orders")]
    public class Order : BaseEntity
    {
        public string Number { get; set; }
        public DateTime Date { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public long TotalItem { get; set; }
        public decimal TotalPrice { get; set; }
        [NotMapped]
        public string PaymentType { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
