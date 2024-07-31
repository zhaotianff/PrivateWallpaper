using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WindowsX.Shell
{
    class Startup
    {
        private static readonly string MutexName = "MMutex";

        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            System.Threading.Mutex mutex = new System.Threading.Mutex(true, MutexName, out createdNew);
            if (createdNew)
            {
                PrivateWallpaper.App app = new PrivateWallpaper.App();
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
