using DataAccessLibrary.DA;
using MellowColorUI.Utility;
using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MellowColorUI
{
    public partial class RegisterUser : Form
    {
        User user;
        public RegisterUser(User user)
        {
            InitializeComponent();
            this.user = user;
            lblUserName.Text = user.Name;
            Utilities.SetFullScreen(this);
            TimerUpdate();

        }

        async void TimerUpdate()
        {
            while (true)
            {
                lblTime.Text = DateTime.Now.ToString("dd MMM yy HH:mm tt");
                await Task.Delay(1000);
            }
        }
        private void RegisterUser_Load(object sender, EventArgs e)
        {
            //setFullScreen();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if(new UserDataAccess().GetByUserName(tbxRegisterUserName.Text) != null)
            {
                lblMessage.Text = "Could not register, User Name already exists.";
                return;
            }
            User newUser = new User()
            {
                Name = tbxRegisterName.Text,
                UserName = tbxRegisterUserName.Text,
                Password = tbxRegisterPassword.Text,
                ConfirmPassword = tbxRegisterConfPassword.Text,
            };
            #region SetUserType
            if (user.Type == "Developer")
            {
                user.Type = "SuperAdmin";
            }
            else if (user.Type == "SuperAdmin")
            {
                newUser.Type = "Admin";
            }
            else if (user.Type == "Admin")
            {
                newUser.Type = "Seller";
            }
            else
            {
                lblMessage.Text = "Could not register try again!";
                return;
            }
            #endregion
            var validationContext = new ValidationContext(newUser, null, null);
            var results = new List<ValidationResult>();


            if (Validator.TryValidateObject(newUser, validationContext, results, true))
            {
                UserDataAccess userDataAccess = new UserDataAccess();
                if(userDataAccess.Register(newUser))
                {
                    ResetFormControls();
                    lblMessage.Text = "Registered Successfully, Please login!";
                }
                else
                {
                    lblMessage.Text = "Could not register, Try again!";
                }
            }
            else
            {
                String errorMessage = "Errors:\n";
                results.ForEach(a => errorMessage = errorMessage + a.ErrorMessage + "\n");
                lblError.Text = errorMessage;     
            }
        }

        private void btnResetRegister_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            tbxRegisterName.Text = "";
            tbxRegisterUserName.Text = "";
            tbxRegisterPassword.Text = "";
            tbxRegisterConfPassword.Text = "";
            lblError.Text = "";
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Utilities.Logout(this);
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Home(user));
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Sell(user));
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Product(user));
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Category(user));
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Brand(user));
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }
    }
}
