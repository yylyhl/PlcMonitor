using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace PlcMonitor.Wpf
{
    internal class MainViewModel : ObservableObject
    {
        private string _status = "状态：未连接";
        private string _temperature = "温度：-- °C";
        public string Status { get => _status; set => SetProperty(ref _status, value); }

        public string Temperature { get => _temperature; set => SetProperty(ref _temperature, value); }

        public ICommand ConnectCommand => new RelayCommand(Connect, () => ConnectButtonStatus);

        private bool _connectButton = true;
        public bool ConnectButtonStatus { get => _connectButton; set => SetProperty(ref _connectButton, value); }
        private async void Connect()
        {
            ConnectButtonStatus = false;
            Status = "状态：已连接";
            await Task.Delay(500);
            //Application.Current.Dispatcher.Invoke(() =>{});
            _ = Task.Run(async () =>
            {
                var random = new Random();
                while (true)
                {
                    Temperature = $"温度：{random.Next(20, 30)} °C";
                    await Task.Delay(1000);
                }
            });
        }
    }
}
