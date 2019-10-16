using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextWindow
{
    public partial class Form1 : Form
    {
        Module DrawerModule { get; set; }
        object Drawer;
        public Form1(Module drawer, object targetWindow)
        {          
            InitializeComponent();
            DrawerModule = drawer;
            Drawer = targetWindow;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //вызываем метод SetText главного окна приложения TextDrawer
            DrawerModule.GetType("TextDrawer.Form1").GetMethod("SetText").Invoke(Drawer,
                new object[] { textBox1.Text });
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            //вызываем метод MoveResize главного окна приложения TextDrawer
            DrawerModule.GetType("TextDrawer.Form1").GetMethod("MoveResize").Invoke(Drawer, new object[]
                {
                    new Point(this.Location.X, this.Location.Y+this.Height), this.Width
                });
        }
    }
}
