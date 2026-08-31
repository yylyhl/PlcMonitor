using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlcMonitor.Wpf
{
    /// <summary>
    /// TitleBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class ControlTitleBar : UserControl
    {

        public ControlTitleBar()
        {
            InitializeComponent();
            // 监听依赖属性变化，同步到UI
            Loaded += (s, e) =>
            {
                UpdateBindings();
            };
        }


        private void UpdateBindings()
        {
            TitleText.Text = Title;
            TitleBarBorder.Background = TitleBarBackground;

            if (IconSource != null)
            {
                IconImage.Source = IconSource;
                IconImage.Visibility = Visibility.Visible;
            }
        }
        #region 依赖属性
        /// <summary>标题文字</summary>
        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(ControlTitleBar), new PropertyMetadata("应用程序"));

        /// <summary>标题栏背景色</summary>
        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register(nameof(TitleBarBackground), typeof(Brush), typeof(ControlTitleBar), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(45, 45, 55))));

        /// <summary></summary>
        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }
        public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register(nameof(IconSource), typeof(ImageSource), typeof(ControlTitleBar), new PropertyMetadata(null, OnIconChanged));
        private static void OnIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ControlTitleBar bar && bar.IconImage != null)
            {
                bar.IconImage.Source = e.NewValue as ImageSource;
                bar.IconImage.Visibility = e.NewValue != null ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        #endregion

        private Window? GetParentWindow()
        {
            return Window.GetWindow(this);
        }
        private void UpdateMaximizeIcon(WindowState state)
        {
            if (state == WindowState.Maximized)
            {
                MaximizePath.Data = Geometry.Parse("M 2,0 H 10 V 8 H 2 Z M 0,2 H 8 V 10 H 0 Z");
                MaximizeButton.ToolTip = "还原";// 还原图标：两个重叠的小方框
            }
            else
            {
                MaximizePath.Data = Geometry.Parse("M0,0 H10 V10 H0 Z");// 最大化图标：单个大方框
                MaximizeButton.ToolTip = "最大化";
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = GetParentWindow();
            if (window != null) window.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            var window = GetParentWindow();
            if (window != null)
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
                UpdateMaximizeIcon(window.WindowState);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            GetParentWindow()?.Close();
        }
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var window = GetParentWindow();
            if (window == null) return;
            if (e.ClickCount == 2)
            {
                window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized; UpdateMaximizeIcon(window.WindowState);
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                GetParentWindow()?.DragMove();
            }
        }
    }
}
