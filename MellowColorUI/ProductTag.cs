using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrinterUtility;
using MellowColorUI.Utility;
using System.Globalization;
using ModelLibrary.Entity;

namespace MellowColorUI
{
    public partial class ProductTag : Form
    {
        User user;
        String Size = "";
        String Color = "";
        Decimal Price = 0;
        String Barcode = "";
        PrinterUtility.EscPosEpsonCommands.EscPosEpson obj;
        public ProductTag(User user)
        {
            InitializeComponent();
            obj = new PrinterUtility.EscPosEpsonCommands.EscPosEpson();
            this.user = user;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ResetControls();
            tbxBarcode.Text = "";
        }

        private void ResetControls()
        {
            tbxSize.Clear();
            tbxColor.Clear();
            nudPrice.Value = 0;
            tbxBarcode.Text = Barcode;
        }
        private void SetValues()
        {
            Size = tbxSize.Text;
            Color = tbxColor.Text;
            Price = nudPrice.Value;
            SetBarCode();
        }
        private byte[] SetPrintValue()
        {
            if (tbxSize.Text != "")
            {
                SetValues();

            }
            var BytesValue = obj.Alignment.Center();
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.FontSelect.FontE());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.Nomarl());
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,22} : {1,-22}\n","Size",Size));
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,22} : {1,-22}\n", "Color", Color));
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,22} : {1,-22}\n", "Price", Price.ToString("N", new CultureInfo("en-US"))));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.BarCode.Code128(Barcode));
            BytesValue = PrintExtensions.AddBytes(BytesValue, "\n\n\n\n\n");
            return BytesValue;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Printer printer = new Printer();
            var BytesValue = SetPrintValue();
            printer.PrintToPrinter(BytesValue);
            ResetControls();
        }
        private void SetBarCode()
        {
            if (tbxBarcode.Text=="")
            {
                Barcode = DateTime.Now.ToString("yyyyMMddmmss");
            }
            else
            {
                Barcode = tbxBarcode.Text;
            }
        }
        private void btnCut_Click(object sender, EventArgs e)
        {
            Printer print = new Printer();
            print.PrintToPrinter(print.CutFull());
        }

        private void btnPrintCut_Click(object sender, EventArgs e)
        {
            Printer printer = new Printer();
            var BytesValue = SetPrintValue();
            BytesValue = PrintExtensions.AddBytes(BytesValue, printer.CutFull());
            printer.PrintToPrinter(BytesValue);
            ResetControls();
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
            Utilities.RedirectiontoForm(this, new ViewSells(user));
        }
    }
}
