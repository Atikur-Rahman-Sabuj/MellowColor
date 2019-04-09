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
    public partial class Return : Form
    {
        User user;
        List<Order> GlobalOrders;
        List<OrderDetail> GlobalOrderDetails;
        Order GlobalOrder;
        String SearchString;
        OrderDataAccess orderDataAccess;
        public Return(User user)
        {
            InitializeComponent();
            this.user = user;
            dataGridViewOrder.AutoGenerateColumns = false;
            orderDataAccess = new OrderDataAccess();
            dataGridViewOrderDetails.AutoGenerateColumns = false;
            lblUserName.Text = user.Name;
            Utilities.TimerUpdate(lblTime);
            Utilities.SetFullScreen(this);
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

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.MinimizeApplication(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void lblMessage2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            ShowSearchedOrders();
        }

        private void ShowSearchedOrders()
        {
            SearchString = tbxSearch.Text;
            GlobalOrders = orderDataAccess.GetAll().Where(a => (a.Number + a.CustomerName + a.CustomerPhone + a.Date.ToString()).ToLower().Contains(tbxSearch.Text.ToLower())).ToList();
            Utilities.BindListToGridView(GlobalOrders, dataGridViewOrder, lblMessage);
            if (GlobalOrders.Count == 1)
            {
                ShowSearchedOrderDetails();
            }
        }

        private void tbxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ShowSearchedOrders();
            }
        }

        private void dataGridViewOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (dataGridViewOrder.Columns[e.ColumnIndex].Name == "btnDetails")
            {
                ShowSearchedOrderDetails(e.RowIndex);
            }
        }
        private void ShowSearchedOrderDetails( int i = 0)
        {
            GlobalOrder = GlobalOrders[i];
            GlobalOrderDetails = GlobalOrders[i].OrderDetails.ToList();
            GlobalOrderDetails.ForEach(a => a.Name = a.Product.Name);
            Utilities.BindListToGridView(GlobalOrderDetails, dataGridViewOrderDetails, lblMessage);
        }

        private void dataGridViewOrderDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (dataGridViewOrderDetails.Columns[e.ColumnIndex].Name == "btnReturn")
            {
                GlobalOrder.TotalBuyingPrice -= GlobalOrderDetails[e.RowIndex].BuyingPrice;
                GlobalOrder.TotalPrice -= GlobalOrderDetails[e.RowIndex].SellingPrice;
                if(orderDataAccess.Save(GlobalOrder, user.Id))
                {
                    ModelLibrary.Entity.Product product = GlobalOrderDetails[e.RowIndex].Product;
                    product.Stock++;
                    GlobalOrderDetails[e.RowIndex].Quantity--;
                    OrderDetailDataAccess orderDetailDataAccess = new OrderDetailDataAccess();
                    orderDetailDataAccess._mellowColorContext = orderDataAccess._mellowColorContext;

                    orderDetailDataAccess._mellowColorDbSet = orderDetailDataAccess._mellowColorContext.Set<OrderDetail>();
                    if (orderDetailDataAccess.Save(GlobalOrderDetails[e.RowIndex], user.Id))
                    {

                        ProductDataAccess productDataAccess = new ProductDataAccess();
                        productDataAccess._mellowColorContext = orderDetailDataAccess._mellowColorContext;
                        productDataAccess._mellowColorDbSet = productDataAccess._mellowColorContext.Set<ModelLibrary.Entity.Product>();
                        if (productDataAccess.Save(product, user.Id))
                        {
                            Utilities.BindListToGridView(GlobalOrderDetails, dataGridViewOrderDetails, lblMessage);
                            lblMessage.Text = "Return successfull";
                        }
                        else
                        {
                            lblMessage.Text = "Could not save product!";
                        }


                    }
                    else
                    {
                        lblMessage.Text = "Could not save orderdetails!";
                    }
                }
                else
                {
                    lblMessage.Text = "Could not save order!";
                }
               



                //MessageBox.Show("Saved Successfully");
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Utilities.Logout(this);

        }


    }
}
