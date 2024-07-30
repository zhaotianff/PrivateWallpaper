using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PrivateWallpaper.PInvoke
{
    public class User32
    {
        public static readonly uint SPI_SETDESKWALLPAPER = 0x0014;
        public static readonly uint SPIF_UPDATEINIFILE = 0x0001;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam,string lpImagePath, uint fWinIni);
    }
}
