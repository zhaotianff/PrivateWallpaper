using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PrivateWallpaper.Util
{
    public class ImageHelper
    {
        public static ImageSource GetImageSource(string filePath)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            bi.UriSource = new Uri(filePath,UriKind.Absolute);
            bi.EndInit();
            return bi;
        }
    }
}
