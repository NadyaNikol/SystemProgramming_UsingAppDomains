using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace UsingAppDomains
{
    static class Program
    {
        static AppDomain TextDrawer;
        static AppDomain TextWindow;

        static Assembly TextDrawerAsm;//будет хранить обьект сборки TextDriwer.exe
        static Assembly TextWindowAsm;//будет хранить обьект сборки TextWindow.exe

        static Form DrawerWindow;// будет хранить обьект окна TextDriwer
        static Form TextWindowWnd;// будет хранить обьект окна TextWindow


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [LoaderOptimization(LoaderOptimization.MultiDomain)]
        static void Main()
        {
            // включаем визуальные стили для приложения, поскольку оно является оконным
            Application.EnableVisualStyles();

            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());


            //создаем необходимые домены приложений с дружественными именами и сораняем ссылки на них в соответствующие переменные
            TextDrawer = AppDomain.CreateDomain("TextDrawer");
            TextWindow = AppDomain.CreateDomain("TextWindow");


            //загружаем сборки с оконным приложением в соответствующие домены приложений
            TextDrawerAsm = TextDrawer.Load(AssemblyName.GetAssemblyName("TextDrawer.exe"));
            TextWindowAsm = TextWindow.Load(AssemblyName.GetAssemblyName("TextWindow.exe"));


            //создаем обьекты окон на основе оконных типов данных из загруженных сборок
            DrawerWindow = Activator.CreateInstance(TextDrawerAsm.GetType("TextDrawer.Form1")) as Form;
            TextWindowWnd = Activator.CreateInstance(TextWindowAsm.GetType("TextWindow.Form1"),
                new object[]
                {
                    TextDrawerAsm.GetModule("TextDrawer.exe"), DrawerWindow
                }) as Form;

            //запускаем потоки
            (new Thread(new ThreadStart(RunVizualizer))).Start();
            (new Thread(new ThreadStart(RunDrawer))).Start();

            //добавляем обработчик события DomainUnload
            TextDrawer.DomainUnload += TextDrawer_DomainUnload;

        }

        private static void TextDrawer_DomainUnload(object sender, EventArgs e)
        {
            MessageBox.Show("Domain with name "+ (sender as AppDomain).FriendlyName + " has been succesfully unloaded!");
        }

        static void RunDrawer()
        {
            //запускаем окно модально в текущем потоке
            DrawerWindow.ShowDialog();

            //отгружаем домен приложения
            AppDomain.Unload(TextDrawer);
        }

        static void RunVizualizer()
        {
            TextWindowWnd.ShowDialog();

            //завершаем работу приложения, следствием чего стане закрытиие потока
            Application.Exit();
        }
    }
}
