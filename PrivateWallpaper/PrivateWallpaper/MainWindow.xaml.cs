using PrivateWallpaper.Model;
using PrivateWallpaper.PInvoke;
using PrivateWallpaper.Util;
using PrivateWallpaper.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using WindowsX.Shell.Util;
using static PrivateWallpaper.PInvoke.User32;

namespace PrivateWallpaper
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Storyboard onAnimation;
        private Storyboard offAnimation;

        private IntPtr g_eventhook;
        private User32.Wineventproc Wineventproc;

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

            if (privateWallpaperKey == null)
            {
                privateWallpaperKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Software\\PrivateWallpaper");
                privateWallpaperKey.SetValue("IsPrivate", "0", Microsoft.Win32.RegistryValueKind.DWord);
                privateWallpaperKey.SetValue("WallpaperType", "0", Microsoft.Win32.RegistryValueKind.DWord);
                privateWallpaperKey.SetValue("IsHideInFullscreen", "0", Microsoft.Win32.RegistryValueKind.DWord);
                privateWallpaperKey.SetValue("PrivateFilePath", "", Microsoft.Win32.RegistryValueKind.String);
                var localWallpaperPath = Wallpaper.Manager.GetCurrentWallpaper();
                wallpaperConfig.PublicFilePath = localWallpaperPath;
                privateWallpaperKey.SetValue("PublicFilePath", localWallpaperPath, Microsoft.Win32.RegistryValueKind.String);
                privateWallpaperKey.Dispose();
                return;
            }

            wallpaperConfig.IsPrivate = privateWallpaperKey.GetValue("IsPrivate").ToString() == "1";
            wallpaperConfig.IsHideInFullscreen = privateWallpaperKey.GetValue("IsHideInFullscreen").ToString() == "1";
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

        private async void SwitchToPrivateWallpaper()
        {
            if (string.IsNullOrEmpty(wallpaperConfig.PrivateFilePath))
                return;

            onAnimation.Begin();
            await Task.Delay(300);

            this.PART_Border.Background = new ImageBrush() { ImageSource = ImageHelper.GetBitmapImageFromDefaultResource("back.jpg"), Stretch = Stretch.UniformToFill };
            this.PART_Ellipse.Fill = new ImageBrush() { ImageSource = ImageHelper.GetBitmapImageFromDefaultResource("smilewithpleasure.png"), Stretch = Stretch.UniformToFill };

            Wallpaper.Manager.ChangeWallpaper(wallpaperConfig.PrivateFilePath);
            wallpaperConfig.IsPrivate = true;
        }

        private async void SwitchToPublicWallpaper()
        {
            offAnimation.Begin();
            await Task.Delay(300);

            this.PART_Border.Background = Brushes.White;
            this.PART_Ellipse.Fill = new ImageBrush() { ImageSource = ImageHelper.GetBitmapImageFromDefaultResource("smile.png"), Stretch = Stretch.UniformToFill };

            Wallpaper.Manager.ChangeWallpaper(wallpaperConfig.PublicFilePath);
            wallpaperConfig.IsPrivate = false;
        }

        private void RefreshWallpaper()
        {
            if(wallpaperConfig.IsPrivate)
            {
                Wallpaper.Manager.ChangeWallpaper(wallpaperConfig.PrivateFilePath);
            }
            else
            {
                Wallpaper.Manager.ChangeWallpaper(wallpaperConfig.PublicFilePath);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - 120;
            this.Top = SystemParameters.WorkArea.Height - 60;

            if (wallpaperConfig.IsPrivate)
            {
                SwitchToPrivateWallpaper();
            }
            else
            {
                SwitchToPublicWallpaper();
            }

            CreateNotifyIcon();
            RefreshSetting();
        }

        private void RefreshSetting()
        {
            if(wallpaperConfig.IsHideInFullscreen)
            {
                CreateMaximiumEventHook();
            }
        }

        private void CreateMaximiumEventHook()
        {
            if (g_eventhook != IntPtr.Zero)
                return;

            Wineventproc = new Wineventproc(EventProc);

            g_eventhook = User32.SetWinEventHook(User32.EVENT_OBJECT_LOCATIONCHANGE, 
                User32.EVENT_OBJECT_LOCATIONCHANGE, 
                IntPtr.Zero,
                Wineventproc, 0, 0, 
                User32.WINEVENT_OUTOFCONTEXT | User32.WINEVENT_SKIPOWNPROCESS);
        }

        private void ReleaseMaximiumEventHook()
        {
            if (g_eventhook == IntPtr.Zero)
                return;

            User32.UnhookWinEvent(g_eventhook);
        }

        private void EventProc(IntPtr hWinEventHook, uint eventId, IntPtr hwnd, int idObject, int idChild, uint idEventThread, uint dwmsEventTime)
        {
            if (hwnd == IntPtr.Zero)
                return;

            if (User32.GetForegroundWindow() != hwnd)
                return;

            if (User32.EVENT_OBJECT_LOCATIONCHANGE == eventId)
            {
                WINDOWPLACEMENT wp = new WINDOWPLACEMENT();
                wp.length = (uint)Marshal.SizeOf(wp);
                if (User32.GetWindowPlacement(hwnd, ref wp))  //Not very accurate
                {
                    if (User32.SW_SHOWMAXIMIZED == wp.showCmd)
                    {
                        this.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        private void CreateNotifyIcon()
        {
            NotifyIconCreateData data = new NotifyIconCreateData();
            data.ClickHandler = ShowOrHiderMainWindow;
            data.ContextMenu = this.TryFindResource("NotifyIconContextMenu") as ContextMenu;
            InitContextMenuEvent(data.ContextMenu);
            data.IconRelativePath = "logo.ico";
            data.Tooltip = "个人壁纸切换器";
            NotifyIconHelper.Instance.CreateNotifyIcon(data);
            NotifyIconHelper.Instance.SetNotifyIconState(true);
        }

        private void ShowOrHiderMainWindow()
        {
            this.Visibility = this.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void InitContextMenuEvent(ContextMenu contextMenu)
        {
            if (contextMenu == null)
                return;

            MenuItem exitMenu = contextMenu.Items[0] as MenuItem;
            exitMenu.Click += (a, b) =>
            {
                NotifyIconHelper.Instance.SetNotifyIconState(false);
                this.Close();
            };
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void PART_Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((double)PART_Ellipse.GetValue(Canvas.LeftProperty) == 0)
            {
                SwitchToPrivateWallpaper();
            }
            else
            {
                SwitchToPublicWallpaper();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveConfig();
            ReleaseMaximiumEventHook();
        }

        private void SaveConfig()
        {
            var privateWallpaperKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("Software\\PrivateWallpaper",true);
            privateWallpaperKey.SetValue("IsPrivate", wallpaperConfig.IsPrivate == true ? 1 : 0,Microsoft.Win32.RegistryValueKind.DWord);
            privateWallpaperKey.SetValue("WallpaperType", (int)wallpaperConfig.WallpaperType, Microsoft.Win32.RegistryValueKind.DWord);
            privateWallpaperKey.SetValue("IsHideInFullscreen", wallpaperConfig.IsHideInFullscreen == true ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
            privateWallpaperKey.SetValue("PrivateFilePath", wallpaperConfig.PrivateFilePath, Microsoft.Win32.RegistryValueKind.String);
            privateWallpaperKey.SetValue("PublicFilePath", wallpaperConfig.PublicFilePath, Microsoft.Win32.RegistryValueKind.String);
            privateWallpaperKey.Dispose();
        }

        private void OpenSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow setting = new SettingWindow(this.wallpaperConfig);
            if(setting.ShowDialog() == true)
            {
                RefreshWallpaper();
                RefreshSetting();
            }
        }
    }
}
