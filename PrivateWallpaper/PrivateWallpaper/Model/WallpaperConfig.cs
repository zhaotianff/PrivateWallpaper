using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateWallpaper.Model
{
    public class WallpaperConfig
    {
        public WallpaperType WallpaperType { get; set; } = WallpaperType.File;

        public string PublicFilePath { get; set; }

        public string PrivateFilePath { get; set; }

        public bool IsPrivate { get; set; } = false;
    }
}
