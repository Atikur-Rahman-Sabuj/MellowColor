using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MellowColorUI.Utility;
using ModelLibrary.Entity;
using DataAccessLibrary.DA;

namespace MellowColorUI
{
    public partial class Home : Form
    {
        public User user;
        ProductDataAccess productDataAccess;
        List<ModelLibrary.Entity.Product> GlobalProducts;
        public Home(User user)
        {
            this.user = user;  
            InitializeComponent();
            dataGridViewProduct.AutoGenerateColumns = false;
            lblUserName.Text = user.Name;
            Utilities.TimerUpdate(lblTime);
            Utilities.SetFullScreen(this);
            productDataAccess = new ProductDataAccess();
            GlobalProducts = productDataAccess.GetAllExceptDeleted().Where(a => a.Stock <= 2).OrderBy(a => a.Stock).ToList();
            Utilities.BindListToGridView(GlobalProducts, dataGridViewProduct, lblMessage);
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

        private void pnlSell_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlCategory_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pnlCategory_Click(object sender, EventArgs e)
        {
            Category category = new Category(user);
            category.Show();
            this.Hide();
        }

        private void pnlBrand_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Brand(user));
        }

        private void pnlSell_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Sell(user));
        }

        private void pnlProduct_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Product(user));
        }


        private void panel5_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }

        private void panel27_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ProductTag(user));
        }

        private void panel29_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ViewSells(user));
        }

        private void dataGridViewProduct_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewProduct.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }

        private void panel11_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new RegisterUser(user));
        }

        private void Home_Load(object sender, EventArgs e)
        {
            if (user.Type == "Seller")
            {
                HideComponents();
            }
            if(user.Type == "SuperAdmin")
            {
                btnManageUser.Show();
            }
        }
        private void HideComponents()
        {
            pnlAddUser.Hide();
            PnlAddUserUpper.Hide();
            pnlViewSells.Hide();
            pnlViewSellsUperPart.Hide();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Sell(user));
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Sell(user));
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Sell(user));
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Product(user));
        }

        private void panel7_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Product(user));
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Product(user));
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Category(user));
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Category(user));
        }

        private void lblCategory_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Category(user));
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Brand(user));
        }

        private void panel6_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Brand(user));
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Brand(user));
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ProductTag(user));
        }

        private void panel28_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ProductTag(user));
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ProductTag(user));
        }

        private void pbxViewSells_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ViewSells(user));
        }

        private void pnlViewSellsText_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ViewSells(user));
        }

        private void lblviewSellsText_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ViewSells(user));
        }

        private void pbxAddUser_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new RegisterUser(user));
        }

        private void pnlAddUserText_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new RegisterUser(user));
        }

        private void lblAddUserText_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new RegisterUser(user));
        }

        private void btnManageUser_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new ManageUser(user));
        }
    }
}
