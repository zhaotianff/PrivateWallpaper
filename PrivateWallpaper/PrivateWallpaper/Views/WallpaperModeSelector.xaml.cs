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

            pwd_PublicMode.Focus();
        }

        private void BlurWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = cancelCloseFlag;
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

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if(pwd_PublicMode.Password == "1")
            {
                RunExplorer();
                RunPrivateWallpaper(false);
            }

            if(pwd_PrivateMode.Password == "2")
            {
                RunExplorer();
                RunPrivateWallpaper(true);
            }

            if(string.IsNullOrEmpty(pwd_PrivateMode.Password) && string.IsNullOrEmpty(pwd_PublicMode.Password))
            {
                MessageBox.Show("请输入对应模式的密码");
            }
            else
            {
                MessageBox.Show("密码错误，请重新输入");
            }
        }
    }
}
