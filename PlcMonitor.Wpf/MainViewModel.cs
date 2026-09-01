using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PlcMonitor.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PlcMonitor.Wpf
{
    internal class MainViewModel : ObservableObject
    {
        private readonly ILogger<MainViewModel> _logger;
        public MainViewModel(ILogger<MainViewModel> logger)
        {
            _logger = logger;
        }
        private static readonly Random _random = new();
        private string _commonLog = "inti-TextBox";
        public string CommonLog { get => _commonLog; set => SetProperty(ref _commonLog, value); }
        private ObservableCollection<string> _commonLogArray = ["init-ListView"];
        public ObservableCollection<string> CommonLogArray { get => _commonLogArray; set => SetProperty(ref _commonLogArray, value); }
        private void WriteLog(string message)
        {
            _logger.LogInformation(message);
            //Application.Current.Dispatcher.Invoke(() => WriteTxtComLog(message));
            CommonLog += Environment.NewLine + message;
            Application.Current.Dispatcher.Invoke(() => CommonLogArray.Add(message));
        }

        #region Modbus Master
        private string _connectionType = "0";//0=TCP,1=SerialPort
        public string ConnectionType
        {
            get => _connectionType; set
            {
                if (_connectionType == value) return;
                _connectionType = value;
                OnPropertyChanged();
                if (value == "0")
                {
                    ConnectionSlaveTcpArgs = Visibility.Visible.ToString();
                    ConnectionSlaveSerialArgs = Visibility.Collapsed.ToString();
                }
                else
                {
                    ConnectionSlaveTcpArgs = Visibility.Collapsed.ToString();
                    ConnectionSlaveSerialArgs = Visibility.Visible.ToString();
                }
            }
        }

        private string _slaveId = "1";
        public string SlaveId { get => _slaveId; set => SetProperty(ref _slaveId, value); }

        private string _connectionSerialArgs = Visibility.Collapsed.ToString();//Hidden/Collapsed/Visible
        public string ConnectionSlaveSerialArgs { get => _connectionSerialArgs; set => SetProperty(ref _connectionSerialArgs, value); }
        private string _serialName = "COM4";
        public string SerialName { get => _serialName; set => SetProperty(ref _serialName, value); }
        private string _serialProtocol = "0";//0=RTU,1=ASCII
        public string SerialProtocol { get => _serialProtocol; set => SetProperty(ref _serialProtocol, value); }

        private string _connectionTcpArgs = Visibility.Visible.ToString();//Hidden/Collapsed/Visible
        public string ConnectionSlaveTcpArgs { get => _connectionTcpArgs; set => SetProperty(ref _connectionTcpArgs, value); }
        private string _slaveAddress = "127.0.0.1";
        public string SlaveAddress { get => _slaveAddress; set => SetProperty(ref _slaveAddress, value); }
        private string _slavePort = "502";
        public string SlavePort { get => _slavePort; set => SetProperty(ref _slavePort, value); }

        ICommunicationClient _modbusClient;
        private string _statusMasterModbus = "状态：未连接";
        public string StatusMasterModbus { get => _statusMasterModbus; set => SetProperty(ref _statusMasterModbus, value); }
        private bool _connectTcpBtnStatus = true;
        public bool ConnectSlaveBtnStatus { get => _connectTcpBtnStatus; set => SetProperty(ref _connectTcpBtnStatus, value); }
        public ICommand ConnectSlaveCommand => new RelayCommand(ConnectTcp);
        private bool _disconnectTcpBtnStatus = false;
        public bool DisconnectSlaveBtnStatus { get => _disconnectTcpBtnStatus; set => SetProperty(ref _disconnectTcpBtnStatus, value); }
        public ICommand DisconnectSlaveCommand => new RelayCommand(DisconnectTcp);
        private async void DisconnectTcp()
        {
            if (_modbusClient.IsConnected) await _modbusClient.DisconnectAsync();
            await Task.Delay(100);
            ConnectSlaveBtnStatus = true;
            DisconnectSlaveBtnStatus = false;
            StatusMasterModbus = $"状态：已断开连接";
            WriteLog($"[{nameof(StatusMasterModbus)}]状态：已断开连接");
        }
        private async void ConnectTcp()
        {
            if (_modbusClient != null && _modbusClient.IsConnected)
            {
                MessageBox.Show("已连接", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(SlaveId))
            {
                MessageBox.Show("Modbus站号呢？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }
            if (ConnectionType == "0")
            {
                if (string.IsNullOrWhiteSpace(SlaveAddress)||string.IsNullOrWhiteSpace(SlavePort))
                {
                    MessageBox.Show("缺少地址/端口", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return;
                }
            }
            else if (string.IsNullOrWhiteSpace(SerialName))
            {
                MessageBox.Show("缺少串口号", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }
            var slaveHost = SlaveAddress ?? "127.0.0.1";
            _ = int.TryParse(SlavePort, out var port);
            _ = byte.TryParse(SlaveId, out var slaveId);
            ConnectSlaveBtnStatus = false;
            StatusMasterModbus = "状态：连接中...";
            WriteLog($"[{nameof(StatusMasterModbus)}]状态：连接中...");

            var device = new Device
            {
                DeviceType = ConnectionType == "0" ? DeviceType.ModbusTcp : DeviceType.ModbusSerialPort,
                IpAddress = slaveHost,
                Port = port,
                //SerialMode = (SerialMode)Enum.Parse(typeof(SerialMode), SerialProtocol, true),
                //SerialMode = (SerialMode)int.Parse(SerialProtocol),
                SerialMode = SerialProtocol == "0" ? SerialMode.RTU : SerialMode.ASCII,
                PortName = SerialName,
                SlaveId = slaveId
            };
            _modbusClient = CommunicationClientFactory.CreateClient(device);
            if (ConnectionType != "0")
            {
                _modbusClient.OnLog += (message) =>
                {
                    StatusMasterModbus = $"OnLog：[{message}]";
                    WriteLog($"[{nameof(StatusMasterModbus)}]OnLog：{message}");
                };
            }
            var ress = await _modbusClient.ConnectAsync();
            if (!ress.Success)
            {
                ConnectSlaveBtnStatus = true;
                DisconnectSlaveBtnStatus = false;
                StatusMasterModbus = $"状态：[{ress.ErrorMessage}]";
                WriteLog($"[{nameof(StatusMasterModbus)}]状态：[{ress.ErrorMessage}]");
                return;
            }

            DisconnectSlaveBtnStatus = true;
            StatusMasterModbus = "状态：已连接";
            WriteLog($"[{nameof(StatusMasterModbus)}]状态：已连接");
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (!_modbusClient.IsConnected)
                    {
                        await Task.Delay(100);
                        if (!ConnectSlaveBtnStatus) DisconnectTcp();
                        break;
                    }
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusClient.WriteAsync(ModbusFunction.HR + "1", DataPointType.Float, randData);
                    var readData = await _modbusClient.ReadAsync(ModbusFunction.HR + "1", DataPointType.Float);
                    StatusMasterModbus = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                    WriteLog($"[{nameof(StatusMasterModbus)}]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                    Thread.Sleep(1000);
                }
            });
        }
        #endregion

        #region Modbus Slave Server
        private string _slaveServerType = "0";//0=TCP,1=SerialPort
        public string SlaveServerType
        {
            get => _slaveServerType; set
            {
                if (_slaveServerType == value) return;
                _slaveServerType = value;
                OnPropertyChanged();
                if (value == "0")
                {
                    SlaveServerTcpArgs = Visibility.Visible.ToString();
                    SlaveServerSerialArgs = Visibility.Collapsed.ToString();
                }
                else
                {
                    SlaveServerTcpArgs = Visibility.Collapsed.ToString();
                    SlaveServerSerialArgs = Visibility.Visible.ToString();
                }
            }
        }

        private string _slaveServerId = "1";
        public string SlaveServerId { get => _slaveServerId; set => SetProperty(ref _slaveServerId, value); }

        private string _slaveServerSerialArgs = Visibility.Collapsed.ToString();//Hidden/Collapsed/Visible
        public string SlaveServerSerialArgs { get => _slaveServerSerialArgs; set => SetProperty(ref _slaveServerSerialArgs, value); }
        private string _slaveServerSerialName = "COM3";
        public string SlaveServerSerialName { get => _slaveServerSerialName; set => SetProperty(ref _slaveServerSerialName, value); }
        private string _slaveServerSerialProtocol = "0";//0=RTU,1=ASCII
        public string SlaveServerSerialProtocol { get => _slaveServerSerialProtocol; set => SetProperty(ref _slaveServerSerialProtocol, value); }

        private string _slaveServerTcpArgs = Visibility.Visible.ToString();//Hidden/Collapsed/Visible
        public string SlaveServerTcpArgs { get => _slaveServerTcpArgs; set => SetProperty(ref _slaveServerTcpArgs, value); }
        private string _slaveServerPort = "502";
        public string SlaveServerPort { get => _slaveServerPort; set => SetProperty(ref _slaveServerPort, value); }

        ICommunicationServer _modbusServer;
        private string _statusSlaveModbus = "状态：未启动";
        public string StatusSlaveModbus { get => _statusSlaveModbus; set => SetProperty(ref _statusSlaveModbus, value); }
        private bool _startSlaveServerBtnStatus = true;
        public bool StartSlaveServerBtnStatus { get => _startSlaveServerBtnStatus; set => SetProperty(ref _startSlaveServerBtnStatus, value); }
        public ICommand StartSlaveServerCommand => new RelayCommand(StartSlaveServer);
        private bool _stopSlaveServerBtnStatus = false;
        public bool StopSlaveServerBtnStatus { get => _stopSlaveServerBtnStatus; set => SetProperty(ref _stopSlaveServerBtnStatus, value); }
        public ICommand StopSlaveServerCommand => new RelayCommand(StopSlaveServer);
        private async void StopSlaveServer()
        {
            if (_modbusServer != null && _modbusServer.IsStarted) await _modbusServer.StopAsync();
            StartSlaveServerBtnStatus = true;
            StopSlaveServerBtnStatus = false;
            StatusSlaveModbus = "状态：已停止";
            WriteLog($"[{nameof(StatusSlaveModbus)}]状态：已停止");
        }
        private async void StartSlaveServer()
        {
            if (_modbusServer != null && _modbusServer.IsStarted)
            {
                MessageBox.Show("已启动", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(SlaveServerId))
            {
                MessageBox.Show("Modbus站号呢？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }
            if (SlaveServerType == "0")
            {
                if (string.IsNullOrWhiteSpace(SlaveServerPort))
                {
                    MessageBox.Show("缺少端口", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return;
                }
            }
            else if (string.IsNullOrWhiteSpace(SlaveServerSerialName))
            {
                MessageBox.Show("缺少串口号", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }
            _ = int.TryParse(SlaveServerPort, out var port);
            _ = byte.TryParse(SlaveServerId, out var slaveId);
            StartSlaveServerBtnStatus = false;
            StatusSlaveModbus = "状态：启动中...";
            WriteLog($"[{nameof(StatusSlaveModbus)}]状态：启动中...");

            var device = new Device
            {
                DeviceType = SlaveServerType == "0" ? DeviceType.ModbusTcp : DeviceType.ModbusSerialPort,
                Port = port,
                //SerialMode = (SerialMode)Enum.Parse(typeof(SerialMode), SlaveServerSerialProtocol, true),
                //SerialMode = (SerialMode)int.Parse(SlaveServerSerialProtocol),
                SerialMode = SlaveServerSerialProtocol == "0" ? SerialMode.RTU : SerialMode.ASCII,
                PortName = SlaveServerSerialName,
                SlaveId = slaveId
            };
            _modbusServer = CommunicationServerFactory.CreateServer(device);
            if (!_modbusServer.AddSlave(slaveId, out var msg))
            {
                StartSlaveServerBtnStatus = true;
                MessageBox.Show(msg, "提示", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                return;
            }//添加从站（与上位机主站代码对应）
            _modbusServer.OnLog += msg =>
            {
                StatusSlaveModbus = $"OnLog：[{msg}]";
                WriteLog(msg);
            };//绑定日志事件
            //绑定读写事件，打印主站操作
            _modbusServer.HoldingRegistersStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                StatusSlaveModbus = $"[{opera} 保持寄存器] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]";
                WriteLog($"[{opera} 保持寄存器] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            _modbusServer.InputRegistersStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                StatusSlaveModbus = $"[{opera} 输入寄存器] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]";
                WriteLog($"[{opera} 输入寄存器] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };
            _modbusServer.CoilDiscretesStorageOperationOccurred += (slaveId, opera, addr, data, count) =>
            {
                StatusSlaveModbus = $"[{opera} 线圈] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]";
                WriteLog($"[{opera} 线圈] 站号:{slaveId} 起始地址:{addr} 数量:{count} 值:[{string.Join(", ", data)}]");
            };
            _modbusServer.CoilInputsStorageOperationOccurred += (slaveId, opera, addr, data) =>
            {
                StatusSlaveModbus = $"[{opera} 离散输入] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]";
                WriteLog($"[{opera} 离散输入] 站号:{slaveId} 起始地址:{addr} 值:[{string.Join(", ", data)}]");
            };
            try
            {
                await _modbusServer.StartAsync();
                StopSlaveServerBtnStatus = true;
                if (SlaveServerType == "0")
                {
                    StatusSlaveModbus = $"状态：已启动，站号{SlaveServerId}，端口{SlaveServerPort}";
                    WriteLog($"[{nameof(StatusSlaveModbus)}]状态：已启动，站号{SlaveServerId}，端口{SlaveServerPort}");
                }
                else
                {
                    StatusSlaveModbus = $"状态：已启动，站号{SlaveServerId}，串口{SlaveServerSerialName}";
                    WriteLog($"[{nameof(StatusSlaveModbus)}]状态：已启动，站号{SlaveServerId}，串口{SlaveServerSerialName}");
                }
            }
            catch (Exception ex)
            {
                StartSlaveServerBtnStatus = true;
                StopSlaveServerBtnStatus = false;
                StatusSlaveModbus = $"状态：启动失败，运行错误: {ex.Message}，站号{SlaveServerId}，串口{SlaveServerSerialName}";
                WriteLog($"[{nameof(StatusSlaveModbus)}]状态：启动失败，运行错误: {ex.Message}，站号{SlaveServerId}，串口{SlaveServerSerialName}");
            }
        }
        #endregion
    }
}
