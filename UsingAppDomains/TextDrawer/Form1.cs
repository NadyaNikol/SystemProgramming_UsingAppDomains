using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextDrawer
{
    public partial class Form1 : Form
    {
        String SourceText = "No text was added";
        Font DrawindFont;
        public Form1()
        {
            InitializeComponent();

            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            ControlBox = false;
            panel1.Dock = DockStyle.Fill;

            DrawindFont = new Font("Arial", 45);
            panel1.Paint += Panel1_Paint;
            this.Paint += Form1_Paint;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Panel1_Paint(panel1, new PaintEventArgs(panel1.CreateGraphics(), panel1.ClientRectangle));
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            if (SourceText.Length>0)
            {
                //создаем буферное изображение основываясь на размерах клиенской части элемента управления Panel
                Image img = new Bitmap(panel1.ClientRectangle.Width, panel1.ClientRectangle.Height);

                //получаем графический контекст созданного нами изображения
                Graphics ImgDC = Graphics.FromImage(img);

                //очищаем изображение используя цвет фона окна
             //   ImgDC.Clear(BackColor);

                //прорисовываем на элементе управления Panel текст используя выбранный шрифт
                ImgDC.DrawString(SourceText, DrawindFont, Brushes.Brown, ClientRectangle, new StringFormat(StringFormatFlags.NoFontFallback));

                //прорисовываем изображение на элементе управления Panel
                e.Graphics.DrawImage(img, 0, 0);
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //создаем обьект стандартного диалога
            FontDialog fd = new FontDialog();

            fd.Font = DrawindFont;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                DrawindFont = fd.Font;

                //инициализируем перерисовку окна
                Panel1_Paint(panel1, new PaintEventArgs(panel1.CreateGraphics(), panel1.ClientRectangle));
            }
        }

        public void SetText(string text)
        {
            SourceText = text;
            Panel1_Paint(panel1, new PaintEventArgs(panel1.CreateGraphics(), panel1.ClientRectangle));
        }

        public void MoveResize(Point newLocation, int width)
        {
            /*устанавливаем.новое.значение.позиции.окна*/
            //this.Location = newLocation;
            /*устанавливаем.новое.значение.ширины.окна*/
            //this.Width = width;
             UpdateForm(newLocation, width);
        }

        private void UpdateForm(Point newLocation, int width)
        {
            if (this.InvokeRequired)
                this.Invoke(new Action<Point, int>(UpdateForm), new object[] { newLocation, width });
            else
            {
                this.Location = newLocation;
                //устанавливаем новое значение ширины окна
                this.Width = width;
            }
        }
    }
}
