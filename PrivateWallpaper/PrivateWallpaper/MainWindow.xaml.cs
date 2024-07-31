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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrivateWallpaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard onAnimation;
        private Storyboard offAnimation;

        private WallpaperConfig wallpaperConfig = new WallpaperConfig();

        public MainWindow()
        {
            InitializeComponent();
            InitializeAnimation();
            LoadConfig();
        }

        private void LoadConfig()
        {
            var privateWallpaperKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\PrivateWallpaper");

            if(privateWallpaperKey == null)
            {
                privateWallpaperKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\PrivateWallpaper");
                privateWallpaperKey.SetValue("IsPrivate", "0", Microsoft.Win32.RegistryValueKind.DWord);
                privateWallpaperKey.SetValue("WallpaperType", "0", Microsoft.Win32.RegistryValueKind.DWord);
                privateWallpaperKey.SetValue("PrivateFilePath", "", Microsoft.Win32.RegistryValueKind.String);
                privateWallpaperKey.SetValue("PublicFilePath", "", Microsoft.Win32.RegistryValueKind.String);
                privateWallpaperKey.Dispose();
                return;
            }

            wallpaperConfig.IsPrivate = privateWallpaperKey.GetValue("IsPrivate").ToString() == "1";
            wallpaperConfig.WallpaperType = (WallpaperType)(privateWallpaperKey.GetValue("WallpaperType"));
            wallpaperConfig.PrivateFilePath = privateWallpaperKey.GetValue("PrivateFilePath").ToString();
            wallpaperConfig.PublicFilePath = privateWallpaperKey.GetValue("PublicFilePath").ToString();

            privateWallpaperKey.Dispose();
        }

        private void InitializeAnimation()
        {
            onAnimation = FindResource("SwitchOnAnimation") as Storyboard;
            offAnimation = FindResource("SwitchOffAnimation") as Storyboard;
        }

        private void SwitchToPrivateWallpaper()
        {
            Wallpaper.Manager.ChangeWallpaper(wallpaperConfig.PrivateFilePath);
        }

        private void SwitchToPublicWallpaper()
        {
            Wallpaper.Manager.ChangeWallpaper(wallpaperConfig.PublicFilePath);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - 120;
            this.Top = SystemParameters.WorkArea.Height - 60;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private async void PART_Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((double)PART_Ellipse.GetValue(Canvas.LeftProperty) == 0)
            {
                onAnimation.Begin();
                await Task.Delay(300);
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri("pack://application:,,,/PrivateWallpaper;component/Resources/back.jpg", UriKind.Absolute);
                bi.EndInit();
                this.PART_Border.Background = new ImageBrush() { ImageSource = bi, Stretch = Stretch.UniformToFill };
                SwitchToPrivateWallpaper();
            }
            else
            {
                offAnimation.Begin();
                await Task.Delay(300);
                this.PART_Border.Background = Brushes.White;
                SwitchToPublicWallpaper();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveConfig();
        }

        private void SaveConfig()
        {
            var privateWallpaperKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\PrivateWallpaper");
            privateWallpaperKey.SetValue("IsPrivate", wallpaperConfig.IsPrivate == true ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
            privateWallpaperKey.SetValue("WallpaperType", (int)wallpaperConfig.WallpaperType, Microsoft.Win32.RegistryValueKind.DWord);
            privateWallpaperKey.SetValue("PrivateFilePath", wallpaperConfig.PrivateFilePath, Microsoft.Win32.RegistryValueKind.String);
            privateWallpaperKey.SetValue("PublicFilePath", wallpaperConfig.PublicFilePath, Microsoft.Win32.RegistryValueKind.String);
            privateWallpaperKey.Dispose();
        }
    }
}
