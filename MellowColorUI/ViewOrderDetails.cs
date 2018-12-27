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
    public partial class ViewOrderDetails : Form
    {

        public ViewOrderDetails(List<OrderDetail> orderDetails)
        {
            InitializeComponent();
            List<DataSourceClass> DataSources = orderDetails.Select(a => new DataSourceClass() { Name = a.Product.Name, Quantity = a.Quantity, BuyingPrice = a.Product.BuyingPrice, SellingPrice = a.SellingPrice }).ToList();
            dataGridViewSells.AutoGenerateColumns = false;
            Utilities.BindListToGridView<DataSourceClass>(DataSources, dataGridViewSells, lblMessage);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ViewOrderDetails_Load(object sender, EventArgs e)
        {

        }

        private void dataGridViewSells_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewSells.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }


        
    }


}
