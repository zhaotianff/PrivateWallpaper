using PrivateWallpaper.PInvoke;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PrivateWallpaper.Wallpaper
{
    public class Manager
    {
        public static void ChangeWallpaper(string path)
        {
            User32.SystemParametersInfo(User32.SPI_SETDESKWALLPAPER, 0, path, User32.SPIF_UPDATEINIFILE);
        }
    }
}
