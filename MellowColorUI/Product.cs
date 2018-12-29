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
    public partial class Product : Form
    {
        ModelLibrary.Entity.User user;
        ModelLibrary.Entity.Product GlobalProduct;
        List<ModelLibrary.Entity.Product> GlobalProducts;
        Boolean CreateMood = true;
        Boolean PageLoaded = false;
        ProductDataAccess productDataAccess;
        public Product(ModelLibrary.Entity.User user)
        {
            InitializeComponent();
            this.user = user;
            productDataAccess = new ProductDataAccess();
            dataGridViewProduct.AutoGenerateColumns = false;
            lblUserName.Text = user.Name;
            Utilities.TimerUpdate(lblTime);
            Utilities.SetFullScreen(this);
            BindDataToCategoryComboBox();
            BindDataToBrandComboBox();
            BindDataToCustomFilterComboBox();
            BindDataToCategoryFilterComboBox();
            BindDataToBrandFilterComboBox();
            BindDataToCustomFilterComboBox();
            GlobalProducts = productDataAccess.GetAllExceptDeleted();
            Utilities.BindListToGridView<ModelLibrary.Entity.Product>(GlobalProducts, dataGridViewProduct,lblMessage);
            PageLoaded = true;
        }
        private void BindFilteredDataToGridView()
        {
            GlobalProducts = productDataAccess.GetAllExceptDeleted().Where(a => (a.BarCode + a.Name + a.CategoryName + a.BrandName).ToLower().Contains(tbxSearch.Text.ToLower())).ToList();
            if(cbxBrandFilter.SelectedValue.ToString() != "-1")
            {
                GlobalProducts = GlobalProducts.Where(a => a.BrandId.ToString() == cbxBrandFilter.SelectedValue.ToString()).ToList();
            }
            if(cbxCategoryFilter.SelectedValue.ToString() != "-1")
            {
                GlobalProducts = GlobalProducts.Where(a => a.CategoryId.ToString() == cbxCategoryFilter.SelectedValue.ToString()).ToList();
            }
            switch (cbxCustomFilter.SelectedValue.ToString())
            {
                case "1":
                    {
                        GlobalProducts = GlobalProducts.OrderBy(a => a.Name).ToList();
                        break;
                    }
                case "2":
                    {
                        GlobalProducts = GlobalProducts.OrderByDescending(a => a.Name).ToList();
                        break;
                    }
                case "3":
                    {
                        GlobalProducts = GlobalProducts.OrderBy(a => a.SellingPrice).ToList();
                        break;
                    }
                case "4":
                    {
                        GlobalProducts = GlobalProducts.OrderByDescending(a => a.SellingPrice).ToList();
                        break;
                    }
                case "5":
                    {
                        GlobalProducts = GlobalProducts.OrderBy(a => a.Stock).ToList();
                        break;
                    }
                case "6":
                    {
                        GlobalProducts = GlobalProducts.OrderByDescending(a => a.Stock).ToList();
                        break;
                    }
                default: GlobalProducts = GlobalProducts.OrderBy(a => a.Name).ToList();
                         break;
            }
            Utilities.BindListToGridView<ModelLibrary.Entity.Product>(GlobalProducts, dataGridViewProduct, lblMessage);

        }

        private void BindDataToCustomFilterComboBox()
        {
            List<CustomFilter> FilterItems = new List<CustomFilter>(){
            new CustomFilter(){Name = "Filter By Name A-Z", Id = 1},
            new CustomFilter(){Name = "Filter By Name Z-A", Id = 2},
            new CustomFilter(){Name = "Filter By Price Low-High", Id = 3},
            new CustomFilter(){Name = "Filter By Price High-Low", Id = 4},
            new CustomFilter(){Name = "Filter By Stock High-Low", Id = 5},
            new CustomFilter(){Name = "Filter By Stock Low-High", Id = 6}
            };
            cbxCustomFilter.DataSource = new BindingSource(FilterItems, null);
            cbxCustomFilter.DisplayMember = "Name";
            cbxCustomFilter.ValueMember = "Id";
        }

        private void BindDataToBrandFilterComboBox()
        {
            BrandDataAccess brandDataAccess = new BrandDataAccess();
            List<ModelLibrary.Entity.Brand> Brands = brandDataAccess.GetAllExceptDeleted();
            Brands.Insert(0, new ModelLibrary.Entity.Brand() { Name = "Filter By Brand", Id = -1 });
            cbxBrandFilter.DataSource = new BindingSource(Brands, null);
            cbxBrandFilter.DisplayMember = "Name";
            cbxBrandFilter.ValueMember = "Id";
        }

        private void BindDataToCategoryFilterComboBox()
        {
            CategoryDataAccess categoryDataAccess = new CategoryDataAccess();
            List<ModelLibrary.Entity.Category> Categories = categoryDataAccess.GetAllExceptDeleted();
            Categories.Insert(0, new ModelLibrary.Entity.Category() { Name = "Filter By Category", Id = -1 });
            cbxCategoryFilter.DataSource = new BindingSource(Categories, null);
            cbxCategoryFilter.DisplayMember = "Name";
            cbxCategoryFilter.ValueMember = "Id";
        }

        private void BindDataToBrandComboBox()
        {
            BrandDataAccess brandDataAccess = new BrandDataAccess();
            List<ModelLibrary.Entity.Brand> Brands = brandDataAccess.GetAllExceptDeleted();
            Brands.Insert(0, new ModelLibrary.Entity.Brand() { Name = "Select Brand", Id = -1 });
            cbxBrand.DataSource = new BindingSource(Brands, null);
            cbxBrand.DisplayMember = "Name";
            cbxBrand.ValueMember = "Id";
        }

        private void BindDataToCategoryComboBox()
        {
            CategoryDataAccess categoryDataAccess = new CategoryDataAccess();
            List<ModelLibrary.Entity.Category> Categories = categoryDataAccess.GetAllExceptDeleted();
            Categories.Insert(0, new ModelLibrary.Entity.Category() { Name = "Select Category", Id = -1 });
            cbxCategory.DataSource = new BindingSource(Categories, null);
            cbxCategory.DisplayMember = "Name";
            cbxCategory.ValueMember = "Id";
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Home(user));
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Sell(user));
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Category(user));
        }

        private void btnBrand_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Category(user));
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Utilities.Logout(this);
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.MinimizeApplication(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CreateMood && productDataAccess.GetAllExceptDeleted().Where(a => a.Name == tbxName.Text && a.BarCode == tbxBarCode.Text && a.Size == tbxSize.Text && a.Color == tbxColor.Text).ToList().FirstOrDefault() != null)
            {
                lblError.Text = "Product with same Name, Barcode, Size and Color already exists!";
                return;
            }
            
            if (CreateMood)
            {
                GlobalProduct = new ModelLibrary.Entity.Product()
                {
                    BarCode = tbxBarCode.Text,
                    Name = tbxName.Text,
                    Size = tbxSize.Text,
                    Color = tbxColor.Text,
                    Stock = Convert.ToInt64(nmUDStock.Value),
                    BuyingPrice = nmUDBuyPrice.Value,
                    SellingPrice = nmUDSellPrice.Value,
                    BrandId = Convert.ToInt64(cbxBrand.SelectedValue),
                    CategoryId = Convert.ToInt64(cbxCategory.SelectedValue)
                };
                if (GlobalProduct.BrandId<0||GlobalProduct.CategoryId<0)
                {
                    lblMessage.Text = "Please select Brand and Category!";
                    return;
                }
            }
            else
            {
                GlobalProduct.BarCode = tbxBarCode.Text;
                GlobalProduct.Name = tbxName.Text;
                GlobalProduct.Size = tbxSize.Text;
                GlobalProduct.Color = tbxColor.Text;
                GlobalProduct.Stock = Convert.ToInt64(nmUDStock.Value);
                GlobalProduct.BuyingPrice = nmUDBuyPrice.Value;
                GlobalProduct.SellingPrice = nmUDSellPrice.Value;
            }


            var validationContext = new ValidationContext(GlobalProduct, null, null);
            var results = new List<ValidationResult>();


            if (Validator.TryValidateObject(GlobalProduct, validationContext, results, true))
            {
                lblError.Text = "";
                if (productDataAccess.Save(GlobalProduct, user.Id))
                {
                    
                    ResetFormControls();
                    //GlobalProducts = productDataAccess.GetAllExceptDeleted();
                    //Utilities.BindListToGridView<ModelLibrary.Entity.Product>(GlobalProducts, dataGridViewProduct, lblMessage);
                    if (CreateMood)
                    {
                        lblMessage.Text = "Created Successfully! Reload to view on list.";
                    }
                    else
                    {
                        lblMessage.Text = "Updated Successfully!";
                    }


                }
                else
                {
                    lblMessage.Text = "Could not complete operation, Try again!";
                }
            }
            else
            {
                String errorMessage = "Errors:\n";
                results.ForEach(a => errorMessage = errorMessage + a.ErrorMessage + "\n");
                lblError.Text = errorMessage;
            }
        }

        private void ResetFormControls()
        {
            CreateMood = true;
            btnSave.Text = "Create";
            lblError.Text = "";
            tbxBarCode.Text = "";
            tbxName.Text = "";
            tbxSize.Text = "";
            tbxColor.Text = "";
            nmUDBuyPrice.Value = 0;
            nmUDSellPrice.Value = 0;
            nmUDStock.Value = 0;
            cbxBrand.SelectedIndex = 0;
            cbxCategory.SelectedIndex = 0;
        }

        private void label10_Click(object sender, EventArgs e)
        {
            if (PageLoaded) BindFilteredDataToGridView();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void dataGridViewProduct_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewProduct.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            if(PageLoaded) BindFilteredDataToGridView();
        }

        private void cbxCategoryFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(PageLoaded) BindFilteredDataToGridView();
        }

        private void cbxBrandFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(PageLoaded) BindFilteredDataToGridView();
        }

        private void cbxCustomFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(PageLoaded) BindFilteredDataToGridView();
        }

        private void tbxBarCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //MessageBox.Show(tbxBarCode.Text);
            }
        }

        private void tbxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(PageLoaded) BindFilteredDataToGridView();
            }
        }

        private void dataGridViewProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (dataGridViewProduct.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                GlobalProduct = GlobalProducts.ElementAt(e.RowIndex);
                tbxName.Text = GlobalProduct.Name;
                tbxBarCode.Text = GlobalProduct.BarCode;
                tbxSize.Text = GlobalProduct.Size;
                tbxColor.Text = GlobalProduct.Color;
                cbxBrand.SelectedValue = GlobalProduct.BrandId;
                cbxCategory.SelectedValue = GlobalProduct.CategoryId;
                nmUDStock.Value = GlobalProduct.Stock;
                nmUDBuyPrice.Value = GlobalProduct.BuyingPrice;
                nmUDSellPrice.Value = GlobalProduct.SellingPrice;
                btnSave.Text = "Update";
                CreateMood = false;
            }
            else if (dataGridViewProduct.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                if (MessageBox.Show("Do you want to delete this Product ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GlobalProduct = GlobalProducts.ElementAt(e.RowIndex);
                    productDataAccess.Delete(GlobalProduct.Id, user.Id);
                    GlobalProducts = productDataAccess.GetAllExceptDeleted();
                    Utilities.BindListToGridView<ModelLibrary.Entity.Product>(GlobalProducts, dataGridViewProduct, lblMessage);
                    ResetFormControls();

                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }
    }
}
