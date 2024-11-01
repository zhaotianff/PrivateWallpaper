﻿using PrivateWallpaper;
using PrivateWallpaper.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace WindowsX.Shell
{
    class Startup
    {
        private static readonly string MutexName = "MMutex";
        private static System.Threading.Mutex mutex;

        [STAThread]
        static void Main(string[] args)
        {
            bool createdNew;
            mutex = new System.Threading.Mutex(true, MutexName, out createdNew);
            if (createdNew)
            {
                Application application = new Application();

                if (args.Length > 0 && args[0].ToUpper() == "SELECTOR")
                {
                    application.Run(new WallpaperModeSelector());
                }
                else
                {
                    application.Run(new MainWindow());
                }

            }
            else
            {
                //TODO activate window
            }
        }
    }
}
