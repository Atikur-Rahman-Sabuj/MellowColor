using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.Entity
{
    [Table("Users")]
    public class User : BaseEntity
    {
        [Required(ErrorMessage="Please enter name")]
        public String Name { get; set; }
        [Required(ErrorMessage="Please enter a user name")]
        public String UserName { get; set; }
        [Required(ErrorMessage="Please enter a password")]
       
        public String Password { get; set; }
        [NotMapped]
        [Compare("Password", ErrorMessage = "Password and Confirm Password did not match")]
        public String ConfirmPassword { get; set; }
        public String Type { get; set; }
    }
}
