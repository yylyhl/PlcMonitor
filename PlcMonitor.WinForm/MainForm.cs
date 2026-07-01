using Microsoft.Extensions.Logging;
using PlcMonitor.Core;
using PlcMonitor.Core.Slave;
using System.Net.Sockets;

namespace PlcMonitor.WinForm
{
    public partial class MainForm : Form
    {
        private readonly ILogger<MainForm> _logger;
        private readonly System.Windows.Forms.Timer _timer;
        public MainForm(ILogger<MainForm> logger)
        {
            InitializeComponent();
            _logger = logger;
            Text = "PLC Monitor Form";
            Location = new Point(100, 100);
            //Width = 800;
            //Height = 600;

            InitControlsModbusMasterTcp();
            InitControlsModbusMasterSerialPort();
            InitControlsModbusSlaveTcp();
            InitControlsModbusSlaveSerialPort();

            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += Timer_Tick;
            //ftimer.Start();
            _logger.LogWarning("MainForm init done");
            WriteLog("MainForm init done");
        }
        private void WriteLog(string message)
        {
            txtComLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            #region 倒序，新的在上面
            //txtComLog.SuspendLayout();//暂停绘制防闪烁
            //txtComLog.SelectionStart = 0;//光标移到最开头，插入文本
            //txtComLog.SelectedText = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            //txtComLog.ResumeLayout(); 
            #endregion
            _logger.LogInformation(message);
        }

        private static readonly Random _random = new();
        private void Timer_Tick(object? sender, EventArgs e)
        {
            // TODO: 这里放 Modbus / OPC UA 读值
            statusMasterTcp.Text = $"温度：{_random.Next(20, 30)} °C";
        }

