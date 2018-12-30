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
    public partial class ManageUser : Form
    {
        User user;
        User GlobalUser;
        List<User> GlobalUsers;
        UserDataAccess userDataAccess;
        public ManageUser(User user)
        {
            InitializeComponent();
            this.user = user;
            lblUserName.Text = user.Name;
            Utilities.SetFullScreen(this);
            Utilities.TimerUpdate(lblTime);
            dataGridViewUsers.AutoGenerateColumns = false;
            userDataAccess = new UserDataAccess();
            GlobalUsers = userDataAccess.GetAll().Where(a => a.Type == "Admin" || a.Type == "Seller").ToList();
            Utilities.BindListToGridView<User>(GlobalUsers, dataGridViewUsers, lblMessage);
            
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.MinimizeApplication(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Utilities.Logout(this);
        }

        private void dataGridViewSearchResult_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewUsers.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridViewUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (dataGridViewUsers.Columns[e.ColumnIndex].Name == "btnChangePassword")
            {
                GlobalUser = GlobalUsers[e.RowIndex];
                tbxRegisterName.Text = GlobalUser.Name;
                tbxRegisterUserName.Text = GlobalUser.UserName;
            }
        }

        private void btnResetRegister_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

        private void ResetControls()
        {
            tbxRegisterName.ResetText();
            tbxRegisterUserName.ResetText();
            tbxRegisterPassword.ResetText();
            tbxRegisterConfPassword.ResetText();
            GlobalUser = null;
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (tbxRegisterPassword.Text=="")
            {
                lblMessage.Text = "Please input password!";
                return;
            }
            if (tbxRegisterPassword.Text!=tbxRegisterConfPassword.Text)
            {
                lblMessage.Text = "Password and confirm password did not match";
                return;
            }
            GlobalUser.Password = tbxRegisterPassword.Text;
            if (userDataAccess.Register(GlobalUser, user.Id))
            {
                lblMessage.Text = "Password changed successfully!";
                ResetControls();
            }
            else
            {
                lblMessage.Text = "Couldnot save password, please try again!";
            }
        }
    }
}
