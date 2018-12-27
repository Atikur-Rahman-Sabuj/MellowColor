using ModelLibrary.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MellowColorUI.Utility
{
    public static class Utilities
    {
        public static void ExitApplication()
        {
            Application.Exit();
        }
        
        public static void MinimizeApplication(Form form)
        {
            form.WindowState = FormWindowState.Minimized;
        }
        public static void Logout(Form form)
        {
            //RegisterUser registerPage = new RegisterUser();
            //registerPage.Show();
            //form.Hide();
            RedirectiontoForm(form, new LoginUser());
        }
        public static void SetFullScreen(Form form)
        {
            int x = Screen.PrimaryScreen.Bounds.Width;
            int y = Screen.PrimaryScreen.Bounds.Height;
            form.Location = new Point(0, 0);
            form.Size = new Size(x, y);
        }
        public static void RedirectiontoForm(Form FromForm, Form ToForm)
        {
            ToForm.Show();
            FromForm.Hide();
        }
        public static void BindListToGridView<T>(List<T> list, DataGridView GridView, Label Label)
        {
            var bindingList = new BindingList<T>(list);
            var source = new BindingSource(bindingList, null);
            GridView.DataSource = source;
            Label.Text = "Total " + list.Count + " result/s found";
        }

        async public static void TimerUpdate(Label lblTime)
        {
            while (true)
            {
                lblTime.Text = DateTime.Now.ToString("dd MMM yy HH:mm tt");
                await Task.Delay(1000);
            }
        }

    }
}
