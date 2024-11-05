using PrivateWallpaper;
using PrivateWallpaper.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WindowsX.Shell
{
    class Startup
    {
        private static readonly string MutexName = "PrivateWallpaperMutex";
        private static System.Threading.Mutex mutex;

        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            mutex = new System.Threading.Mutex(true, MutexName, out createdNew);
            if (createdNew)
            {
                App app = new App();
                
                if (args.Length > 0 && args[0].ToUpper() == "SELECTOR")
                {
                    app.StartupUri = new Uri("Views/WallpaperModeSelector.xaml", UriKind.Relative);
                }
                else
                {
                    app.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
                }

                app.InitializeComponent();
                app.Run();
            }
            else
            {
                //TODO activate window
            }
        }
    }
}
