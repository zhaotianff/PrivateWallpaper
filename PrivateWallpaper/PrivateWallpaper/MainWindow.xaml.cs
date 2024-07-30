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

        public MainWindow()
        {
            InitializeComponent();
            onAnimation = FindResource("SwitchOnAnimation") as Storyboard;
            offAnimation = FindResource("SwitchOffAnimation") as Storyboard;
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
                bi.UriSource = new Uri("pack://application:,,,/PrivateWallpaper;component/Styles/back.jpg", UriKind.Absolute);
                bi.EndInit();
                this.PART_Border.Background = new ImageBrush() { ImageSource = bi, Stretch = Stretch.UniformToFill };
            }
            else
            {
                offAnimation.Begin();
                this.PART_Border.Background = Brushes.White;
            }

        }
    }
}
