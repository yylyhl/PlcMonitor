using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace PlcMonitor.WinForm
{
    public static class LoggingSerilogExtensions
    {
        /// <summary>
        /// 初始化Serilog全局实例，并注入到DI
        /// </summary>
        public static ILoggingBuilder AddMonitorSerilog(this ILoggingBuilder builder, IConfiguration configuration)
        {
            //Serilog.Debugging.SelfLog.Enable(msg => System.IO.File.AppendAllText("logs/serilog-selflog.txt", msg + Environment.NewLine));
            // 全局Serilog配置，集中管理，业务层无感知
            if (configuration.GetSection("Serilog").Exists())
            {
                Log.Logger = new LoggerConfiguration()
                    .Enrich.FromLogContext()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
            }
            else
            {
                Log.Logger = DefaultConfig().CreateLogger();
            }

            builder.AddSerilog(dispose: true);// 桥接到Microsoft.Extensions.Logging抽象
            return builder;
        }
        /// <summary>
        /// 默认日志预设
        /// </summary>
        private static LoggerConfiguration DefaultConfig()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Debug()//{Message:lj}// lj = literal json，消息自动转义换行、引号，打印干净单行
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                //.WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {ThreadId} {SourceContext} | {Message:lj}{NewLine}{Exception}")
                .WriteTo.File(
                    //formatter: new Serilog.Formatting.Json.JsonFormatter(),
                    path: "logs/error-.log",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1024 * 1024 * 10,  // 10MB
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: LogEventLevel.Error,
                    buffered: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(2),
                    encoding: new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] Thread:{ThreadId} {SourceContext}: {Message}{NewLine}{Properties}{NewLine}{Exception}"
                ).WriteTo.Logger(lc => lc.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning).WriteTo.File(
                    //formatter: new Serilog.Formatting.Json.JsonFormatter(),
                    path: "logs/warn-.log",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1024 * 1024 * 10,  // 10MB
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: LogEventLevel.Warning,
                    buffered: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(2),
                    encoding: new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] Thread:{ThreadId} {SourceContext}: {Message}{NewLine}{Properties}{NewLine}{Exception}"
                )).WriteTo.File(
                    //formatter: new Serilog.Formatting.Json.JsonFormatter(),
                    path: "logs/serilog-.log",
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 1024 * 1024 * 10,  // 10MB
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 31,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    buffered: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(2),
                    encoding: new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] Thread:{ThreadId} {SourceContext}: {Message}{NewLine}{Properties}{NewLine}{Exception}"
                );
        }

        /// <summary>
        /// 释放Serilog资源（程序退出调用）
        /// </summary>
        public static void CloseSerilog()
        {
            Log.CloseAndFlush();
        }
    }
}
