using PrivateWallpaper.Model;
using PrivateWallpaper.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public partial class WallpaperModeSelector : Window
    {
        private bool cancelCloseFlag = true;
        private static readonly string ByPassUACName = "ByPassUAC.exe";
        private static readonly string AdminTaskName = "PrivateWallpaper.AdminTask.exe";
        private static readonly string PrivateWallpaperName = "PrivateWallpaper.exe";

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
            var programPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var byPassUACPath = System.IO.Path.Combine(programPath, ByPassUACName);
            var adminTaskPath = System.IO.Path.Combine(programPath, AdminTaskName);
            var privateWallpaperPath = System.IO.Path.Combine(programPath, PrivateWallpaperName);

            if (System.IO.File.Exists(byPassUACPath) == false || System.IO.File.Exists(adminTaskPath) == false)
            {
                return;
            }

            var mode = "userinit";
            ProcessHelper.Execute(byPassUACPath, $"{adminTaskPath} {privateWallpaperPath} {mode}");
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
        }

        private void btn_Start_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pwd_PrivateMode.Password) && string.IsNullOrEmpty(pwd_PublicMode.Password))
            {
                MessageBox.Show("请输入对应模式的密码");
            }

            if (!string.IsNullOrEmpty(pwd_PublicMode.Password) && pwd_PublicMode.Password != "1")
            {
                MessageBox.Show("密码输入错误，请重新输入");
                return;
            }

            if (!string.IsNullOrEmpty(pwd_PrivateMode.Password) && pwd_PrivateMode.Password != "2")
            {
                MessageBox.Show("密码输入错误，请重新输入");
                return;
            }

            if (pwd_PublicMode.Password == "1")
            {
                RunPrivateWallpaper(false);
                RunExplorer();

                this.cancelCloseFlag = false;
                this.Close();
            }

            if(pwd_PrivateMode.Password == "2")
            {
                RunPrivateWallpaper(true);
                RunExplorer();

                this.cancelCloseFlag = false;
                this.Close();
            }
        }

        private void BlurWindow_Loaded(object sender, RoutedEventArgs e)
        {
            media.Play();
            media.MediaEnded += Media_MediaEnded;
        }

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Stop();
            media.Play();
        }
    }
}
