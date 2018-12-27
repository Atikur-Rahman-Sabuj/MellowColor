using DataAccessLibrary.DA;
using MellowColorUI.Utility;
using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MellowColorUI
{
    public partial class LoginUser : Form
    {
        public LoginUser()
        {
            InitializeComponent();
            Utilities.SetFullScreen(this);
            Utilities.TimerUpdate(lblTime);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.MinimizeApplication(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            User user = new UserDataAccess().LoginValidation(tbxLoginUserName.Text, tbxLoginPassword.Text);
            if (user != null)
            {
                Home home = new Home(user);
                home.Show();
                this.Hide();
            }
            else
            {
                lblMessage.Text = "Invalid username or password !";
                lblMessage.Text = "Coutld not login";
            }
        }

        private void btnResetLogin_Click(object sender, EventArgs e)
        {
            tbxLoginUserName.Text = "";
            tbxLoginPassword.Text = "";
            lblMessage.Text = "";
        }
    }
}
