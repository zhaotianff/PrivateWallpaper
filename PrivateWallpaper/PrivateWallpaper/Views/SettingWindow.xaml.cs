using Microsoft.Win32;
using PrivateWallpaper.Model;
using PrivateWallpaper.Util;
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
    /// SettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWindow : TianXiaTech.BlurWindow
    {
        private WallpaperConfig wallpaperConfig;

        public SettingWindow(WallpaperConfig wallpaperConfig)
        {
            this.wallpaperConfig = wallpaperConfig;

            InitializeComponent();
            InitializeSetting();
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
    }
}
