using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlcMonitor.WinForm
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;//全局服务容器
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //Application.Run(new MainForm());

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettingsSerilog.json", optional: false, reloadOnChange: true)
                .Build();

            var services = new ServiceCollection();
            services.AddLogging(factory => factory.AddScadaSerilog(configuration));
            services.AddLogging(factory => factory.AddScadaNLog());
            services.AddTransient<MainForm>();
            ServiceProvider = services.BuildServiceProvider();
            Application.Run(ServiceProvider.GetRequiredService<MainForm>());
            LoggingSerilogExtensions.CloseSerilog();
            LoggingNLogExtensions.CloseNLog();
        }
    }
}