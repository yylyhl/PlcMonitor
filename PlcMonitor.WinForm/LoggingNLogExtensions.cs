using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;

namespace PlcMonitor.WinForm
{
    public static class LoggingNLogExtensions
    {
        /// <summary>
        /// 初始化NLog并注入DI
        /// </summary>
        public static ILoggingBuilder AddMonitorNLog(this ILoggingBuilder builder, IConfiguration configuration)
        {
            string configPath = Path.Combine(AppContext.BaseDirectory, "nlog.config");
            if (File.Exists(configPath))
            {
                try
                {
                    //LogManager.Configuration = new XmlLoggingConfiguration(configPath);//直接赋值
                    LogManager.Setup().LoadConfigurationFromXml(configPath);//链式加载
                }
                catch
                {
                    LogManager.Configuration = DefaultConfig();
                }
            }
            else
            {
                var nlogSection = configuration.GetSection("NLog");
                if (nlogSection.Exists())
                {
                    try
                    {
                        //LogManager.Configuration = new NLogLoggingConfiguration(nlogSection);//直接赋值
                        //LogManager.ReconfigExistingLoggers();
                        LogManager.Setup().LoadConfigurationFromSection(configuration, "NLog");//链式加载
                    }
                    catch
                    {
                        LogManager.Configuration = DefaultConfig();
                    }
                }
                else
                {
                    LogManager.Configuration = DefaultConfig();
                }
            }

            //builder.ClearProviders();
            builder.AddNLog();// 桥接到Microsoft.Extensions.Logging抽象
            return builder;
        }
        private static LoggingConfiguration DefaultConfig()
        {
            var config = new LoggingConfiguration();

            // 文件滚动目标
            var errFileTarget = new FileTarget("error")
            {
                FileName = "logs/error-${shortdate}.log",
                AutoFlush = false,
                KeepFileOpen = true,
                OpenFileFlushTimeout = 2,
                OpenFileCacheTimeout = 60,
                BufferSize = 32768,//32kb
                ArchiveFileName = "logs/nlog-${shortdate}.{#}.log",
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 300,
                MaxArchiveDays = 30,
                ArchiveAboveSize = 1024 * 1024 * 10,
                Layout = "${longdate} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception:format=tostring}"
            };
            config.AddTarget(errFileTarget);

            var warnFileTarget = new FileTarget("warn")
            {
                FileName = "logs/warn-${shortdate}.log",
                AutoFlush = false,
                KeepFileOpen = true,
                OpenFileFlushTimeout = 2,
                OpenFileCacheTimeout = 60,
                BufferSize = 32768,//32kb
                ArchiveFileName = "logs/nlog-${shortdate}.{#}.log",
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 300,
                MaxArchiveDays = 30,
                ArchiveAboveSize = 1024 * 1024 * 10,
                Layout = "${longdate} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception:format=tostring}"
            };
            config.AddTarget(warnFileTarget);

            var fileTarget = new FileTarget("info")
            {
                FileName = "logs/nlog-${shortdate}.log",
                AutoFlush = false,
                KeepFileOpen = true,
                OpenFileFlushTimeout = 2,
                OpenFileCacheTimeout = 60,
                BufferSize = 32768,//32kb
                ArchiveFileName = "logs/nlog-${shortdate}.{#}.log",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveAboveSize = 1024 * 1024 * 10,
                MaxArchiveFiles = 300,
                MaxArchiveDays = 30,
                Layout = "${longdate} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception:format=tostring}"
            };
            config.AddTarget(fileTarget);

            var consoleTarget = new ConsoleTarget("console");
            config.AddTarget(consoleTarget);

            config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, errFileTarget);
            config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Warn, warnFileTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, consoleTarget);
            return config;
        }

        /// <summary>
        /// 释放NLog
        /// </summary>
        public static void CloseNLog()
        {
            LogManager.Shutdown();
        }
    }
}