        #region Modbus Master Tcp
        private void InitControlsModbusMasterTcp()
        {
            btnConnectTcp.Click += btnConnectTcp_Click;
            Controls.Add(btnConnectSerial);
            btnDisconnectTcp.Click += btnDisconnectTcp_Click;
            Controls.Add(btnDisconnectTcp);
        }
        private void statusSlaveServerTcpTxt(string message)
        {
            statusSlaveServerTcp.Text = $"[statusMasterTcp]---{message}";
            WriteLog($"[statusMasterTcp]---{message}");
        }
        ICommunicationClient _modbusTcpClient;
        private async void btnDisconnectTcp_Click(object sender, EventArgs e)
        {
            if (!_modbusTcpClient.IsConnected) return;
            await _modbusTcpClient.DisconnectAsync();
            await Task.Delay(1500);
            btnConnectTcp.Enabled = true;
            txtConnectTcpStationNo.Enabled = true;
            txtConnectTcpHost.Enabled = true;
            txtConnectTcpPort.Enabled = true;
            statusMasterTcp.Text = $"状态：已断开连接";
            WriteLog($"[statusMasterTcp]状态：已断开连接");
        }
        private async void btnConnectTcp_Click(object sender, EventArgs e)
        {
            var slaveHost = txtConnectTcpHost.Text ?? "127.0.0.1";
            txtConnectTcpHost.Enabled = false;
            _ = int.TryParse(txtConnectTcpPort.Text, out var port);
            txtConnectTcpPort.Enabled = false;
            _ = byte.TryParse(txtConnectTcpStationNo.Text, out var slaveId);
            txtConnectTcpStationNo.Enabled = false;
            btnConnectTcp.Enabled = false;
            statusMasterTcp.Text = "状态：连接中...";
            WriteLog($"[statusMasterTcp]状态：连接中...");
            var device = new Device { DeviceType = DeviceType.ModbusTcp, IpAddress = slaveHost, StationNo = slaveId, Port = port };
            _modbusTcpClient = CommunicationClientFactory.CreateClient(device);
            var ress = await _modbusTcpClient.ConnectAsync();
            if (!ress.Success)
            {
                btnConnectTcp.Enabled = true;
                txtConnectTcpStationNo.Enabled = true;
                txtConnectTcpHost.Enabled = true;
                txtConnectTcpPort.Enabled = true;
                statusMasterTcp.Text = $"状态：[{ress.ErrorMessage}]";
                WriteLog($"[statusMasterTcp]状态：[{ress.ErrorMessage}]");
                return;
            }
            statusMasterTcp.Text = "状态：已连接";
            WriteLog($"[statusMasterTcp]状态：已连接");
            await Task.Delay(500);
            //_timer.Start();
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (btnConnectTcp.Enabled) break;
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusTcpClient.WriteAsync("HR0", DataPointType.Float, randData);
                    var readData = await _modbusTcpClient.ReadAsync("HR0", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterTcp.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                        WriteLog($"[statusMasterTcp]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                        //Timer_Tick(default, default);
                    });
                    Thread.Sleep(1000);
                }
            });
        }
        #endregion

        #region Modbus Master SerialPort
        private void InitControlsModbusMasterSerialPort()
        {
            btnConnectSerial.Click += btnConnectSerial_Click;
            Controls.Add(btnConnectSerial);
            btnDisconnectSerial.Click += btnDisconnectSerial_Click;
            Controls.Add(btnDisconnectSerial);
            comboBoxConnectSerialMode.Items.Add(SerialMode.RTU);
            comboBoxConnectSerialMode.Items.Add(SerialMode.ASCII);
            comboBoxConnectSerialMode.SelectedIndex = 0;
            Controls.Add(comboBoxConnectSerialMode);
        }
        ICommunicationClient _modbusSerialPortClient;
        private async void btnDisconnectSerial_Click(object sender, EventArgs e)
        {
            if (!_modbusSerialPortClient.IsConnected) return;
            await _modbusSerialPortClient.DisconnectAsync();
            await Task.Delay(1500);
            btnConnectSerial.Enabled = true;
            txtConnectSerialStationNo.Enabled = true;
            txtConnectSerialPortName.Enabled = true;
            comboBoxConnectSerialMode.Enabled = true;
            statusMasterSerial.Text = $"状态：已断开连接";
            WriteLog($"[statusMasterSerial]状态：已断开连接");
        }
        private async void btnConnectSerial_Click(object sender, EventArgs e)
        {
            var portName = txtConnectSerialPortName.Text ?? "COM4";
            txtConnectSerialPortName.Enabled = false;
            _ = byte.TryParse(txtConnectSerialStationNo.Text, out var slaveId);
            txtConnectSerialStationNo.Enabled = false;
            btnConnectSerial.Enabled = false;
            var mode = comboBoxConnectSerialMode.SelectedItem;
            comboBoxConnectSerialMode.Enabled = false;
            statusMasterSerial.Text = "状态：连接中...";
            WriteLog($"[statusMasterSerial]状态：连接中...");
            var device = new Device
            {
                DeviceType = DeviceType.ModbusSerialPort,
                PortName = portName,
                StationNo = slaveId,
                SerialMode = mode?.ToString() == SerialMode.ASCII.ToString() ? SerialMode.ASCII : SerialMode.RTU
            };
            _modbusSerialPortClient = CommunicationClientFactory.CreateClient(device);
            var ress = await _modbusSerialPortClient.ConnectAsync();
            if (!ress.Success)
            {
                btnConnectSerial.Enabled = true;
                txtConnectSerialStationNo.Enabled = true;
                txtConnectSerialPortName.Enabled = true;
                statusMasterSerial.Text = $"状态：[{ress.ErrorMessage}]";
                WriteLog($"[statusMasterSerial]状态：{ress.ErrorMessage}");
                return;
            }
            statusMasterSerial.Text = "状态：已连接";
            WriteLog($"[statusMasterSerial]状态：已连接");
            await Task.Delay(500);
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (btnConnectSerial.Enabled) break;
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusSerialPortClient.WriteAsync("HR4", DataPointType.Float, randData);
                    var readData = await _modbusSerialPortClient.ReadAsync("HR4", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterSerial.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                        WriteLog($"[statusMasterSerial]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                    });
                    Thread.Sleep(1000);
                }
            });
        }
        #endregion


        #region Modbus Slave Tcp
        private void InitControlsModbusSlaveTcp()
        {
            btnStartSlaveServerTcp.Click += btnStartSlaveServerTcp_Click;
            Controls.Add(btnStartSlaveServerTcp);
            btnStopSlaveServerTcp.Click += btnStopSlaveServerTcp_Click;
            Controls.Add(btnStopSlaveServerTcp);
        }

        private TcpListener? _tcpListener;
        private NModbus.IModbusSlaveNetwork? _slaveNetwork;
        private Task? _tcpListenTask;
        private CancellationTokenSource? _ctsTcpListenTask;
        private async void btnStopSlaveServerTcp_Click(object sender, EventArgs e)
        {
            try
            {
                _ctsTcpListenTask?.Cancel();
                if (_tcpListenTask != null)
                    await _tcpListenTask.WaitAsync(TimeSpan.FromSeconds(3));
            }
            catch { }
            finally
            {
                _slaveNetwork?.Dispose();
                _slaveNetwork = null;
                _tcpListener?.Stop();
                _tcpListener = null;
                _tcpListenTask = null;
                _ctsTcpListenTask?.Dispose();
                _ctsTcpListenTask = null;
                txtSlaveServerTcpPort.Enabled = true;
                btnStartSlaveServerTcp.Enabled = true;
                statusSlaveServerTcp.Text = "状态：已停止";
                WriteLog($"[statusSlaveServerTcp]状态：已停止");
                //OnLog?.Invoke("从站服务已停止");
            }
        }
        private void OutputSlaveServerTcpStatus(string message)
        {
            this.Invoke(() =>
            {
                statusSlaveServerTcp.Text = message;
            });
            WriteLog($"[statusSlaveServerTcp]{message}");
        }
        private async void btnStartSlaveServerTcp_Click(object sender, EventArgs e)
        {
            if (_tcpListenTask != null && !_ctsTcpListenTask!.IsCancellationRequested) return;//从站服务已在运行

            _ = byte.TryParse(txtSlaveServerTcpStationNo.Text, out var slaveId);
            txtSlaveServerTcpStationNo.Enabled = false;
            _ = int.TryParse(txtSlaveServerTcpPort.Text, out var port);
            txtSlaveServerTcpPort.Enabled = false;
            btnStartSlaveServerTcp.Enabled = false;
            statusSlaveServerTcp.Text = "状态：启动中...";
            WriteLog($"[statusSlaveServerTcp]状态：启动中...");
            _ctsTcpListenTask = new CancellationTokenSource();
            _tcpListener = new TcpListener(System.Net.IPAddress.Any, port);

            var factory = new NModbus.ModbusFactory();
            _slaveNetwork = factory.CreateSlaveNetwork(_tcpListener);//创建Modbus TCP Slave网络

            //var dataStore = new NModbus.Data.DefaultSlaveDataStore();//创建默认数据存储
            //dataStore.HoldingRegisters.WritePoints(0, new ushort[] { 123, 456 });//自定义数据存储-寄存器值
            //dataStore.CoilDiscretes.WritePoints(0, new[] { true, false, true });//自定义数据存储-预设线圈
            var dataStore = new EventDrivenDataStore();//数据存储
            dataStore.CoilDiscretes.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"[{args.Operation} 线圈]:[{DateTime.Now:HH:mm:ss}] 起始地址:{args.StartingAddress} 数量:{args.NumberOfPoints} 值:[{string.Join(", ", args.Points)}]");
            dataStore.CoilInputs.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"[{args.Operation} 离散输入]: [{DateTime.Now:HH:mm:ss}] 起始地址:{args.StartingAddress} 值:[{string.Join(", ", args.Points)}]");
            dataStore.InputRegisters.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"[{args.Operation} 输入寄存器]: [{DateTime.Now:HH:mm:ss}] 起始地址:{args.StartingAddress} 值:[{string.Join(", ", args.Points)}]");
            dataStore.HoldingRegisters.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"[{args.Operation} 保持寄存器]: [{DateTime.Now:HH:mm:ss}] 起始地址:{args.StartingAddress} 数量:{args.NumberOfPoints} 值:[{string.Join(", ", args.Points)}]");

            var slave = factory.CreateSlave(slaveId, dataStore);//创建Slave（指定站号+数据存储）
            _slaveNetwork.AddSlave(slave);//将Slave添加到网络中
            _tcpListener.Start();
            _ctsTcpListenTask.Token.Register(() => _tcpListener.Stop());
            statusSlaveServerTcp.Text = $"状态：Modbus TCP 从站监听端口 {port}";
            WriteLog($"[statusSlaveServerTcp]状态：Modbus TCP 从站监听端口 {port}");
            _tcpListenTask = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500);
                    await _slaveNetwork.ListenAsync();
                    OutputSlaveServerTcpStatus($"状态：Modbus TCP 从站监听端口 {port}，已启动");
                }
                catch (ObjectDisposedException)
                {
                    // 正常停止导致的对象释放，忽略
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.Interrupted)
                {
                    // 取消监听导致的中断，忽略
                }
                catch (Exception ex)
                {
                    //OnLog?.Invoke($"监听异常: {ex.Message}");
                }
            }, _ctsTcpListenTask.Token);
        }
        #endregion

        #region Modbus Slave SerialPort
        private void InitControlsModbusSlaveSerialPort()
        {
            btnStartSlaveServerSerial.Click += btnStartSlaveServerSerial_Click;
            Controls.Add(btnStartSlaveServerSerial);
            btnStopSlaveServerSerial.Click += btnStopSlaveServerSerial_Click;
            Controls.Add(btnStopSlaveServerSerial);
            comboBoxSlaveServerSerialMode.Items.Add(SerialMode.RTU.ToString());
            comboBoxSlaveServerSerialMode.Items.Add(SerialMode.ASCII);
            comboBoxSlaveServerSerialMode.SelectedIndex = 0;
            Controls.Add(comboBoxSlaveServerSerialMode);
        }

        private async void btnStopSlaveServerSerial_Click(object sender, EventArgs e)
        {
            await Task.Delay(1500);
            comboBoxSlaveServerSerialMode.Enabled = true;
            txtSlaveServerSerialStationNo.Enabled = true;
            txtSlaveServerSerialPortName.Enabled = true;
            btnStartSlaveServerSerial.Enabled = true;
            statusSlaveServerSerial.Text = "状态：已停止";
            WriteLog($"[statusSlaveServerSerial]状态：已停止");
        }
        private void OutputSlaveServerSerialStatus(string message)
        {
            this.Invoke(() =>
            {
                statusSlaveServerSerial.Text = message;
            });
            WriteLog($"[statusSlaveServerSerial]{message}");
        }
        private async void btnStartSlaveServerSerial_Click(object sender, EventArgs e)
        {
            btnStartSlaveServerSerial.Enabled = false;
            statusSlaveServerSerial.Text = "状态：启动中...";
            WriteLog($"[statusSlaveServerSerial]状态：启动中...");
            _ = byte.TryParse(txtSlaveServerSerialStationNo.Text, out var slaveId);
            txtSlaveServerSerialStationNo.Enabled = false;
            var mode = comboBoxSlaveServerSerialMode.SelectedItem;
            comboBoxSlaveServerSerialMode.Enabled = false;
            var portName = txtSlaveServerSerialPortName.Text;
            txtSlaveServerSerialPortName.Enabled = false;
            var device = new Device
            {
                DeviceType = DeviceType.ModbusSerialPort,
                PortName = portName,
                StationNo = slaveId,
                SerialMode = mode?.ToString() == SerialMode.ASCII.ToString() ? SerialMode.ASCII : SerialMode.RTU
            };
            var server = new ModbusSerialSlave(device);
            server.AddSlave(slaveId);//添加站号1的从站（与你上位机主站代码对应）
            server.OnLog += msg => OutputSlaveServerSerialStatus($"[系统] {DateTime.Now:HH:mm:ss} {msg}");//绑定日志事件

            //绑定读写事件，打印主站操作
            server.HoldingRegistersStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 保持寄存器] [{DateTime.Now:HH:mm:ss}]站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            server.InputRegistersStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 输入寄存器] [{DateTime.Now:HH:mm:ss}]站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };
            server.CoilDiscretesStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 线圈] [{DateTime.Now:HH:mm:ss}]站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            server.CoilInputsStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 离散输入] [{DateTime.Now:HH:mm:ss}]站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };

            try
            {
                
                await server.StartAsync();// 启动服务

                //// 6. 模拟硬件数据：每秒更新温度（HR0，float）和压力（HR2，float）
                //var random = new Random();
                //float temperature = 25.0f;
                //float pressure = 1.2f;

                //_ = Task.Run(async () =>
                //{
                //    while (!server.Disposed)
                //    {
                //        // 温度在20~30℃之间随机波动
                //        temperature += (float)(random.NextDouble() - 0.5) * 0.8;
                //        temperature = Math.Clamp(temperature, 20f, 30f);
                //        server.SetHoldingRegisterFloat(1, 0, temperature);

                //        // 压力在1.0~1.5MPa之间随机波动
                //        pressure += (float)(random.NextDouble() - 0.5) * 0.1;
                //        pressure = Math.Clamp(pressure, 1.0f, 1.5f);
                //        server.SetHoldingRegisterFloat(1, 2, pressure);

                //        await Task.Delay(1000);
                //    }
                //});
                statusSlaveServerSerial.Text = $"已启动，可使用主站连接，站号{slaveId}，串口{portName}";
                WriteLog($"[statusSlaveServerSerial]已启动，可使用主站连接，站号{slaveId}，串口{portName}");
            }
            catch (Exception ex)
            {
                comboBoxSlaveServerSerialMode.Enabled = true;
                txtSlaveServerSerialStationNo.Enabled = true;
                txtSlaveServerSerialPortName.Enabled = true;
                btnStartSlaveServerSerial.Enabled = true;
                statusSlaveServerSerial.Text = $"程序已退出，运行错误: {ex.Message}";
                WriteLog($"[statusSlaveServerSerial]程序已退出，运行错误: {ex.Message}");
                await server.StopAsync();
            }
        }
        #endregion
    }
}
