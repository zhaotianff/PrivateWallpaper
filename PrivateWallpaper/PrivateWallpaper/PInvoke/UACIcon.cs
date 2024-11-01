using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;

namespace PrivateWallpaper.PInvoke
{
    public class UACIcon
    {
        public const int MAX_PATH = 260;

        [DllImport("Shell32.dll", SetLastError = false)]
        public static extern Int32 SHGetStockIconInfo(SHSTOCKICONID siid, SHGSI uFlags, ref SHSTOCKICONINFO psii);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool DestroyIcon(IntPtr hIcon);

        public static BitmapSource GetUACIcon()
        {
            BitmapSource shieldSource = null;

            SHSTOCKICONINFO sii = new SHSTOCKICONINFO();
            sii.cbSize = (UInt32)Marshal.SizeOf(typeof(SHSTOCKICONINFO));

            Marshal.ThrowExceptionForHR(SHGetStockIconInfo(SHSTOCKICONID.SIID_SHIELD,
                SHGSI.SHGSI_ICON | SHGSI.SHGSI_LARGEICON,
                ref sii));

            shieldSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                sii.hIcon,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            DestroyIcon(sii.hIcon);
            return shieldSource;
        }
    }
}
