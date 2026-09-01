using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace PlcMonitor.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ServiceProvider Services { get; private set; } = null;// new ServiceProvider();
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(factory => factory.AddMonitorSerilog());
            serviceCollection.AddLogging(factory => factory.AddMonitorNLog());
            serviceCollection.AddTransient<MainViewModel>();
            Services = serviceCollection.BuildServiceProvider();
            base.OnStartup(e);
        }
    }

}
