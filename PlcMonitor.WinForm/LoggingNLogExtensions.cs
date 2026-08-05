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
            builder.AddNLog(new NLogProviderOptions
            {
                //RemoveLoggerFactoryFilter = false // 读取appsettings的LogLevel覆盖规则
            });// 桥接到Microsoft.Extensions.Logging抽象
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
                Layout = "${time} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message} ${exception}"
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
                Layout = "${time} [${level:uppercase=true}] Thread:${threadid} ${logger}: ${message}"
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
                Layout = "${time} [${level:uppercase=true}] Thread:${threadid} ${message}"
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

            var mongoAsyncTargetWrapper = MongoAsyncTargetWrapper();
            config.AddTarget("mongo", mongoAsyncTargetWrapper);

            #region 过滤规则放最前面，匹配后终止后续规则，等价Override抬高最低级别
            var nullTarget = new NullTarget("null");
            //config.AddTarget(nullTarget);
            config.LoggingRules.Add(new LoggingRule("Microsoft.AspNetCore.*", NLog.LogLevel.Warn, nullTarget) { Final = true, FinalMinLevel = NLog.LogLevel.Warn });
            config.LoggingRules.Add(new LoggingRule("Microsoft.Hosting.Lifetime.*", NLog.LogLevel.Info, nullTarget) { Final = true, FinalMinLevel = NLog.LogLevel.Info });
            config.LoggingRules.Add(new LoggingRule("System.*", NLog.LogLevel.Warn, nullTarget) { Final = true, FinalMinLevel = NLog.LogLevel.Warn });
            #endregion
            config.AddRule(NLog.LogLevel.Error, NLog.LogLevel.Fatal, errorFileTargetAsync);
            config.AddRule(NLog.LogLevel.Warn, NLog.LogLevel.Warn, warnFileTargetAsync);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileTargetAsync);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, consoleTargetAsync);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, mongoAsyncTargetWrapper);
            return config;
        }

        #region 写mongodb
        private static AsyncTargetWrapper MongoAsyncTargetWrapper()
        {
            var mongoTargetAsync = new AsyncTargetWrapper(MongoTarget())
            {
                QueueLimit = 10000,
                OverflowAction = AsyncTargetWrapperOverflowAction.Discard,
                BatchSize = 200,
                FullBatchSizeWriteLimit = 5,
                TimeToSleepBetweenBatches = 1,
            };
            return mongoTargetAsync;
        }
        private static NLog.Mongo.MongoTarget MongoTarget()
        {
            var mongoTarget = new NLog.Mongo.MongoTarget()
            {
                //Name = "Mongo",
                ConnectionString = "mongodb://admin:adminpassword@192.168.100.209:27017?authSource=admin",
                DatabaseName = "TransLog",
                CollectionName = "PLC",
                IncludeDefaults = false,
                //CappedCollectionSize = 1024576000,
                //CappedCollectionMaxItems = 1000000,
            };
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "logtime", Layout = "${longdate}" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "processName", Layout = "${processname:fullName=false}" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "hostname", Layout = "${hostname}" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "processId", Layout = "${processid}", BsonType = "Int32" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "thread", Layout = "${threadid}", BsonType = "Int32" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "level", Layout = "${level}" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "message", Layout = "${message}" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "logger", Layout = "${logger}" });
            mongoTarget.Fields.Add(new NLog.Mongo.MongoField { Name = "error", Layout = "${exception}" });
            #region 增加property扩展字段（对应xml <property/>，aspnet web字段，需要NLog.Web.AspNetCore）
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "RequestIP", Layout = "${aspnet-request-ip}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "UserName", Layout = "${aspnet-user-identity}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "BaseDir", Layout = "${aspnet-appbasepath}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "QueryUrl", Layout = "${aspnet-request-url}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "Method", Layout = "${aspnet-request-method}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "Controller", Layout = "${aspnet-mvc-controller}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "Action", Layout = "${aspnet-mvc-action}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "FormContent", Layout = "${aspnet-request-form}" });
            //mongoTarget.Properties.Add(new NLog.Mongo.MongoField { Name = "QueryContent", Layout = "${aspnet-request-querystring}" }); 
            #endregion

            return mongoTarget;
        }
        #endregion

        /// <summary>
        /// 释放NLog
        /// </summary>
        public static void CloseNLog()
        {
            LogManager.Shutdown();
        }
    }
}
