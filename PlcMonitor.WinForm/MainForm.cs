using PlcMonitor.Core;
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

            btnConnectTcp.Click += btnConnectTcp_Click;
            Controls.Add(btnConnectTcp);

            btnConnectSerialPort.Click += btnConnectSerialPort_Click;
            Controls.Add(btnConnectSerialPort);

            btnServerRunTcp.Click += btnServerRunTcp_Click;
            Controls.Add(btnServerRunTcp);

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

        private async void btnConnectTcp_Click(object sender, EventArgs e)
        {
            btnConnectTcp.Enabled = false;
            statusMasterTcp.Text = "状态：连接中...";
            var device = new Device { DeviceType = DeviceType.ModbusTcp, IpAddress = "127.0.0.1", StationNo = 2 };
            var modbusClient = CommunicationClientFactory.CreateClient(device);
            var ress = await modbusClient.ConnectAsync();
            if (!ress.Success)
            {
                statusMasterTcp.Text = $"状态：连接失败[{ress.ErrorMessage}]";
                btnConnectTcp.Enabled = true;
                return;
            }
            statusMasterTcp.Text = "状态：已连接";
            await Task.Delay(500);
            //_timer.Start();
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var randData = _random.Next(10, 100);
                    var writeData = await modbusClient.WriteAsync("HR0", DataPointType.Float, randData);
                    var modbusData = await modbusClient.ReadAsync("HR0", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterTcp.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(modbusData.Success ? modbusData.Data : modbusData.ErrorMessage)}]";
                        //Timer_Tick(default, default);
                    });
                    Thread.Sleep(1000);
                }
            });
        }
        private async void btnConnectSerialPort_Click(object sender, EventArgs e)
        {
            btnConnectSerialPort.Enabled = false;
            statusMasterSerialPort.Text = "状态：连接中...";
            var device = new Device { DeviceType = DeviceType.ModbusSerialPort, PortName = "COM4", StationNo = 1 };
            var modbusClient = CommunicationClientFactory.CreateClient(device);
            var ress = await modbusClient.ConnectAsync();
            if (!ress.Success)
            {
                statusMasterSerialPort.Text = $"状态：连接失败[{ress.ErrorMessage}]";
                btnConnectSerialPort.Enabled = true;
                return;
            }
            statusMasterSerialPort.Text = "状态：已连接";
            await Task.Delay(500);
            _ = Task.Run(async () =>
            {
                while (true)
                {
                    var randData = _random.Next(10, 100);
                    var writeData = await modbusClient.WriteAsync("HR4", DataPointType.Float, randData);
                    var modbusData = await modbusClient.ReadAsync("HR4", DataPointType.Float);
                    this.Invoke(() =>
                    {
                        statusMasterSerialPort.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(modbusData.Success ? modbusData.Data : modbusData.ErrorMessage)}]";
                    });
                    Thread.Sleep(1000);
                }
            });
        }
        private async void btnServerRunTcp_Click(object sender, EventArgs e)
        {
            btnServerRunTcp.Enabled = false;
            statusSlaveTcp.Text = "状态：启动中...";
            using (var listener = new TcpListener(System.Net.IPAddress.Any, 502))
            {
                var factory = new NModbus.ModbusFactory();
                var slaveNetwork = factory.CreateSlaveNetwork(listener);//创建Modbus TCP Slave网络
                var dataStore = new NModbus.Data.DefaultSlaveDataStore();//创建默认数据存储
                //dataStore.CoilDiscretes.WritePoints(0, new[] { true, false, true });//自定义数据存储-预设线圈
                //dataStore.HoldingRegisters.WritePoints(0, new ushort[] { 123, 456 });//自定义数据存储-寄存器值
                var slave = factory.CreateSlave(1, dataStore);//创建Slave（指定站号+数据存储）
                slaveNetwork.AddSlave(slave);//将Slave添加到网络中

                listener.Start();
                statusSlaveTcp.Text = "状态：Modbus TCP 从站监听端口 502";
                await Task.Delay(500);
                await slaveNetwork.ListenAsync();
                statusSlaveTcp.Text = "状态：Modbus TCP 从站已启动，监听端口 502";
                _ = Task.Run(async () =>
                {
                    while (true)
                    {
                        //var randData = _random.Next(10, 100);
                        //var writeData = await slave.DataStore.HoldingRegisters.ReadPoints("HR0", 2);
                        //var modbusData = await modbusClient.ReadAsync("HR40001", DataPointType.Float);
                        //this.Invoke(() =>
                        //{
                        //    statusSlaveTcp.Text = $"data：[write={(writeData.Success ? randData : writeData.ErrorMessage)}] [read={(modbusData.Success ? modbusData.Data : modbusData.ErrorMessage)}]";
                        //    //Timer_Tick(default, default);
                        //});
                        Thread.Sleep(1000);
                    }
                });
            }
        }

    }
}
