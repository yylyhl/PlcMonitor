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
        private string _commonLog = "init";
        public string CommonLog { get => _commonLog; set => SetProperty(ref _commonLog, value); }
        private ObservableCollection<string> _commonLogArray = ["init"];
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
                    ConnectionTcpArgs = Visibility.Visible.ToString();
                    ConnectionSerialArgs = Visibility.Collapsed.ToString();
                }
                else
                {
                    ConnectionTcpArgs = Visibility.Collapsed.ToString();
                    ConnectionSerialArgs = Visibility.Visible.ToString();
                }
            }
        }


        private string _slaveId = "1";
        public string SlaveId { get => _slaveId; set => SetProperty(ref _slaveId, value); }

        private string _connectionSerialArgs = Visibility.Collapsed.ToString();//Hidden/Collapsed/Visible
        public string ConnectionSerialArgs { get => _connectionSerialArgs; set => SetProperty(ref _connectionSerialArgs, value); }
        private string _serialName = "COM4";
        public string SerialName { get => _serialName; set => SetProperty(ref _serialName, value); }
        private string _serialProtocol = "0";//0=RTU,1=ASCII
        public string SerialProtocol { get => _serialProtocol; set => SetProperty(ref _serialProtocol, value); }

        private string _connectionTcpArgs = Visibility.Visible.ToString();//Hidden/Collapsed/Visible
        public string ConnectionTcpArgs { get => _connectionTcpArgs; set => SetProperty(ref _connectionTcpArgs, value); }
        private string _slaveAddress = "127.0.0.1";
        public string SlaveAddress { get => _slaveAddress; set => SetProperty(ref _slaveAddress, value); }
        private string _slavePort = "502";
        public string SlavePort { get => _slavePort; set => SetProperty(ref _slavePort, value); }

        ICommunicationClient _modbusClient;
        private string _statusMasterTcp = "状态：未连接";
        public string StatusMasterTcp { get => _statusMasterTcp; set => SetProperty(ref _statusMasterTcp, value); }
        private bool _connectTcpBtnStatus = true;
        public bool ConnectTcpBtnStatus { get => _connectTcpBtnStatus; set => SetProperty(ref _connectTcpBtnStatus, value); }
        public ICommand ConnectTcpCommand => new RelayCommand(ConnectTcp);
        private bool _disconnectTcpBtnStatus = false;
        public bool DisconnectTcpBtnStatus { get => _disconnectTcpBtnStatus; set => SetProperty(ref _disconnectTcpBtnStatus, value); }
        public ICommand DisconnectTcpCommand => new RelayCommand(DisconnectTcp);
        private async void DisconnectTcp()
        {
            if (!_modbusClient.IsConnected) return;
            await _modbusClient.DisconnectAsync();
            await Task.Delay(100);
            ConnectTcpBtnStatus = true;
            DisconnectTcpBtnStatus = false;
            StatusMasterTcp = "状态：已断开连接";
            WriteLog($"[statusMasterTcp]状态：已断开连接");
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
            ConnectTcpBtnStatus = false;
            StatusMasterTcp = "状态：连接中...";
            WriteLog($"[statusMasterTcp]状态：连接中...");

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
                    StatusMasterTcp = $"OnLog：[{message}]";
                    WriteLog($"[statusMasterSerial]OnLog：{message}");
                };
            }
            var ress = await _modbusClient.ConnectAsync();
            if (!ress.Success)
            {
                ConnectTcpBtnStatus = true;
                DisconnectTcpBtnStatus = false;
                StatusMasterTcp = $"状态：[{ress.ErrorMessage}]";
                WriteLog($"[statusMasterTcp]状态：[{ress.ErrorMessage}]");
                return;
            }

            DisconnectTcpBtnStatus = true;
            StatusMasterTcp = "状态：已连接";
            WriteLog($"[statusMasterTcp]状态：已连接");
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    if (!_modbusClient.IsConnected)
                    {
                        await Task.Delay(100);
                        if (!ConnectTcpBtnStatus) DisconnectTcp();
                        break;
                    }
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusClient.WriteAsync(ModbusFunction.HR + "1", DataPointType.Float, randData);
                    var readData = await _modbusClient.ReadAsync(ModbusFunction.HR + "1", DataPointType.Float);
                    StatusMasterTcp = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                    WriteLog($"[statusMasterTcp]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                    Thread.Sleep(1000);
                }
            });
        } 
        #endregion
    }
}
