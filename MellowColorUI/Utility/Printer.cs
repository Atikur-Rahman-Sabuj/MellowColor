using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrinterUtility;
using ModelLibrary.Entity;
using System.Globalization;

namespace MellowColorUI.Utility
{
    public class Printer
    {
        PrinterUtility.EscPosEpsonCommands.EscPosEpson obj;
        String PrinterPath;
        public Printer()
        {
            obj = new PrinterUtility.EscPosEpsonCommands.EscPosEpson();
            PrinterPath = "\\\\" + System.Windows.Forms.SystemInformation.ComputerName + "\\RP80 Printer";
        }
        public bool Print(Order order, List<OrderDetail> orderDetails, String PaymentType, Decimal PaymentAmount, User user)
        {
            try
            {
                byte[] BytesValue = GetHeader(order, user);
                BytesValue = PrintExtensions.AddBytes(BytesValue, GenerateInvoiceData(order, orderDetails, PaymentType, PaymentAmount));
                BytesValue = PrintExtensions.AddBytes(BytesValue, GetFooter(order));
                BytesValue = PrintExtensions.AddBytes(BytesValue, CutFull());
                PrintToPrinter(BytesValue);
                return true;
            }
            catch (Exception exc)
            {

                return false;
            }
        }
        public byte[] GetHeader(Order order, User user)
        {
            var BytesValue = obj.FontSelect.FontE();
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.DoubleHeight8());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.DoubleWidth3());
            BytesValue = PrintExtensions.AddBytes(BytesValue, "MELLOW COLOR\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.CharSize.Nomarl());
            BytesValue = PrintExtensions.AddBytes(BytesValue, "RAMC Complex, 1st floor, Shop# 53,\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "Medical More, Dhap, Rangpur\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "01796937977\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "www.facebook.com/mellowcolor\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());

            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.FontSelect.SpecialFontA());

            BytesValue = PrintExtensions.AddBytes(BytesValue, "Date : " + DateTime.Now.ToString() + "\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Left());
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,-30}{1,34}\n", "Inv : "+order.Number, "Served By : "+user.Name));

            return BytesValue;
        }
        public byte[] GenerateInvoiceData(Order order, List<OrderDetail> orderDetails, String PaymentType, Decimal PaymentAmount)
        {
            var BytesValue = obj.FontSelect.SpecialFontA();
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Left());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
           // BytesValue = PrintExtensions.AddBytes(BytesValue, "Items                       Qty    Price       Disc        Total\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,-19}{1,6}{2,12}{3,12}{4,15}\n", "Items", "Qty", "Price","Disc", "Total"));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());

            foreach (var item in orderDetails)
            {
                BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,-19}{1,6}{2,12}{3,12}{4,15}\n", item.Name, item.Quantity, item.Product.SellingPrice.ToString("N", new CultureInfo("en-US")), ((item.Quantity * item.Product.SellingPrice) - item.SellingPrice).ToString("N", new CultureInfo("en-US")), item.SellingPrice.ToString("N", new CultureInfo("en-US"))));
            }
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());

            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Right());

            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,15}{1,15}\n", "Sub Total :", (orderDetails.Sum(a=>a.SellingPrice)).ToString("N", new CultureInfo("en-US"))));
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,15}{1,15}\n", "(-) Discount :", (orderDetails.Sum(a => a.SellingPrice)-order.TotalPrice).ToString("N", new CultureInfo("en-US"))));
            BytesValue = PrintExtensions.AddBytes(BytesValue, "---------------\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,15}{1,15}\n", "Net Payable :", Math.Floor(order.TotalPrice).ToString("N", new CultureInfo("en-US"))));
            BytesValue = PrintExtensions.AddBytes(BytesValue, "---------------\n");

            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,15}{1,15}\n", "Payment Type :", PaymentType));
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,15}{1,15}\n", "Payment Amount :", PaymentAmount.ToString("N", new CultureInfo("en-US"))));   
            BytesValue = PrintExtensions.AddBytes(BytesValue, String.Format("{0,15}{1,15}\n", "Change Amount :", (PaymentAmount-Math.Floor(order.TotalPrice)).ToString("N", new CultureInfo("en-US"))));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
            return BytesValue;
        }
        public byte[] GetFooter(Order order)
        {
            var BytesValue = obj.FontSelect.FontE();
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Center());
            BytesValue = PrintExtensions.AddBytes(BytesValue, "Sold Product Can Not Be Returned\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "Only Exchangeable Within 7 Days\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Separator());
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.BarCode.Code128(order.Number));
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.QrCode.Print("www.facebook.com/mellowcolor, 01796937977", PrinterUtility.Enums.QrCodeSize.Grande));
            BytesValue = PrintExtensions.AddBytes(BytesValue, "\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "Thanks For Shopping At Mellow Color\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, "\n\n\n\n\n\n");
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Alignment.Left());
            return BytesValue;
        }
        public byte[] CutFull()
        {
            var BytesValue = obj.Alignment.Left();
            BytesValue = PrintExtensions.AddBytes(BytesValue, obj.Guilhotina.CutFull());
            return BytesValue;
        }
        public void PrintToPrinter(byte[] BytesValue)
        {
            PrinterUtility.PrintExtensions.Print(BytesValue, PrinterPath);
        }


    }
}
