using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using NLog.Targets.Wrappers;

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
                        LogManager.Setup().LoadConfigurationFromSection(configuration, "NLog");//链式加载
                        //LogManager.Setup().LoadConfigurationFromSection(configuration);//链式加载
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
            var errorFileTarget = new FileTarget("error")
            {
                FileName = "logs/error-${shortdate}.txt",//${time} = ${date:format=HH:mm:ss.ffff}
                AutoFlush = false,
                KeepFileOpen = true,
                OpenFileFlushTimeout = 2,
                OpenFileCacheTimeout = 60,
                BufferSize = 32768,//32kb
                ArchiveFileName = "logs/nlog-${shortdate}.{#}.txt",
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 300,
                MaxArchiveDays = 30,
                ArchiveAboveSize = 1024 * 1024 * 10,
                Layout = "${time} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception:format=tostring}"
            };
            var errorFileTargetAsync = new AsyncTargetWrapper(errorFileTarget)
            {
                QueueLimit = 10000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Discard,
                BatchSize = 200,
                FullBatchSizeWriteLimit = 5,
                TimeToSleepBetweenBatches = 1,
            };
            config.AddTarget(errorFileTargetAsync);

            var warnFileTarget = new FileTarget("warn")
            {
                FileName = "logs/warn-${shortdate}.txt",
                AutoFlush = false,
                KeepFileOpen = true,
                OpenFileFlushTimeout = 2,
                OpenFileCacheTimeout = 60,
                BufferSize = 32768,//32kb
                ArchiveFileName = "logs/nlog-${shortdate}.{#}.txt",
                ArchiveEvery = FileArchivePeriod.Day,
                MaxArchiveFiles = 300,
                MaxArchiveDays = 30,
                ArchiveAboveSize = 1024 * 1024 * 10,
                Layout = "${time} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception:format=tostring}"
            };
            var warnFileTargetAsync = new AsyncTargetWrapper(warnFileTarget)
            {
                QueueLimit = 10000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Discard,
                BatchSize = 200,
                FullBatchSizeWriteLimit = 5,
                TimeToSleepBetweenBatches = 1,
            };
            config.AddTarget(warnFileTargetAsync);

            var fileTarget = new FileTarget("info")
            {
                FileName = "logs/nlog-${shortdate}.txt",
                AutoFlush = false,
                KeepFileOpen = true,
                OpenFileFlushTimeout = 2,
                OpenFileCacheTimeout = 60,
                BufferSize = 32768,//32kb
                ArchiveFileName = "logs/nlog-${shortdate}.{#}.txt",
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveAboveSize = 1024 * 1024 * 10,
                MaxArchiveFiles = 300,
                MaxArchiveDays = 30,
                Layout = "${time} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception:format=tostring}"
            };
            var fileTargetAsync = new AsyncTargetWrapper(fileTarget)
            {
                QueueLimit = 10000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Discard,
                BatchSize = 200,
                FullBatchSizeWriteLimit = 5,
                TimeToSleepBetweenBatches = 1,
            };
            config.AddTarget(fileTargetAsync);

            var consoleTarget = new ConsoleTarget("console");
            var consoleTargetAsync = new AsyncTargetWrapper(consoleTarget);
            config.AddTarget(consoleTargetAsync);

            config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, errorFileTargetAsync);
            config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Warn, warnFileTargetAsync);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileTargetAsync);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, consoleTargetAsync);
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
