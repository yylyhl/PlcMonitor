namespace PlcMonitor.WinForm
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            btnConnectTcp = new Button();
            btnStartSlaveServerTcp = new Button();
            btnConnectSerial = new Button();
            btnStartSlaveServerSerial = new Button();
            statusSlaveServerTcp = new Label();
            statusMasterTcp = new Label();
            statusSlaveServerSerial = new Label();
            statusMasterSerial = new Label();
            txtConnectTcpStationNo = new TextBox();
            txtConnectSerialStationNo = new TextBox();
            txtSlaveServerTcpPort = new TextBox();
            btnStopSlaveServerTcp = new Button();
            btnDisconnectTcp = new Button();
            btnStopSlaveServerSerial = new Button();
            btnDisconnectSerial = new Button();
            txtSlaveServerSerialPortName = new TextBox();
            txtSlaveServerTcpStationNo = new TextBox();
            txtConnectTcpHost = new TextBox();
            txtConnectSerialPortName = new TextBox();
            txtConnectTcpPort = new TextBox();
            txtSlaveServerSerialStationNo = new TextBox();
            comboBoxSlaveServerSerialMode = new ComboBox();
            comboBoxConnectSerialMode = new ComboBox();
            txtComLog = new TextBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnConnectTcp
            // 
            btnConnectTcp.Location = new Point(15, 22);
            btnConnectTcp.Name = "btnConnectTcp";
            btnConnectTcp.Size = new Size(161, 35);
            btnConnectTcp.TabIndex = 0;
            btnConnectTcp.Text = "连接Slave-Tcp";
            btnConnectTcp.UseVisualStyleBackColor = true;
            // 
            // btnStartSlaveServerTcp
            // 
            btnStartSlaveServerTcp.Location = new Point(11, 32);
            btnStartSlaveServerTcp.Name = "btnStartSlaveServerTcp";
            btnStartSlaveServerTcp.Size = new Size(161, 35);
            btnStartSlaveServerTcp.TabIndex = 1;
            btnStartSlaveServerTcp.Text = "启动Slave-Tcp";
            btnStartSlaveServerTcp.UseVisualStyleBackColor = true;
            // 
            // btnConnectSerial
            // 
            btnConnectSerial.Location = new Point(15, 121);
            btnConnectSerial.Name = "btnConnectSerial";
            btnConnectSerial.Size = new Size(161, 37);
            btnConnectSerial.TabIndex = 0;
            btnConnectSerial.Text = "连接Slave-SerialPort";
            btnConnectSerial.UseVisualStyleBackColor = true;
            // 
            // btnStartSlaveServerSerial
            // 
            btnStartSlaveServerSerial.Location = new Point(11, 133);
            btnStartSlaveServerSerial.Name = "btnStartSlaveServerSerial";
            btnStartSlaveServerSerial.Size = new Size(161, 35);
            btnStartSlaveServerSerial.TabIndex = 1;
            btnStartSlaveServerSerial.Text = "启动Slave-Serial";
            btnStartSlaveServerSerial.UseVisualStyleBackColor = true;
            // 
            // statusSlaveServerTcp
            // 
            statusSlaveServerTcp.AutoSize = true;
            statusSlaveServerTcp.Location = new Point(185, 82);
            statusSlaveServerTcp.Name = "statusSlaveServerTcp";
            statusSlaveServerTcp.Size = new Size(130, 17);
            statusSlaveServerTcp.TabIndex = 2;
            statusSlaveServerTcp.Text = "statusSlaveServerTcp";
            // 
            // statusMasterTcp
            // 
            statusMasterTcp.AutoSize = true;
            statusMasterTcp.Location = new Point(189, 72);
            statusMasterTcp.Name = "statusMasterTcp";
            statusMasterTcp.Size = new Size(104, 17);
            statusMasterTcp.TabIndex = 2;
            statusMasterTcp.Text = "statusMasterTcp";
            // 
            // statusSlaveServerSerial
            // 
            statusSlaveServerSerial.AutoSize = true;
            statusSlaveServerSerial.Location = new Point(185, 183);
            statusSlaveServerSerial.Name = "statusSlaveServerSerial";
            statusSlaveServerSerial.Size = new Size(141, 17);
            statusSlaveServerSerial.TabIndex = 2;
            statusSlaveServerSerial.Text = "statusSlaveServerSerial";
            // 
            // statusMasterSerial
            // 
            statusMasterSerial.AutoSize = true;
            statusMasterSerial.Location = new Point(189, 174);
            statusMasterSerial.Name = "statusMasterSerial";
            statusMasterSerial.Size = new Size(115, 17);
            statusMasterSerial.TabIndex = 2;
            statusMasterSerial.Text = "statusMasterSerial";
            // 
            // txtConnectTcpStationNo
            // 
            txtConnectTcpStationNo.Location = new Point(189, 28);
            txtConnectTcpStationNo.Name = "txtConnectTcpStationNo";
            txtConnectTcpStationNo.PlaceholderText = "station id";
            txtConnectTcpStationNo.Size = new Size(29, 23);
            txtConnectTcpStationNo.TabIndex = 3;
            txtConnectTcpStationNo.Text = "1";
            // 
            // txtConnectSerialStationNo
            // 
            txtConnectSerialStationNo.Location = new Point(189, 128);
            txtConnectSerialStationNo.Name = "txtConnectSerialStationNo";
            txtConnectSerialStationNo.PlaceholderText = "station id";
            txtConnectSerialStationNo.Size = new Size(29, 23);
            txtConnectSerialStationNo.TabIndex = 3;
            txtConnectSerialStationNo.Text = "2";
            // 
            // txtSlaveServerTcpPort
            // 
            txtSlaveServerTcpPort.Location = new Point(233, 38);
            txtSlaveServerTcpPort.Name = "txtSlaveServerTcpPort";
            txtSlaveServerTcpPort.PlaceholderText = "port";
            txtSlaveServerTcpPort.Size = new Size(57, 23);
            txtSlaveServerTcpPort.TabIndex = 3;
            txtSlaveServerTcpPort.Text = "502";
            // 
            // btnStopSlaveServerTcp
            // 
            btnStopSlaveServerTcp.Location = new Point(11, 73);
            btnStopSlaveServerTcp.Name = "btnStopSlaveServerTcp";
            btnStopSlaveServerTcp.Size = new Size(161, 35);
            btnStopSlaveServerTcp.TabIndex = 1;
            btnStopSlaveServerTcp.Text = "停止Slave-Tcp";
            btnStopSlaveServerTcp.UseVisualStyleBackColor = true;
            // 
            // btnDisconnectTcp
            // 
            btnDisconnectTcp.Location = new Point(15, 63);
            btnDisconnectTcp.Name = "btnDisconnectTcp";
            btnDisconnectTcp.Size = new Size(161, 35);
            btnDisconnectTcp.TabIndex = 0;
            btnDisconnectTcp.Text = "断开Slave-Tcp";
            btnDisconnectTcp.UseVisualStyleBackColor = true;
            // 
            // btnStopSlaveServerSerial
            // 
            btnStopSlaveServerSerial.Location = new Point(11, 174);
            btnStopSlaveServerSerial.Name = "btnStopSlaveServerSerial";
            btnStopSlaveServerSerial.Size = new Size(161, 35);
            btnStopSlaveServerSerial.TabIndex = 1;
            btnStopSlaveServerSerial.Text = "停止Slave-Serial";
            btnStopSlaveServerSerial.UseVisualStyleBackColor = true;
            // 
            // btnDisconnectSerial
            // 
            btnDisconnectSerial.Location = new Point(15, 164);
            btnDisconnectSerial.Name = "btnDisconnectSerial";
            btnDisconnectSerial.Size = new Size(161, 37);
            btnDisconnectSerial.TabIndex = 0;
            btnDisconnectSerial.Text = "断开Slave-SerialPort";
            btnDisconnectSerial.UseVisualStyleBackColor = true;
            // 
            // txtSlaveServerSerialPortName
            // 
            txtSlaveServerSerialPortName.Location = new Point(233, 139);
            txtSlaveServerSerialPortName.Name = "txtSlaveServerSerialPortName";
            txtSlaveServerSerialPortName.PlaceholderText = "portName";
            txtSlaveServerSerialPortName.Size = new Size(56, 23);
            txtSlaveServerSerialPortName.TabIndex = 3;
            txtSlaveServerSerialPortName.Text = "COM3";
            // 
            // txtSlaveServerTcpStationNo
            // 
            txtSlaveServerTcpStationNo.Location = new Point(185, 38);
            txtSlaveServerTcpStationNo.Name = "txtSlaveServerTcpStationNo";
            txtSlaveServerTcpStationNo.PlaceholderText = "station id";
            txtSlaveServerTcpStationNo.Size = new Size(29, 23);
            txtSlaveServerTcpStationNo.TabIndex = 3;
            txtSlaveServerTcpStationNo.Text = "1";
            // 
            // txtConnectTcpHost
            // 
            txtConnectTcpHost.Location = new Point(224, 28);
            txtConnectTcpHost.Name = "txtConnectTcpHost";
            txtConnectTcpHost.PlaceholderText = "host";
            txtConnectTcpHost.Size = new Size(80, 23);
            txtConnectTcpHost.TabIndex = 3;
            txtConnectTcpHost.Text = "127.0.0.1";
            // 
            // txtConnectSerialPortName
            // 
            txtConnectSerialPortName.Location = new Point(237, 128);
            txtConnectSerialPortName.Name = "txtConnectSerialPortName";
            txtConnectSerialPortName.PlaceholderText = "portName";
            txtConnectSerialPortName.Size = new Size(56, 23);
            txtConnectSerialPortName.TabIndex = 3;
            txtConnectSerialPortName.Text = "COM4";
            // 
            // txtConnectTcpPort
            // 
            txtConnectTcpPort.Location = new Point(310, 28);
            txtConnectTcpPort.Name = "txtConnectTcpPort";
            txtConnectTcpPort.PlaceholderText = "port";
            txtConnectTcpPort.Size = new Size(57, 23);
            txtConnectTcpPort.TabIndex = 3;
            txtConnectTcpPort.Text = "502";
            // 
            // txtSlaveServerSerialStationNo
            // 
            txtSlaveServerSerialStationNo.Location = new Point(185, 139);
            txtSlaveServerSerialStationNo.Name = "txtSlaveServerSerialStationNo";
            txtSlaveServerSerialStationNo.PlaceholderText = "station id";
            txtSlaveServerSerialStationNo.Size = new Size(29, 23);
            txtSlaveServerSerialStationNo.TabIndex = 3;
            txtSlaveServerSerialStationNo.Text = "1";
            // 
            // comboBoxSlaveServerSerialMode
            // 
            comboBoxSlaveServerSerialMode.FormattingEnabled = true;
            comboBoxSlaveServerSerialMode.Location = new Point(306, 139);
            comboBoxSlaveServerSerialMode.Name = "comboBoxSlaveServerSerialMode";
            comboBoxSlaveServerSerialMode.Size = new Size(121, 25);
            comboBoxSlaveServerSerialMode.TabIndex = 4;
            comboBoxSlaveServerSerialMode.Text = "RTU";
            // 
            // comboBoxConnectSerialMode
            // 
            comboBoxConnectSerialMode.FormattingEnabled = true;
            comboBoxConnectSerialMode.Location = new Point(310, 128);
            comboBoxConnectSerialMode.Name = "comboBoxConnectSerialMode";
            comboBoxConnectSerialMode.Size = new Size(121, 25);
            comboBoxConnectSerialMode.TabIndex = 4;
            comboBoxConnectSerialMode.Text = "RTU";
            // 
            // txtComLog
            // 
            txtComLog.Location = new Point(463, 12);
            txtComLog.Multiline = true;
            txtComLog.Name = "txtComLog";
            txtComLog.ScrollBars = ScrollBars.Both;
            txtComLog.Size = new Size(325, 808);
            txtComLog.TabIndex = 5;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnConnectTcp);
            groupBox1.Controls.Add(btnDisconnectTcp);
            groupBox1.Controls.Add(comboBoxConnectSerialMode);
            groupBox1.Controls.Add(btnConnectSerial);
            groupBox1.Controls.Add(btnDisconnectSerial);
            groupBox1.Controls.Add(txtConnectSerialPortName);
            groupBox1.Controls.Add(statusMasterTcp);
            groupBox1.Controls.Add(txtConnectSerialStationNo);
            groupBox1.Controls.Add(statusMasterSerial);
            groupBox1.Controls.Add(txtConnectTcpStationNo);
            groupBox1.Controls.Add(txtConnectTcpPort);
            groupBox1.Controls.Add(txtConnectTcpHost);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(445, 221);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "modbus master client";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtSlaveServerSerialPortName);
            groupBox2.Controls.Add(btnStartSlaveServerTcp);
            groupBox2.Controls.Add(btnStopSlaveServerTcp);
            groupBox2.Controls.Add(comboBoxSlaveServerSerialMode);
            groupBox2.Controls.Add(btnStartSlaveServerSerial);
            groupBox2.Controls.Add(btnStopSlaveServerSerial);
            groupBox2.Controls.Add(txtSlaveServerTcpPort);
            groupBox2.Controls.Add(statusSlaveServerTcp);
            groupBox2.Controls.Add(txtSlaveServerSerialStationNo);
            groupBox2.Controls.Add(statusSlaveServerSerial);
            groupBox2.Controls.Add(txtSlaveServerTcpStationNo);
            groupBox2.Location = new Point(12, 249);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(445, 226);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "modbus slave server";
            // 
            // groupBox3
            // 
            groupBox3.Location = new Point(12, 497);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(445, 149);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "OPC UA";
            // 
            // groupBox4
            // 
            groupBox4.Location = new Point(12, 671);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(445, 149);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Text = "Siemens S7";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 846);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(txtComLog);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "MainForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnConnectTcp;
        private Button btnStartSlaveServerTcp;
        private Button btnConnectSerial;
        private Button btnStartSlaveServerSerial;
        private Label statusSlaveServerTcp;
        private Label statusMasterTcp;
        private Label statusSlaveServerSerial;
        private Label statusMasterSerial;
        private TextBox txtConnectTcpStationNo;
        private TextBox txtConnectSerialStationNo;
        private TextBox txtSlaveServerTcpPort;
        private Button btnStopSlaveServerTcp;
        private Button btnDisconnectTcp;
        private Button btnStopSlaveServerSerial;
        private Button btnDisconnectSerial;
        private TextBox txtSlaveServerSerialPortName;
        private TextBox txtSlaveServerTcpStationNo;
        private TextBox txtConnectTcpHost;
        private TextBox txtConnectSerialPortName;
        private TextBox txtConnectTcpPort;
        private TextBox txtSlaveServerSerialStationNo;
        private ComboBox comboBoxSlaveServerSerialMode;
        private ComboBox comboBoxConnectSerialMode;
        private TextBox txtComLog;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
    }
}
