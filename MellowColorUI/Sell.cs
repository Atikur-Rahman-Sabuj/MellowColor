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
    public partial class Sell : Form
    {
        User user;
        ModelLibrary.Entity.Product Product;
        List<ModelLibrary.Entity.Product> AllProducts;
        List<ModelLibrary.Entity.Product> CustomProducts;
        List<ModelLibrary.Entity.Product> SearchProducts;
        List<ModelLibrary.Entity.Product> CartProducts;
        Order GlobalOrder;
        List<OrderDetail> GlobalOrderDetails;
        ProductDataAccess productDataAccess;
        String PaymentType;
        Decimal PaidAmount;

        public Sell(User user)
        {
            InitializeComponent();
            this.user = user;
            productDataAccess = new ProductDataAccess();
            dataGridViewSearchResult.AutoGenerateColumns = false;
            dataGridViewCart.AutoGenerateColumns = false;
            lblUserName.Text = user.Name;
            Utilities.TimerUpdate(lblTime);
            Utilities.SetFullScreen(this);
            SearchProducts = new List<ModelLibrary.Entity.Product>();
            AllProducts = productDataAccess.GetAllExceptDeleted();
            AllProducts.ForEach(a => { a.Total = 1; a.TotalPrice = a.SellingPrice; a.DiscountAmount = 0; a.DiscountType = "Flat"; });
            CartProducts = new List<ModelLibrary.Entity.Product>();
            BindDataToDiscountTypeComboBox();
            BindDataToPaymentTypeComboBox();
        }

        private void BindDataToPaymentTypeComboBox()
        {
            List<CustomFilter> FilterItems = new List<CustomFilter>(){
            new CustomFilter(){Name = "Cash", Id = 0},
            new CustomFilter(){Name = "Card", Id = 1},
            new CustomFilter(){Name = "Bkah", Id = 2},
            new CustomFilter(){Name = "Other", Id = 3},
 
            };
            cbxPaymentType.DataSource = new BindingSource(FilterItems, null);
            cbxPaymentType.DisplayMember = "Name";
            cbxPaymentType.ValueMember = "Id";
        }

        private void BindDataToDiscountTypeComboBox()
        {
            List<CustomFilter> FilterItems = new List<CustomFilter>(){
            new CustomFilter(){Name = "Select Discount Type", Id = 0},
            new CustomFilter(){Name = "Flat", Id = 1},
            new CustomFilter(){Name = "Percentage", Id = 2},
 
            };
            cbxDiscountType.DataSource = new BindingSource(FilterItems, null);
            cbxDiscountType.DisplayMember = "Name";
            cbxDiscountType.ValueMember = "Id";
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.MinimizeApplication(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Home(user));
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

        private void dataGridViewCart_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (dataGridViewCart.Columns[e.ColumnIndex].Name == "btnRemove")
            {
                CartProducts.RemoveAt(e.RowIndex);
                Utilities.BindListToGridView<ModelLibrary.Entity.Product>(CartProducts, dataGridViewCart, lblMessage2);
                SetTotals();
            }
        }

        private void tbxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PerformSearchAction();
            }
        }
        private void PerformSearchAction()
        {
            CustomProducts = AllProducts.Where(a => (a.Name + a.BarCode + a.BarCode + a.CategoryName + a.BrandName).ToLower().Contains(tbxSearch.Text.ToLower())).ToList();
            if (CustomProducts.Count == 1)
            {
                if (CustomProducts[0].Stock>0)
                {
                    long Id = CustomProducts[0].Id;
                    List<long> IdList = CartProducts.Select(a => a.Id).ToList();
                    if (!IdList.Contains(Id))
                    {
                        CartProducts = CartProducts.Concat(CustomProducts).ToList();
                    }
                    else
                    {
                        Product = CartProducts.Where(a => a.Id == Id).ToList().First();
                        int RowIndex = IdList.IndexOf(Id);
                        Product.Total += 1;
                        dataGridViewCart_CellEndEdit(null, new DataGridViewCellEventArgs(0, RowIndex));
                    }
                    Utilities.BindListToGridView<ModelLibrary.Entity.Product>(CartProducts, dataGridViewCart, lblMessage2);
                    SetTotals();
                }
                else
                {
                    lblMessage.Text = "Empty Stock";
                }
                
            }
            else
            {
                SearchProducts = CustomProducts;

                Utilities.BindListToGridView<ModelLibrary.Entity.Product>(SearchProducts, dataGridViewSearchResult, lblMessage);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            PerformSearchAction();
        }

        private void dataGridViewSearchResult_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewSearchResult.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridViewCart_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewCart.Rows[e.RowIndex].Cells["SerialNo"].Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridViewSearchResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (dataGridViewSearchResult.Columns[e.ColumnIndex].Name == "btnAddToCart")
            {

                Product = SearchProducts.ElementAt(e.RowIndex);
                if (CartProducts.Select(a => a.Id).ToList().Contains(Product.Id))
                {
                    Product = CartProducts.Where(a => a.Id == Product.Id).ToList().First();
                    int RowIndex = CartProducts.IndexOf(Product);
                    Product.Total += 1;
                    dataGridViewCart_CellEndEdit(null, new DataGridViewCellEventArgs(0, RowIndex));
                }
                else
                {
                    if (Product.Stock>0)
                    {
                        CartProducts.Add(Product);
                    }
                    else
                    {
                        lblMessage.Text = "Empty Stock";
                    }
                    
                }
                //Product.Total = 1;

                Utilities.BindListToGridView<ModelLibrary.Entity.Product>(CartProducts, dataGridViewCart, lblMessage2);
                SetTotals();
            }
        }

        private void dataGridViewCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Product = CartProducts[e.RowIndex];
                Product.Total = Convert.ToInt32(this.dataGridViewCart.Rows[e.RowIndex].Cells["Total"].Value);
                if (Product.Total > Product.Stock || Product.Total<1)
                {
                    Product.Total = Product.Stock;
                    lblMessage.Text = "Quantity can not be bigger than stock or smaller than one.";
                }
     
                Product.DiscountAmount = Convert.ToDecimal(this.dataGridViewCart.Rows[e.RowIndex].Cells["DiscountAmount"].Value);
                string GridSelectedItem = Convert.ToString((dataGridViewCart.Rows[e.RowIndex].Cells["Discount"] as DataGridViewComboBoxCell).FormattedValue.ToString());
                Product.DiscountType = GetDiscountType(GridSelectedItem);
                if (GridSelectedItem == "Percentage")
                {
                    Product.TotalPrice = (Product.SellingPrice - (Product.SellingPrice * Product.DiscountAmount / 100)) * Product.Total;
                }
                else
                {
                    Product.TotalPrice = (Product.SellingPrice - Product.DiscountAmount) * Product.Total;
                }
                if (Product.TotalPrice < 0)
                {
                    Product.TotalPrice = 0;
                }
                dataGridViewCart.Refresh();
                SetTotals();
                //this.dataGridViewCart.Rows[e.RowIndex].Cells["TotalPrice"].Value = Convert.ToDecimal(this.dataGridViewCart.Rows[e.RowIndex].Cells["Price"].Value) * Convert.ToInt32(this.dataGridViewCart.Rows[e.RowIndex].Cells["Total"].Value);
            }
            catch (Exception ex)
            {

                lblMessage.Text = ex.Message;
            }

        }
        private void SetTotals()
        {
            tbxTotal.Text = CartProducts.Sum(a => a.TotalPrice).ToString();
            if (nudDiscount.Value > 0)
            {
                if (cbxDiscountType.SelectedValue.ToString() == "2")
                {
                    tbxNetTotal.Text = (CartProducts.Sum(a => a.TotalPrice) - (CartProducts.Sum(a => a.TotalPrice) * nudDiscount.Value / 100)).ToString();
                }
                else
                {
                    tbxNetTotal.Text = (CartProducts.Sum(a => a.TotalPrice) - nudDiscount.Value).ToString();
                }
            }
            else
            {
                tbxNetTotal.Text = tbxTotal.Text;
            }
            if (Convert.ToDecimal(tbxNetTotal.Text) < 0)
            {
                tbxNetTotal.Text = "0";
            }


        }

        private void nudDiscount_ValueChanged(object sender, EventArgs e)
        {
            SetTotals();
        }

        private void cbxDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetTotals();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsAllValidated())
            {
                SaveOrder();
                ResetControls();
            }
        }

        private void SaveOrder()
        {
            Order order = new Order()
            {
                Number = DateTime.Now.ToString("yyyyMMddHHmmss"),
                Date = DateTime.Now,
                CustomerName = tbxCustomerName.Text,
                CustomerPhone = tbxCustomerPhone.Text,
                CustomerAddress = tbxCustomerAddress.Text,
                DiscountType = GetDiscountType(cbxDiscountType.Text),
                DiscountAmount = nudDiscount.Value,
                TotalItem = CartProducts.Count,
                TotalPrice = Convert.ToDecimal(tbxNetTotal.Text)
            };
            List<OrderDetail> orderDetails = CartProducts.Select(a => new OrderDetail()
            {
                Name = a.Name,
                ProductId = a.Id,
                Product = a,
                Quantity = a.Total,
                DiscountType = a.DiscountType,
                DiscountAmount = a.DiscountAmount,
                SellingPrice = a.TotalPrice,
            }).ToList();
            order.OrderDetails = orderDetails;
            CartProducts.ForEach(a => a.Stock -= a.Total);
            if (productDataAccess.UpdateListOfProducts(CartProducts, user.Id))
            {
                lblMessage.Text = "Stocks updated";
            }
            //productDataAccess._mellowColorContext.Dispose();
            //productDataAccess._mellowColorContext = null;
            //productDataAccess._mellowColorContext.Entry(order).State = System.Data.Entity.EntityState.Detached;
            //productDataAccess._mellowColorContext.Orders.Add(order);
            //productDataAccess._mellowColorContext.SaveChanges();
            OrderDataAccess orderDataAccess = new OrderDataAccess();
            orderDataAccess._mellowColorContext = productDataAccess._mellowColorContext;
            orderDataAccess._mellowColorDbSet = orderDataAccess._mellowColorContext.Set<Order>();
            if (new GenericDataAccess<Order>().Save(order, user.Id))
            {
                lblMessage.Text = "Order Saved Sussessfully!";
            }
            //productDataAccess._mellowColorContext = new MellowColorContext();
            GlobalOrder = order;
            GlobalOrderDetails = orderDetails;
            PaymentType = GetSelectedText(cbxPaymentType.SelectedIndex);
            PaidAmount = nudPaidAmount.Value;
            PrintOrder();
        }

        private void PrintOrder()
        {
            Printer printer = new Printer();
            bool IsPrintSuccessfull =  printer.Print(GlobalOrder, GlobalOrderDetails, PaymentType, PaidAmount, user);
        }

        private string GetDiscountType(String ControlText)
        {
            if (ControlText == "Percentage")
            {
                return ControlText;
            }
            return "Flat";
        }
        private bool IsAllValidated()
        {
            if (CartProducts.Count > 0 && nudPaidAmount.Value>=Convert.ToDecimal(tbxNetTotal.Text))
            {
                return true;
            }
            return false;
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }

        private void cbxPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxPaymentType.SelectedIndex.ToString()!="0")
            {
                nudPaidAmount.Value = Convert.ToDecimal(tbxNetTotal.Text);
            }
        }
        private String GetSelectedText(int SelectedIndex)
        {
            if (SelectedIndex == 0)
            {
                return "Cash";
            }
            if (SelectedIndex == 1)
            {
                return "Card";
            }
            if (SelectedIndex == 2)
            {
                return "Bkash";
            }
            return "Other";
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            PrintOrder();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
        }
        private void ResetControls()
        {
            tbxSearch.Clear();
            tbxCustomerName.Clear();
            tbxCustomerAddress.Clear();
            tbxCustomerPhone.Clear();
            tbxTotal.Clear();
            tbxNetTotal.Clear();
            nudDiscount.Value = 0;
            nudPaidAmount.Value = 0;
            SearchProducts = new List<ModelLibrary.Entity.Product>();
            CartProducts = new List<ModelLibrary.Entity.Product>();
            Utilities.BindListToGridView(SearchProducts, dataGridViewSearchResult, lblMessage);
            Utilities.BindListToGridView(CartProducts, dataGridViewCart, lblMessage2);
        }

        private void Sell_Load(object sender, EventArgs e)
        {
            tbxSearch.Focus();
        }
    }
}
