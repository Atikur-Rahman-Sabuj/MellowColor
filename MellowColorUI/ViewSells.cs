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
    public partial class ViewSells : Form
    {
        User user;
        List<Order> Orders;
        OrderDataAccess orderDataAccess;
        Decimal TotalBuyingPrice;
        Decimal TotalSellingPrice;
        long SelectedUser;
        public ViewSells(User user)
        {
            InitializeComponent();
            dataGridViewSells.AutoGenerateColumns = false;
            this.user = user;           
            orderDataAccess = new OrderDataAccess();
            BindDatatoFilter();
            BindDataToPersonComboBox();
            SelectedUser = -1;
        }

        private void BindDataToPersonComboBox()
        {
            UserDataAccess userDataAccess = new UserDataAccess();
            
            List<ModelLibrary.Entity.User> Users = userDataAccess.GetAll().Where(a=>a.Type == "Seller"||a.Type=="Admin").ToList();
            Users.Insert(0, new ModelLibrary.Entity.User() { Name = "All Users", Id = -1 });
            cbxUser.DisplayMember = "Name";
            cbxUser.ValueMember = "Id";
            cbxUser.DataSource = new BindingSource(Users, null);
            
        }

        private void BindDatatoFilter()
        {
            List<CustomFilter> FilterItems = new List<CustomFilter>(){
            new CustomFilter(){Name = "View By Date", Id = 1},
            new CustomFilter(){Name = "View By Range", Id = 2},
            };
            cbxDateFilterType.DataSource = new BindingSource(FilterItems, null);
            cbxDateFilterType.DisplayMember = "Name";
            cbxDateFilterType.ValueMember = "Id";
        }

        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            if (IsDatesValidated())
            {
                BindDataToDataGridView();
            }
        }
        private bool IsDatesValidated()
        {
            if (cbxDateFilterType.SelectedIndex!=0)
            {
                if (dtpToDate.Value>DateTime.Now.Date)
                {
                    lblMessage.Text = "To date can not be greater than today!";
                    return false;
                }
                if (dtpFromDate.Value.Date>dtpToDate.Value.Date)
                {
                    lblMessage.Text = "From date can not greater than To date!";
                    return false;
                }
            }
            else
            {
                if (dtpFromDate.Value>DateTime.Now.Date)
                {
                    lblMessage.Text = "Date can not be greater than today!";
                    return false;
                }
            }
            return true;
        }
        private void cbxFileterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDateFilterType.SelectedIndex == 0)
            {
                dtpFromDate.Value = DateTime.Today;
                lblFromDate.Text = "Date :";
                lblToDate.Hide();
                dtpToDate.Hide();
            }
            else
            {
                lblFromDate.Text = "From Date :";
                lblToDate.Show();
                dtpToDate.Show();
                dtpToDate.Value = DateTime.Today;
                dtpFromDate.Value = DateTime.Today.AddDays(-30);
            }
            BindDataToDataGridView();
        }
        private void BindDataToDataGridView()
        {
            if (cbxDateFilterType.SelectedIndex == 0)
            {
                Orders = orderDataAccess.GetByDate(dtpFromDate.Value.Date);
            }
            else
            {
                Orders = orderDataAccess.GetByDateRange(dtpFromDate.Value.Date, dtpToDate.Value.Date);
            }
            if (SelectedUser!= -1)
            {
                Orders = Orders.Where(a => a.CreatedBy == SelectedUser).ToList();
            }
            Utilities.BindListToGridView<Order>(Orders, dataGridViewSells, lblMessage);
            TotalBuyingPrice = Orders.Sum(a => a.OrderDetails.Sum(b => b.Product.BuyingPrice));
            TotalSellingPrice = Orders.Sum(a => a.TotalPrice);
            tbxTotalSells.Text = Orders.Count.ToString();
            tbxTotalBuyingPrice.Text = TotalBuyingPrice.ToString();
            tbxTotalSellingPrice.Text = TotalSellingPrice.ToString();
            tbxTotalRevenue.Text = (TotalSellingPrice - TotalBuyingPrice).ToString();

        }

        private void dtpToDate_ValueChanged(object sender, EventArgs e)
        {
            if (IsDatesValidated())
            {
                BindDataToDataGridView();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.MinimizeApplication(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void cbxUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedUser = Convert.ToInt64(cbxUser.SelectedValue.ToString());
            BindDataToDataGridView();
        }

        private void dataGridViewSells_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (dataGridViewSells.Columns[e.ColumnIndex].Name == "btnDetails")
            {
                ViewOrderDetails viewOrderDetails = new ViewOrderDetails(Orders[e.RowIndex].OrderDetails.ToList());
                viewOrderDetails.Show();
            }
        }

        private void dataGridViewSells_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewSells.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
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
