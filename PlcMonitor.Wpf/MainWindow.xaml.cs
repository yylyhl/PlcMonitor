using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PlcMonitor.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Services.GetRequiredService<MainViewModel>();
        }

        #region 自定义标题栏
        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;// 双击最大化/还原
                //ButtonMaximize.Content = WindowState == WindowState.Maximized ? "❐" : "□";// 切换按钮符号/□/❐
                UpdateMaximizePath(WindowState == WindowState.Maximized);
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
            //((Button)sender).Content = WindowState == WindowState.Maximized ? "❐" : "□";// 切换按钮符号
            UpdateMaximizePath(WindowState == WindowState.Maximized);
        }
        private void UpdateMaximizePath(bool isMaximized)
        {
            if (isMaximized)
            {
                ButtonMaximizePath.Data = Geometry.Parse("M 2,0 H 10 V 8 H 2 Z M 0,2 H 8 V 10 H 0 Z");// 还原图标：小方框
            }
            else
            {
                ButtonMaximizePath.Data = Geometry.Parse("M0,0 H10 V10 H0 Z");// 最大化图标：大方框
            }
        }

        // 关闭窗口
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}