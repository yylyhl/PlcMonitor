using PlcMonitor.Core;
using PlcMonitor.Core.Slave;
using System.Net.Sockets;

namespace PlcMonitor.WinForm
{
    public partial class MainForm : Form
    {

        private readonly System.Windows.Forms.Timer _timer;
        public MainForm()
        {
            InitializeComponent();
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
            Controls.Add(btnConnectSerialPort);
            btnDisconnectTcp.Click += btnDisconnectTcp_Click;
            Controls.Add(btnDisconnectTcp);
        }
        ICommunicationClient _modbusTcpClient;
        private async void btnDisconnectTcp_Click(object sender, EventArgs e)
        {
            if (!_modbusTcpClient.IsConnected) return;
            await _modbusTcpClient.DisconnectAsync();
            await Task.Delay(1500);
            statusMasterTcp.Text = $"状态：已断开连接";
            btnConnectTcp.Enabled = true;
            txtSlaveStationNoTcp.Enabled = true;
            txtSlaveHostTcp.Enabled = true;
            txtSlavePortTcp.Enabled = true;
        }
        private async void btnConnectTcp_Click(object sender, EventArgs e)
        {
            var slaveHost = txtSlaveHostTcp.Text ?? "127.0.0.1";
            txtSlaveHostTcp.Enabled = false;
            _ = int.TryParse(txtSlavePortTcp.Text, out var port);
            txtSlavePortTcp.Enabled = false;
            _ = byte.TryParse(txtSlaveStationNoTcp.Text, out var slaveId);
            txtSlaveStationNoTcp.Enabled = false;
            btnConnectTcp.Enabled = false;
            statusMasterTcp.Text = "状态：连接中...";
            var device = new Device { DeviceType = DeviceType.ModbusTcp, IpAddress = slaveHost, StationNo = slaveId, Port = port };
            _modbusTcpClient = CommunicationClientFactory.CreateClient(device);
            var ress = await _modbusTcpClient.ConnectAsync();
            if (!ress.Success)
            {
                statusMasterTcp.Text = $"状态：连接失败[{ress.ErrorMessage}]";
                btnConnectTcp.Enabled = true;
                txtSlaveStationNoTcp.Enabled = true;
                txtSlaveHostTcp.Enabled = true;
                txtSlavePortTcp.Enabled = true;
                return;
            }
            statusMasterTcp.Text = "状态：已连接";
            await Task.Delay(500);
            //_timer.Start();
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (btnConnectTcp.Enabled) break;
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusTcpClient.WriteAsync("HR0", DataPointType.Float, randData);
                    var modbusData = await _modbusTcpClient.ReadAsync("HR0", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterTcp.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(modbusData.Success ? modbusData.Data : modbusData.ErrorMessage)}]";
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
            btnConnectSerialPort.Click += btnConnectSerialPort_Click;
            Controls.Add(btnConnectSerialPort);
            btnDisconnectSerialPort.Click += btnDisconnectSerialPort_Click;
            Controls.Add(btnDisconnectSerialPort);
        }
        ICommunicationClient _modbusSerialPortClient;
        private async void btnDisconnectSerialPort_Click(object sender, EventArgs e)
        {
            if (!_modbusSerialPortClient.IsConnected) return;
            await _modbusSerialPortClient.DisconnectAsync();
            await Task.Delay(1500);
            statusMasterSerialPort.Text = $"状态：已断开连接";
            btnConnectSerialPort.Enabled = true;
            txtSlaveStationNoSerialPort.Enabled = true;
            txtSlavePortNameSerialPort.Enabled = true;
        }
        private async void btnConnectSerialPort_Click(object sender, EventArgs e)
        {
            var portName = txtSlavePortNameSerialPort.Text ?? "COM4";
            txtSlavePortNameSerialPort.Enabled = false;
            _ = byte.TryParse(txtSlaveStationNoSerialPort.Text, out var slaveId);
            txtSlaveStationNoSerialPort.Enabled = false;
            btnConnectSerialPort.Enabled = false;
            statusMasterSerialPort.Text = "状态：连接中...";
            var device = new Device { DeviceType = DeviceType.ModbusSerialPort, PortName = portName, StationNo = slaveId };
            _modbusSerialPortClient = CommunicationClientFactory.CreateClient(device);
            var ress = await _modbusSerialPortClient.ConnectAsync();
            if (!ress.Success)
            {
                statusMasterSerialPort.Text = $"状态：连接失败[{ress.ErrorMessage}]";
                btnConnectSerialPort.Enabled = true;
                txtSlaveStationNoSerialPort.Enabled = true;
                txtSlavePortNameSerialPort.Enabled = true;
                return;
            }
            statusMasterSerialPort.Text = "状态：已连接";
            await Task.Delay(500);
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (btnConnectSerialPort.Enabled) break;
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusSerialPortClient.WriteAsync("HR4", DataPointType.Float, randData);
                    var modbusData = await _modbusSerialPortClient.ReadAsync("HR4", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterSerialPort.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(modbusData.Success ? modbusData.Data : modbusData.ErrorMessage)}]";
                    });
                    Thread.Sleep(1000);
                }
            });
        }
        #endregion


        #region Modbus Slave Tcp
        private void InitControlsModbusSlaveTcp()
        {
            btnServerRunTcp.Click += btnServerRunTcp_Click;
            Controls.Add(btnServerRunTcp);
            btnServerStopTcp.Click += btnServerStopTcp_Click;
            Controls.Add(btnServerStopTcp);
        }

        private TcpListener? _tcpListener;
        private NModbus.IModbusSlaveNetwork? _slaveNetwork;
        private Task? _tcpListenTask;
        private CancellationTokenSource? _ctsTcpListenTask;
        private async void btnServerStopTcp_Click(object sender, EventArgs e)
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
                txtServerSlavePortTcp.Enabled = true;
                btnServerRunTcp.Enabled = true;
                statusSlaveTcp.Text = "状态：已停止";
                //OnLog?.Invoke("从站服务已停止");
            }
        }
        private void OutputSlaveServerTcpStatus(string message)
        {
            this.Invoke(() =>
            {
                statusSlaveTcp.Text = message;
            });
        }
        private async void btnServerRunTcp_Click(object sender, EventArgs e)
        {
            if (_tcpListenTask != null && !_ctsTcpListenTask!.IsCancellationRequested) return;//从站服务已在运行

            _ = byte.TryParse(txtServerSlaveStationNoTcp.Text, out var slaveId);
            txtServerSlaveStationNoTcp.Enabled = false;
            _ = int.TryParse(txtServerSlavePortTcp.Text, out var port);
            txtServerSlavePortTcp.Enabled = false;
            btnServerRunTcp.Enabled = false;
            statusSlaveTcp.Text = "状态：启动中...";
            _ctsTcpListenTask = new CancellationTokenSource();
            _tcpListener = new TcpListener(System.Net.IPAddress.Any, port);

            var factory = new NModbus.ModbusFactory();
            _slaveNetwork = factory.CreateSlaveNetwork(_tcpListener);//创建Modbus TCP Slave网络

            //var dataStore = new NModbus.Data.DefaultSlaveDataStore();//创建默认数据存储
            //dataStore.HoldingRegisters.WritePoints(0, new ushort[] { 123, 456 });//自定义数据存储-寄存器值
            //dataStore.CoilDiscretes.WritePoints(0, new[] { true, false, true });//自定义数据存储-预设线圈
            var dataStore = new EventDrivenDataStore();//数据存储
            dataStore.CoilDiscretes.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"Coil discretes: {args.Operation} starting at {args.StartingAddress}");
            dataStore.CoilInputs.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"Coil inputs: {args.Operation} starting at {args.StartingAddress}");
            dataStore.InputRegisters.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"Input registers: {args.Operation} starting at {args.StartingAddress}");
            dataStore.HoldingRegisters.StorageOperationOccurred += (sender, args) => OutputSlaveServerTcpStatus($"Holding registers: {args.Operation} starting at {args.StartingAddress}");

            var slave = factory.CreateSlave(slaveId, dataStore);//创建Slave（指定站号+数据存储）
            _slaveNetwork.AddSlave(slave);//将Slave添加到网络中
            _tcpListener.Start();
            _ctsTcpListenTask.Token.Register(() => _tcpListener.Stop());
            statusSlaveTcp.Text = $"状态：Modbus TCP 从站监听端口 {port}";
            _tcpListenTask = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500);
                    await _slaveNetwork.ListenAsync();
                    this.Invoke(() =>
                    {
                        statusSlaveTcp.Text = $"状态：Modbus TCP 从站监听端口 {port}，已启动";
                    });
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
            btnServerRunSerialPort.Click += btnServerRunSerialPort_Click;
            Controls.Add(btnServerRunSerialPort);
            btnServerStopSerialPort.Click += btnServerStopSerialPort_Click;
            Controls.Add(btnServerStopSerialPort);
        }

        private async void btnServerStopSerialPort_Click(object sender, EventArgs e)
        {
            await Task.Delay(1500);
            txtServerSlavePortSerialPort.Enabled = true;
            btnServerRunSerialPort.Enabled = true;
            statusSlaveSerialPort.Text = "状态：已停止";
        }
        private async void btnServerRunSerialPort_Click(object sender, EventArgs e)
        {
            _ = int.TryParse(txtServerSlavePortSerialPort.Text, out var port);
            txtServerSlavePortSerialPort.Enabled = false;
            btnServerRunSerialPort.Enabled = false;
            statusSlaveSerialPort.Text = "状态：启动中...";


            Console.WriteLine("===== Modbus TCP 从站模拟器 =====");

            var server = new ModbusTcpSlaveServer("127.0.0.1", 502);//初始化从站服务，监听127.0.0.1:502
            server.AddSlave(1);//添加站号1的从站（与你上位机主站代码对应）
            server.OnLog += msg => Console.WriteLine($"[系统] {DateTime.Now:HH:mm:ss} {msg}");//绑定日志事件

            ////绑定读写事件，打印主站操作
            //server.HoldingRegistersStorageOperationOccurred += (slaveId, opera, addr, count, data, count) =>
            //{
            //    Console.WriteLine($"[读寄存器] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            //};
            //server.InputRegistersStorageOperationOccurred += (slaveId, opera, addr, count, data) =>
            //{
            //    Console.WriteLine($"[写寄存器] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            //};
            //server.CoilDiscretesStorageOperationOccurred += (slaveId, opera, addr, count, data, count) =>
            //{
            //    Console.WriteLine($"[读线圈] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            //};
            //server.CoilInputsStorageOperationOccurred += (slaveId, opera, addr, count, data) =>
            //{
            //    Console.WriteLine($"[写线圈] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            //};

            try
            {
                
                await server.StartAsync();// 启动服务

                // 6. 模拟硬件数据：每秒更新温度（HR0，float）和压力（HR2，float）
                var random = new Random();
                float temperature = 25.0f;
                float pressure = 1.2f;

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

                Console.WriteLine("模拟器运行中，按任意键退出...");
                Console.WriteLine("可使用你的上位机主站连接 127.0.0.1:502，站号1进行测试");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"运行错误: {ex.Message}");
            }
            finally
            {
                await server.StopAsync();
                Console.WriteLine("程序已退出");
            }
        }
        #endregion
    }
}
