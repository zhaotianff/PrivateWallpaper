using PrivateWallpaper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrivateWallpaper.Views
{
    /// <summary>
    /// WallpaperModeSelector.xaml 的交互逻辑
    /// </summary>
    public partial class WallpaperModeSelector : TianXiaTech.BlurWindow
    {
        private bool cancelCloseFlag = true;

        public WallpaperModeSelector()
        {
            InitializeComponent();
        }

        private void BlurWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = cancelCloseFlag;
        }

        private void btn_PrivateWallpaperMode_Click(object sender, RoutedEventArgs e)
        {
            RunExplorer();
            RunPrivateWallpaper(true);
        }

        private void btn_PublicWallpaperMode_Click(object sender, RoutedEventArgs e)
        {
            RunExplorer();
            RunPrivateWallpaper(false);
        }

        private void RunExplorer()
        {
            var systemRoot = Environment.GetFolderPath(Environment.SpecialFolder.System);
            var explorer = System.IO.Path.Combine(systemRoot, "explorer.exe");
            System.Diagnostics.ProcessStartInfo psInfo = new System.Diagnostics.ProcessStartInfo();
            psInfo.FileName = explorer;
            psInfo.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
            System.Diagnostics.Process.Start(psInfo);
        }

        private void RunPrivateWallpaper(bool isPrivateWallpaper)
        {
            var privateWallpaperKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\PrivateWallpaper", true);

            if (privateWallpaperKey != null)
            {
                privateWallpaperKey.SetValue("IsPrivate", isPrivateWallpaper ? "1" : "0", Microsoft.Win32.RegistryValueKind.DWord);
                privateWallpaperKey.Dispose();
            }

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.cancelCloseFlag = false;
            this.Close();
        }
    }
}
