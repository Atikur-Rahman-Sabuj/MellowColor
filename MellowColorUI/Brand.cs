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
    public partial class Brand : Form
    {
        User user;
        ModelLibrary.Entity.Brand globalBrand;
        List<ModelLibrary.Entity.Brand> globalBrands;
        Boolean CreateMood = true;
        BrandDataAccess brandDataAccess;
        public Brand(User user)
        {
            InitializeComponent();
            this.user = user;
            lblUserName.Text = user.Name;
            Utilities.TimerUpdate(lblTime);
            Utilities.SetFullScreen(this);
            brandDataAccess = new BrandDataAccess();
            BindBrandsToList();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            if (brandDataAccess.GetByName(tbxName.Text) != null)
            {
                lblMessage.Text = "Could not register, Category name already exists.";
                
                return;
            }
            ModelLibrary.Entity.Brand   brand;
            if (CreateMood)
            {
                brand = new ModelLibrary.Entity.Brand()
                {
                    Name = tbxName.Text,
                };
            }
            else
            {
                brand = globalBrand;
                brand.Name = tbxName.Text;
            }


            var validationContext = new ValidationContext(brand, null, null);
            var results = new List<ValidationResult>();


            if (Validator.TryValidateObject(brand, validationContext, results, true))
            {
                lblError.Text = "";
                
                if (brandDataAccess.Save(brand, user.Id))
                {
                    ResetFormControls();
                    BindBrandsToList();
                    CreateMood = true;
                    btnSave.Text = "Create";
                    lblMessage.Text = "Created Successfully!";
                }
                else
                {
                    lblMessage.Text = "Could not create, Try again!";
                }
            }
            else
            {
                String errorMessage = "Errors:\n";
                results.ForEach(a => errorMessage = errorMessage + a.ErrorMessage + "\n");
                lblError.Text = errorMessage;
            }
        }

        private void BindBrandsToList()
        {
            dataGridViewBrand.AutoGenerateColumns = false;
            globalBrands = brandDataAccess.GetAllExceptDeleted();
            Utilities.BindListToGridView<ModelLibrary.Entity.Brand>(globalBrands, dataGridViewBrand, lblMessage);
        }

        private void ResetFormControls()
        {
            tbxName.Text = "";
            CreateMood = true;
            btnSave.Text = "Create";
        }

        private void dataGridViewCategory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewBrand.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridViewBrand_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (dataGridViewBrand.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                globalBrand = globalBrands.ElementAt(e.RowIndex);
                tbxName.Text = globalBrand.Name;
                btnSave.Text = "Update";
                CreateMood = false;
            }
            else if (dataGridViewBrand.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                if (MessageBox.Show("Do you want to delete this category ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    globalBrand = globalBrands.ElementAt(e.RowIndex);
                    brandDataAccess.Delete(globalBrand.Id, user.Id);
                    BindBrandsToList();

                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            CreateMood = true;
            btnSave.Text = "Create";
            ResetFormControls();
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            globalBrands = brandDataAccess.GetBrandsForSearch(tbxSearch.Text);
            Utilities.BindListToGridView<ModelLibrary.Entity.Brand>(globalBrands,dataGridViewBrand, lblMessage);
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            globalBrands = brandDataAccess.GetBrandsForSearch(tbxSearch.Text);
            Utilities.BindListToGridView<ModelLibrary.Entity.Brand>(globalBrands, dataGridViewBrand, lblMessage);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }
    }
}
