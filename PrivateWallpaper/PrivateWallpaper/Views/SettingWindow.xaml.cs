using Microsoft.Win32;
using PrivateWallpaper.Model;
using PrivateWallpaper.PInvoke;
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
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : TianXiaTech.BlurWindow
    {
        private WallpaperConfig wallpaperConfig;
        private static readonly string AdminTaskName = "PrivateWallpaper.AdminTask.exe";
        private static readonly string ByPassUACName = "ByPassUAC.exe";
        private static readonly string PrivateWallpaperName = "PrivateWallpaper.exe";

        public SettingWindow(WallpaperConfig wallpaperConfig)
        {
            this.wallpaperConfig = wallpaperConfig;

            InitializeComponent();
            InitializeUI();
            InitializeSetting();
        }

        private void InitializeUI()
        {
            //TODO

            var img = this.btn_InstallStartup.FindName("img_Icon") as Image;
            img.Source = UACIcon.GetUACIcon();
            var uninstallImg = this.btn_UnInstallStartup.FindName("img_UninstallIcon") as Image;
            uninstallImg.Source = UACIcon.GetUACIcon();
        }

        private void InitializeSetting()
        {
            if(wallpaperConfig.WallpaperType == WallpaperType.File)
            {
                this.radio_File.IsChecked = true;
            }

            if(!string.IsNullOrEmpty(wallpaperConfig.PublicFilePath))
            {
                this.img_wallpaper1.Source = ImageHelper.GetImageSource(wallpaperConfig.PublicFilePath);
            }

            if (!string.IsNullOrEmpty(wallpaperConfig.PrivateFilePath))
            {
                this.img_wallpaper2.Source = ImageHelper.GetImageSource(wallpaperConfig.PrivateFilePath);
            }

            if(wallpaperConfig.IsHideInFullscreen)
            {
                this.cbx_HideInFullScreen.IsChecked = true;
            }
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnImg1Browser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp:*.tiff;*.wbep";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if(openFileDialog.ShowDialog() == true)
            {
                this.img_wallpaper1.Source = ImageHelper.GetImageSource(openFileDialog.FileName);
                this.wallpaperConfig.PublicFilePath = openFileDialog.FileName;

            }
        }

        private void BtnImg2Browser_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp:*.tiff;*.wbep";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (openFileDialog.ShowDialog() == true)
            {
                this.img_wallpaper2.Source = ImageHelper.GetImageSource(openFileDialog.FileName);
                this.wallpaperConfig.PrivateFilePath = openFileDialog.FileName;

            }
        }

        private void cbx_HideInFullScreen_Checked(object sender, RoutedEventArgs e)
        {
            wallpaperConfig.IsHideInFullscreen = true;
        }

        private void cbx_HideInFullScreen_Unchecked(object sender, RoutedEventArgs e)
        {
            wallpaperConfig.IsHideInFullscreen = false;
        }

        private void btn_InstallStartup_Click(object sender, RoutedEventArgs e)
        {
            InstallOrUninstallStartup(true);
        }

        private void btn_UnInstallStartup_Click(object sender, RoutedEventArgs e)
        {
            InstallOrUninstallStartup(false);
        }

        private void InstallOrUninstallStartup(bool isInstall)
        {
            var programDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var byPassUACPath = System.IO.Path.Combine(programDir, ByPassUACName);
            var adminTaskPath = System.IO.Path.Combine(programDir, AdminTaskName);
            var privateWallpaperPath = System.IO.Path.Combine(programDir, PrivateWallpaperName);

            if (System.IO.File.Exists(byPassUACPath) == false || System.IO.File.Exists(adminTaskPath) == false)
                return;

            try
            {
                var mode = "install";

                if(isInstall == false)
                {
                    mode = "uninstall";
                }

                System.Diagnostics.Process.Start(byPassUACPath, $"{adminTaskPath} {privateWallpaperPath} {mode}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool GetStartupState()
        {
            return true;
        }
        
    }
}
