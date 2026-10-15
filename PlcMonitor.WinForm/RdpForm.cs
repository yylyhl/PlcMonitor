using Microsoft.Extensions.Logging;

namespace PlcMonitor.WinForm
{
    public partial class RdpForm : Form
    {
        private readonly ILogger<RdpForm> _logger;
        public RdpForm(ILogger<RdpForm> logger)
        {
            InitializeComponent();
            _logger = logger;
        }

        private void RdpForm_Load(object sender, EventArgs e)
        {
            Text = "Microsoft RDP Client Control";
            TB_Host.Text = "192.168.1.1";
            TB_Port.Text = "3389";
            TB_UserName.Text = "administrator";
            TB_Pwd.Text = "password";
            BTN_Disconnect.Enabled = false;
            BTN_Connect.Enabled = true;
        }

        private void BTN_Connect_Click(object sender, EventArgs e)
        {
            if (axMsRdpClient11NotSafeForScripting1.Connected != 0) return;
            if (string.IsNullOrWhiteSpace(TB_UserName.Text) || string.IsNullOrWhiteSpace(TB_Pwd.Text) || string.IsNullOrWhiteSpace(TB_Host.Text))
            {
                MessageBox.Show("参数不对");
                return;
            }
            if (!int.TryParse(TB_Port.Text, out int port) || port < 1 || port > 65536)
            {
                MessageBox.Show("端口不对");
                return;
            }
            AddOnChange();
            BTN_Connect.Enabled = false;
            var pwd = TB_Pwd.Text;
            axMsRdpClient11NotSafeForScripting1.UserName = TB_UserName.Text;//远程电脑的用户名
            axMsRdpClient11NotSafeForScripting1.Server = TB_Host.Text;//远程主机IP
            axMsRdpClient11NotSafeForScripting1.AdvancedSettings2.RDPPort = port;//远程桌面端口
            axMsRdpClient11NotSafeForScripting1.AdvancedSettings2.SmartSizing = true;
            axMsRdpClient11NotSafeForScripting1.AdvancedSettings9.NegotiateSecurityLayer = true;
            
            axMsRdpClient11NotSafeForScripting1.AdvancedSettings7.EnableCredSspSupport = true;// 启用CredSSP身份验证（win11需要开启这个）
            MSTSCLib.IMsTscNonScriptable securd = (MSTSCLib.IMsTscNonScriptable)axMsRdpClient11NotSafeForScripting1.GetOcx();
            securd.ClearTextPassword = pwd;//远程连接用户密码
            axMsRdpClient11NotSafeForScripting1.AdvancedSettings5.ClearTextPassword = pwd;
            axMsRdpClient11NotSafeForScripting1.ColorDepth = 32;//颜色深度
            axMsRdpClient11NotSafeForScripting1.Connect();//连接到远程桌面
            BTN_Disconnect.Enabled = true;
        }
        private void AddOnChange()
        {
            axMsRdpClient11NotSafeForScripting1.OnConnecting += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";
                _logger.LogInformation("OnConnecting>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnConnecting>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnConnected += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//与服务端 RDP 通道建立成功，不等于登录成功
                _logger.LogInformation("OnConnected>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnConnected>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnDisconnected += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.discReason}";//会话断开（主动 / 被动 / 异常）
                _logger.LogInformation("OnDisconnected>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnDisconnected>>{message}{Environment.NewLine}"));
            };

            axMsRdpClient11NotSafeForScripting1.OnLoginComplete += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//用户登录完成，进入桌面
                _logger.LogInformation("OnLoginComplete>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnLoginComplete>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnLogonEvent += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.logonEventType}";
                _logger.LogInformation("OnLogonEvent>>{message}", message);//登录失败，如：密码错/账户锁定/不允许远程登录
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnLogonEvent>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnLogonError += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.lError}";
                _logger.LogInformation("OnLogonError>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnLogonError>>{message}{Environment.NewLine}"));
            };

            axMsRdpClient11NotSafeForScripting1.OnWarning += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.warningCode}";//非致命警告
                _logger.LogInformation("OnWarning>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnWarning>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnFatalError += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.errorCode}";//致命错误，会话废了
                _logger.LogInformation("OnFatalError>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnFatalError>>{message}{Environment.NewLine}"));
            };


            axMsRdpClient11NotSafeForScripting1.OnIdleTimeoutNotification += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//空闲超时（服务端策略）
                _logger.LogInformation("OnIdleTimeoutNotification>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnIdleTimeoutNotification>>{message}{Environment.NewLine}"));
            };

            axMsRdpClient11NotSafeForScripting1.OnAuthenticationWarningDismissed += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//显示身份验证警告（比如 CredSSP）
                _logger.LogInformation("OnAuthenticationWarningDismissed>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnAuthenticationWarningDismissed>>{message}{Environment.NewLine}"));
            };

            axMsRdpClient11NotSafeForScripting1.OnAuthenticationWarningDisplayed += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//警告关掉
                _logger.LogInformation("OnAuthenticationWarningDisplayed>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnAuthenticationWarningDisplayed>>{message}{Environment.NewLine}"));
            };

            axMsRdpClient11NotSafeForScripting1.OnAutoReconnecting += (sender, e) =>
            {
                var message = $"{sender}|||{e.attemptCount},{e.disconnectReason}";//自动重连（旧版）
                _logger.LogInformation("OnAutoReconnecting>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnAutoReconnecting>>{message}{Environment.NewLine}"));
                return MSTSCLib.AutoReconnectContinueState.autoReconnectContinueAutomatic;
            };
            axMsRdpClient11NotSafeForScripting1.OnAutoReconnecting2 += (sender, e) =>
            {
                var message = $"{sender}|||{e.attemptCount},{e.disconnectReason}";//自动重连（新版）
                _logger.LogInformation("OnAutoReconnecting2>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnAutoReconnecting2>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnAutoReconnected += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//重连成功
                _logger.LogInformation("OnAutoReconnected>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnAutoReconnected>>{message}{Environment.NewLine}"));
            };


            axMsRdpClient11NotSafeForScripting1.OnConfirmClose += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs}";//关闭确认
                _logger.LogInformation("OnConfirmClose>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnConfirmClose>>{message}{Environment.NewLine}"));
                return true;
            };
            axMsRdpClient11NotSafeForScripting1.OnRemoteDesktopSizeChange += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.height},{eventArgs.width}";//远程桌面大小变化
                _logger.LogInformation("OnRemoteDesktopSizeChange>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnRemoteDesktopSizeChange>>{message}{Environment.NewLine}"));
            };
            axMsRdpClient11NotSafeForScripting1.OnNetworkStatusChanged += (sender, eventArgs) =>
            {
                var message = $"{sender}|||{eventArgs.bandwidth},{eventArgs.qualityLevel},{eventArgs.rtt}";//网络质量变化
                _logger.LogInformation("OnNetworkStatusChanged>>{message}", message);
                Invoke(() => TB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] OnNetworkStatusChanged>>{message}{Environment.NewLine}"));
            };
        }
        private void BTN_Disconnect_Click(object sender, EventArgs e)
        {
            if (axMsRdpClient11NotSafeForScripting1.Connected == 0) return;
            BTN_Disconnect.Enabled = false;
            BTN_Connect.Enabled = true;
            axMsRdpClient11NotSafeForScripting1.Disconnect();//断开远程桌面连接
        }
    }
}
