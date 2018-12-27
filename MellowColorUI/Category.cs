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
using System.ComponentModel.DataAnnotations;

namespace MellowColorUI
{
    public partial class Category : Form
    {
        User user;
        ModelLibrary.Entity.Category globalCategory;
        List<ModelLibrary.Entity.Category> globalCategories;
        Boolean CreateMood = true;
        CategoryDataAccess categoryDataAccess;
        public Category(User user)
        {
            InitializeComponent();
            this.user = user;
            categoryDataAccess = new CategoryDataAccess();
            lblUserName.Text = user.Name;
            Utilities.TimerUpdate(lblTime);
            Utilities.SetFullScreen(this);
            dataGridViewCategory.AutoGenerateColumns = false;
            BindCategoriesToList();

        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            Utilities.Logout(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Utilities.ExitApplication();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (categoryDataAccess.GetByName(tbxName.Text) != null)
            {
                lblMessage.Text = "Could not register, Category name already exists.";
                return;
            }
            ModelLibrary.Entity.Category category;
            if (CreateMood)
            {
                category = new ModelLibrary.Entity.Category()
                {
                    Name = tbxName.Text,
                };
            }
            else
            {
                category = globalCategory;
                category.Name = tbxName.Text;
            }
            

            var validationContext = new ValidationContext(category, null, null);
            var results = new List<ValidationResult>();


            if (Validator.TryValidateObject(category, validationContext, results, true))
            {
                lblError.Text = "";
                if (categoryDataAccess.Save(category, user.Id))
                {
                    ResetFormControls();
                    BindCategoriesToList();
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

        private void ResetFormControls()
        {
            tbxName.Text = "";
            CreateMood = true;
            btnSave.Text = "Create";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }
        private void BindCategoriesToList()
        {
            dataGridViewCategory.AutoGenerateColumns = false;
            globalCategories = categoryDataAccess.GetAllExceptDeleted();
            Utilities.BindListToGridView<ModelLibrary.Entity.Category>(globalCategories, dataGridViewCategory, lblMessage);
        }

        private void dataGridViewCategory_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            this.dataGridViewCategory.Rows[e.RowIndex].Cells["Column2"].Value = (e.RowIndex + 1).ToString();
        }

        private void dataGridViewCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (dataGridViewCategory.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                globalCategory = globalCategories.ElementAt(e.RowIndex);
                tbxName.Text = globalCategory.Name;
                btnSave.Text = "Update";
                CreateMood = false;
            }
            else if (dataGridViewCategory.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                if(MessageBox.Show("Do you want to delete this category ?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    globalCategory = globalCategories.ElementAt(e.RowIndex);
                    categoryDataAccess.Delete(globalCategory.Id, user.Id);
                    BindCategoriesToList();
                    ResetFormControls();
                    
                }
            }
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

        private void btnBrand_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Brand(user));
        }

        private void Category_Load(object sender, EventArgs e)
        {

        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            globalCategories = categoryDataAccess.GetCategoriesForSearch(tbxSearch.Text);
            Utilities.BindListToGridView<ModelLibrary.Entity.Category>(globalCategories, dataGridViewCategory, lblMessage);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            Utilities.RedirectiontoForm(this, new Return(user));
        }
    }
}
