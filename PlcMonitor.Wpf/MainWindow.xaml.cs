using Microsoft.Extensions.DependencyInjection;
using System.Windows;

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
    }
}