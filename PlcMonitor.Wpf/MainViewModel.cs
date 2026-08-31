using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PlcMonitor.Core;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PlcMonitor.Wpf
{
    internal class MainViewModel : ObservableObject
    {
        private static readonly Random _random = new();
        private string _commonLog = "init";
        public string CommonLog { get => _commonLog; set => SetProperty(ref _commonLog, value); }
        private ObservableCollection<string> _commonLogArray = ["init"];
        public ObservableCollection<string> CommonLogArray { get => _commonLogArray; set => SetProperty(ref _commonLogArray, value); }
        private void WriteTxtComLog(string message)
        {
            //TxtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            #region 倒序，新的在上面
            //txtComLog.SuspendLayout();//暂停绘制防闪烁
            //txtComLog.SelectionStart = 0;//光标移到最开头，插入文本
            //txtComLog.SelectedText = $"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}";
            //txtComLog.ResumeLayout(); 
            #endregion
        }
        private void WriteLog(string message)
        {
            //_logger.LogInformation(message);
            //Application.Current.Dispatcher.Invoke(() => WriteTxtComLog(message));
            CommonLog += Environment.NewLine + message;
            CommonLogArray.Add(message);
        }

        private string _slaveId = "1";
        public string SlaveId { get => _slaveId; set => SetProperty(ref _slaveId, value); }
        private string _slaveAddress = "127.0.0.1";
        public string SlaveAddress { get => _slaveAddress; set => SetProperty(ref _slaveAddress, value); }
        private string _slavePort = "502";
        public string SlavePort { get => _slavePort; set => SetProperty(ref _slavePort, value); }
        ICommunicationClient _modbusTcpClient;
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
            if (!_modbusTcpClient.IsConnected) return;
            await _modbusTcpClient.DisconnectAsync();
            await Task.Delay(100);
            ConnectTcpBtnStatus = true;
            DisconnectTcpBtnStatus = false;
            StatusMasterTcp = "状态：已断开连接";
            WriteLog($"[statusMasterTcp]状态：已断开连接");
        }
        private async void ConnectTcp()
        {
            if (_modbusTcpClient != null && _modbusTcpClient.IsConnected)
            {
                MessageBox.Show("已连接", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var slaveHost = SlaveAddress ?? "127.0.0.1";
            _ = int.TryParse(SlavePort, out var port);
            _ = byte.TryParse(SlaveId, out var slaveId);
            ConnectTcpBtnStatus = false;
            StatusMasterTcp = "状态：连接中...";
            WriteLog($"[statusMasterTcp]状态：连接中...");

            var device = new Device { DeviceType = DeviceType.ModbusTcp, IpAddress = slaveHost, SlaveId = slaveId, Port = port };
            _modbusTcpClient = CommunicationClientFactory.CreateClient(device);
            var ress = await _modbusTcpClient.ConnectAsync();
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
                    if (!_modbusTcpClient.IsConnected)
                    {
                        await Task.Delay(100);
                        if (!ConnectTcpBtnStatus) Application.Current.Dispatcher.Invoke(() => DisconnectTcp());
                        break;
                    }
                    var randData = _random.Next(10, 100);
                    var writeData = await _modbusTcpClient.WriteAsync(ModbusFunction.HR + "1", DataPointType.Float, randData);
                    var readData = await _modbusTcpClient.ReadAsync(ModbusFunction.HR + "1", DataPointType.Float);
                    Application.Current.Dispatcher.Invoke(() => 
                    {
                        StatusMasterTcp = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]";
                        WriteLog($"[statusMasterTcp]data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(readData.Success ? readData.Data : readData.ErrorMessage)}]");
                    });
                    Thread.Sleep(1000);
                }
            });
        }
    }
}
