using Microsoft.Extensions.Logging;
using PlcMonitor.Core;
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
            InitControlsS7Master();
            InitControlsModbusSlaveTcp();
            InitControlsModbusSlaveSerialPort();

            _timer = new System.Windows.Forms.Timer();
            _timer.Tick += Timer_Tick;
            //ftimer.Start();
            _logger.LogWarning("MainForm init done");
            WriteTxtComLog("MainForm init done");
        }
        private void WriteTxtComLog(string message)
        {
            txtComLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            #region 倒序，新的在上面
            //txtComLog.SuspendLayout();//暂停绘制防闪烁
            //txtComLog.SelectionStart = 0;//光标移到最开头，插入文本
            //txtComLog.SelectedText = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            //txtComLog.ResumeLayout(); 
            #endregion
        }
        private void WriteLog(string message)
        {
            _logger.LogInformation(message);
            this.Invoke(() =>
            {
                WriteTxtComLog(message);
            });
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
            btnDisconnectTcp.Click += btnDisconnectTcp_Click;
        }
        ICommunicationClient _modbusTcpClient;
        private async void btnDisconnectTcp_Click(object sender, EventArgs e)
        {
            if (!_modbusTcpClient.IsConnected) return;
            await _modbusTcpClient.DisconnectAsync();
            await Task.Delay(1500);
            DisconnectTcp();
        }
        private async void DisconnectTcp()
        {
            btnConnectTcp.Enabled = true;
            txtConnectTcpSlaveId.Enabled = true;
            txtConnectTcpHost.Enabled = true;
            txtConnectTcpPort.Enabled = true;
            statusMasterTcp.Text = $"状态：已断开连接";
            WriteLog($"[statusMasterTcp]状态：已断开连接");
        }
        private async void btnConnectTcp_Click(object sender, EventArgs e)
        {
            if (_modbusTcpClient != null && _modbusTcpClient.IsConnected)
            {
                MessageBox.Show("已连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var slaveHost = txtConnectTcpHost.Text ?? "127.0.0.1";
            txtConnectTcpHost.Enabled = false;
            _ = int.TryParse(txtConnectTcpPort.Text, out var port);
            txtConnectTcpPort.Enabled = false;
            _ = byte.TryParse(txtConnectTcpSlaveId.Text, out var slaveId);
            txtConnectTcpSlaveId.Enabled = false;
            btnConnectTcp.Enabled = false;
            statusMasterTcp.Text = "状态：连接中...";
            WriteLog($"[statusMasterTcp]状态：连接中...");
            var device = new Device { DeviceType = DeviceType.ModbusTcp, IpAddress = slaveHost, SlaveId = slaveId, Port = port };
            _modbusTcpClient = CommunicationClientFactory.CreateClient(device);
            var ress = await _modbusTcpClient.ConnectAsync();
            if (!ress.Success)
            {
                btnConnectTcp.Enabled = true;
                txtConnectTcpSlaveId.Enabled = true;
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
                    if (!_modbusTcpClient.IsConnected)
                    {
                        await Task.Delay(500);
                        if (!btnConnectTcp.Enabled) this.Invoke(() => DisconnectTcp());
                        break;
                    }
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusTcpClient.WriteAsync(ModbusFunction.HR + "1", DataPointType.Float, randData);
                    var readData = await _modbusTcpClient.ReadAsync(ModbusFunction.HR + "1", DataPointType.Float);
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
            btnDisconnectSerial.Click += btnDisconnectSerial_Click;
            comboBoxConnectSerialMode.Items.Add(SerialMode.RTU);
            comboBoxConnectSerialMode.Items.Add(SerialMode.ASCII);
            comboBoxConnectSerialMode.SelectedIndex = 0;
        }
        ICommunicationClient _modbusSerialClient;
        private async void btnDisconnectSerial_Click(object sender, EventArgs e)
        {
            if (!_modbusSerialClient.IsConnected) return;
            await _modbusSerialClient.DisconnectAsync();
            await Task.Delay(1500);
            DisconnectSerial();
        }
        private async void DisconnectSerial()
        {
            btnConnectSerial.Enabled = true;
            txtConnectSerialSlaveId.Enabled = true;
            txtConnectSerialPortName.Enabled = true;
            comboBoxConnectSerialMode.Enabled = true;
            statusMasterSerial.Text = $"状态：已断开连接";
            WriteLog($"[statusMasterSerial]状态：已断开连接");
        }
        private async void btnConnectSerial_Click(object sender, EventArgs e)
        {
            if (_modbusSerialClient != null && _modbusSerialClient.IsConnected)
            {
                MessageBox.Show("已连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var portName = txtConnectSerialPortName.Text ?? "COM4";
            txtConnectSerialPortName.Enabled = false;
            _ = byte.TryParse(txtConnectSerialSlaveId.Text, out var slaveId);
            txtConnectSerialSlaveId.Enabled = false;
            btnConnectSerial.Enabled = false;
            var mode = comboBoxConnectSerialMode.SelectedItem;
            comboBoxConnectSerialMode.Enabled = false;
            statusMasterSerial.Text = "状态：连接中...";
            WriteLog($"[statusMasterSerial]状态：连接中...");
            var device = new Device
            {
                DeviceType = DeviceType.ModbusSerialPort,
                PortName = portName,
                SlaveId = slaveId,
                SerialMode = mode?.ToString() == SerialMode.ASCII.ToString() ? SerialMode.ASCII : SerialMode.RTU
            };
            _modbusSerialClient = CommunicationClientFactory.CreateClient(device);
            _modbusSerialClient.OnLog += (message) =>
            {
                this.Invoke(() =>
                {
                    statusMasterSerial.Text = $"OnLog：[{message}]";
                });
                WriteLog($"[statusMasterSerial]OnLog：{message}");
            };
            var ress = await _modbusSerialClient.ConnectAsync();
            if (!ress.Success)
            {
                btnConnectSerial.Enabled = true;
                txtConnectSerialSlaveId.Enabled = true;
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
                    if (!_modbusSerialClient.IsConnected)
                    {
                        this.Invoke(() => DisconnectSerial());
                        break;
                    }
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusSerialClient.WriteAsync(ModbusFunction.HR + "1", DataPointType.Float, randData);
                    var readData = await _modbusSerialClient.ReadAsync(ModbusFunction.HR + "1", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterSerial.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                        WriteLog($"[statusMasterSerial]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                    });
                    Thread.Sleep(2000);
                }
            });
        }
        #endregion

        #region S7 Master
        private void InitControlsS7Master()
        {
            btnConnectS7.Click += btnConnectS7_Click;
            btnDisconnectS7.Click += btnDisconnectS7_Click;

            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.S7200);
            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.Logo0BA8);
            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.S7200Smart);
            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.S7300);
            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.S7400);
            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.S71200);
            comboBoxConnectS7CpuType.Items.Add(S7.Net.CpuType.S71500);
            comboBoxConnectS7CpuType.SelectedIndex = 6;
        }
        ICommunicationClient _s7Client;
        private async void btnDisconnectS7_Click(object sender, EventArgs e)
        {
            if (!_s7Client.IsConnected) return;
            await _s7Client.DisconnectAsync();
            await Task.Delay(1500);
            DisconnectS7();
        }
        private async void DisconnectS7()
        {
            btnConnectS7.Enabled = true;
            txtConnectS7Rack.Enabled = true;
            txtConnectS7Host.Enabled = true;
            txtConnectS7Slot.Enabled = true;
            txtConnectS7Port.Enabled = true;
            comboBoxConnectS7CpuType.Enabled = true;
            statusMasterS7.Text = $"状态：已断开连接";
            WriteLog($"[statusMasterS7]状态：已断开连接");
        }
        private async void btnConnectS7_Click(object sender, EventArgs e)
        {
            if (_s7Client != null && _s7Client.IsConnected)
            {
                MessageBox.Show("已连接", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var slaveHost = txtConnectS7Host.Text ?? "127.0.0.1";
            txtConnectS7Host.Enabled = false;
            _ = int.TryParse(txtConnectS7Slot.Text, out var slot);
            txtConnectS7Slot.Enabled = false;
            _ = byte.TryParse(txtConnectS7Rack.Text, out var rack);
            txtConnectS7Rack.Enabled = false;
            _ = int.TryParse(txtConnectS7Port.Text, out var port);
            txtConnectS7Port.Enabled = false;
            var cpuType = comboBoxConnectS7CpuType.SelectedItem;
            comboBoxConnectS7CpuType.Enabled = false;
            btnConnectS7.Enabled = false;
            statusMasterS7.Text = "状态：连接中...";
            WriteLog($"[statusMasterS7]状态：连接中...");
            var device = new Device
            {
                DeviceType = DeviceType.SiemensS7,
                IpAddress = slaveHost,
                Port = port,
                Rack = rack,
                Slot = slot,
                CpuType = cpuType?.ToString()
            };
            _s7Client = CommunicationClientFactory.CreateClient(device);
            var ress = await _s7Client.ConnectAsync();
            if (!ress.Success)
            {
                btnConnectS7.Enabled = true;
                txtConnectS7Rack.Enabled = true;
                txtConnectS7Host.Enabled = true;
                txtConnectS7Slot.Enabled = true;
                txtConnectS7Port.Enabled = true;
                comboBoxConnectS7CpuType.Enabled = true;
                statusMasterS7.Text = $"状态：[{ress.ErrorMessage}]";
                WriteLog($"[statusMasterS7]状态：[{ress.ErrorMessage}]");
                return;
            }
            statusMasterS7.Text = "状态：已连接";
            WriteLog($"[statusMasterS7]状态：已连接");
            await Task.Delay(500);
            //_timer.Start();
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (!_s7Client.IsConnected)
                    {
                        this.Invoke(() => DisconnectS7());
                        break;
                    }
                    var randData = _random.Next(10, 100);
                    var writeData = await _s7Client.WriteAsync("db2.dbd4", DataPointType.Float, randData);
                    var readData = await _s7Client.ReadAsync("db2.dbx0.1", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterS7.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                        WriteLog($"[statusMasterS7]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                        //Timer_Tick(default, default);
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
            btnStopSlaveServerTcp.Click += btnStopSlaveServerTcp_Click;
        }
        ICommunicationServer _modbusTcpSlave;
        //ModbusTcpSlave _modbusTcpSlave;
        private async void btnStopSlaveServerTcp_Click(object sender, EventArgs e)
        {
            if (_modbusTcpSlave != null && _modbusTcpSlave.IsStarted) await _modbusTcpSlave?.StopAsync();
            txtSlaveServerTcpPort.Enabled = true;
            btnStartSlaveServerTcp.Enabled = true;
            statusSlaveServerTcp.Text = "状态：已停止";
            WriteLog($"[statusSlaveServerTcp]状态：已停止");
        }
        private void OutputSlaveServerTcpStatus(string message)
        {
            this.Invoke(() =>
            {
                statusSlaveServerTcp.Text = $"[{DateTime.Now:HH:mm:ss}]{message}";
            });
            WriteLog($"[statusSlaveServerTcp]{message}");
        }
        private async void btnStartSlaveServerTcp_Click(object sender, EventArgs e)
        {
            if (_modbusTcpSlave != null && _modbusTcpSlave.IsStarted)
            {
                MessageBox.Show("已启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            _ = byte.TryParse(txtSlaveServerTcpSlaveId.Text, out var slaveId);
            txtSlaveServerTcpSlaveId.Enabled = false;
            _ = int.TryParse(txtSlaveServerTcpPort.Text, out var port);
            txtSlaveServerTcpPort.Enabled = false;
            btnStartSlaveServerTcp.Enabled = false;
            statusSlaveServerTcp.Text = "状态：启动中...";
            WriteLog($"[statusSlaveServerTcp]状态：启动中...");
            var device = new Device() { IpAddress = "127.0.0.1", Port = port, DeviceType = DeviceType.ModbusTcp };
            _modbusTcpSlave = CommunicationServerFactory.CreateServer(device);
            _modbusTcpSlave = CommunicationServerFactory.CreateServer(device);
            if (!_modbusTcpSlave.AddSlave(slaveId, out var msg))
            {
                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }//添加从站（与上位机主站代码对应）
            _modbusTcpSlave.OnLog += msg => OutputSlaveServerTcpStatus(msg);//绑定日志事件
            //绑定读写事件，打印主站操作
            _modbusTcpSlave.HoldingRegistersStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                OutputSlaveServerTcpStatus($"[{opera} 保持寄存器] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            _modbusTcpSlave.InputRegistersStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                OutputSlaveServerTcpStatus($"[{opera} 输入寄存器] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };
            _modbusTcpSlave.CoilDiscretesStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                OutputSlaveServerTcpStatus($"[{opera} 线圈] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            _modbusTcpSlave.CoilInputsStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                OutputSlaveServerTcpStatus($"[{opera} 离散输入] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };
            await _modbusTcpSlave.StartAsync();
        }
        #endregion

        #region Modbus Slave SerialPort
        private void InitControlsModbusSlaveSerialPort()
        {
            btnStartSlaveServerSerial.Click += btnStartSlaveServerSerial_Click;
            btnStopSlaveServerSerial.Click += btnStopSlaveServerSerial_Click;
            comboBoxSlaveServerSerialMode.Items.Add(SerialMode.RTU.ToString());
            comboBoxSlaveServerSerialMode.Items.Add(SerialMode.ASCII);
            comboBoxSlaveServerSerialMode.SelectedIndex = 0;
        }
        ICommunicationServer _modbusSerialSlave;
        //ModbusSerialSlave _modbusSerialSlave = null;
        private async void btnStopSlaveServerSerial_Click(object sender, EventArgs e)
        {
            await _modbusSerialSlave?.StopAsync();
            await Task.Delay(1500);
            comboBoxSlaveServerSerialMode.Enabled = true;
            txtSlaveServerSerialSlaveId.Enabled = true;
            txtSlaveServerSerialPortName.Enabled = true;
            btnStartSlaveServerSerial.Enabled = true;
            statusSlaveServerSerial.Text = "状态：已停止";
            WriteLog($"[statusSlaveServerSerial]状态：已停止");
        }
        private void OutputSlaveServerSerialStatus(string message)
        {
            this.Invoke(() =>
            {
                statusSlaveServerSerial.Text = $"[{DateTime.Now:HH:mm:ss}]{message}";
            });
            WriteLog($"[statusSlaveServerSerial]{message}");
        }
        private async void btnStartSlaveServerSerial_Click(object sender, EventArgs e)
        {
            if (_modbusSerialSlave != null && _modbusSerialSlave.IsStarted)
            {
                MessageBox.Show("已启动", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            btnStartSlaveServerSerial.Enabled = false;
            statusSlaveServerSerial.Text = "状态：启动中...";
            WriteLog($"[statusSlaveServerSerial]状态：启动中...");
            _ = byte.TryParse(txtSlaveServerSerialSlaveId.Text, out var slaveId);
            txtSlaveServerSerialSlaveId.Enabled = false;
            var mode = comboBoxSlaveServerSerialMode.SelectedItem;
            comboBoxSlaveServerSerialMode.Enabled = false;
            var portName = txtSlaveServerSerialPortName.Text;
            txtSlaveServerSerialPortName.Enabled = false;
            var device = new Device
            {
                DeviceType = DeviceType.ModbusSerialPort,
                PortName = portName,
                SlaveId = slaveId,
                SerialMode = mode?.ToString() == SerialMode.ASCII.ToString() ? SerialMode.ASCII : SerialMode.RTU
            };
            //_modbusSerialSlave = new ModbusSerialSlave(device);
            _modbusSerialSlave = CommunicationServerFactory.CreateServer(device);
            if (!_modbusSerialSlave.AddSlave(slaveId, out var msg))
            {
                MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }//添加从站（与上位机主站代码对应）
            _modbusSerialSlave.OnLog += msg => OutputSlaveServerSerialStatus(msg);//绑定日志事件

            //绑定读写事件，打印主站操作
            _modbusSerialSlave.HoldingRegistersStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 保持寄存器] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            _modbusSerialSlave.InputRegistersStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 输入寄存器] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };
            _modbusSerialSlave.CoilDiscretesStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 线圈] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            _modbusSerialSlave.CoilInputsStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                OutputSlaveServerSerialStatus($"[{opera} 离散输入] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };

            try
            {

                await _modbusSerialSlave.StartAsync();// 启动服务

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
                txtSlaveServerSerialSlaveId.Enabled = true;
                txtSlaveServerSerialPortName.Enabled = true;
                btnStartSlaveServerSerial.Enabled = true;
                statusSlaveServerSerial.Text = $"程序已退出，运行错误: {ex.Message}";
                WriteLog($"[statusSlaveServerSerial]程序已退出，运行错误: {ex.Message}");
                await _modbusSerialSlave.StopAsync();
            }
        }
        #endregion
    }
}
