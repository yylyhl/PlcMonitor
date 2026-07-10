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
            txtConnectTcpSlaveId = new TextBox();
            txtConnectSerialSlaveId = new TextBox();
            txtSlaveServerTcpPort = new TextBox();
            btnStopSlaveServerTcp = new Button();
            btnDisconnectTcp = new Button();
            btnStopSlaveServerSerial = new Button();
            btnDisconnectSerial = new Button();
            txtSlaveServerSerialPortName = new TextBox();
            txtSlaveServerTcpSlaveId = new TextBox();
            txtConnectTcpHost = new TextBox();
            txtConnectSerialPortName = new TextBox();
            txtConnectTcpPort = new TextBox();
            txtSlaveServerSerialSlaveId = new TextBox();
            comboBoxSlaveServerSerialMode = new ComboBox();
            comboBoxConnectSerialMode = new ComboBox();
            txtComLog = new TextBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            groupBox4 = new GroupBox();
            btnConnectS7 = new Button();
            btnDisconnectS7 = new Button();
            txtConnectS7Host = new TextBox();
            comboBoxConnectS7CpuType = new ComboBox();
            txtConnectS7Slot = new TextBox();
            txtConnectS7Rack = new TextBox();
            statusMasterS7 = new Label();
            txtConnectS7Port = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
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
            // txtConnectTcpSlaveId
            // 
            txtConnectTcpSlaveId.Location = new Point(189, 28);
            txtConnectTcpSlaveId.Name = "txtConnectTcpSlaveId";
            txtConnectTcpSlaveId.PlaceholderText = "station id";
            txtConnectTcpSlaveId.Size = new Size(29, 23);
            txtConnectTcpSlaveId.TabIndex = 3;
            txtConnectTcpSlaveId.Text = "1";
            // 
            // txtConnectSerialSlaveId
            // 
            txtConnectSerialSlaveId.Location = new Point(189, 128);
            txtConnectSerialSlaveId.Name = "txtConnectSerialSlaveId";
            txtConnectSerialSlaveId.PlaceholderText = "station id";
            txtConnectSerialSlaveId.Size = new Size(29, 23);
            txtConnectSerialSlaveId.TabIndex = 3;
            txtConnectSerialSlaveId.Text = "2";
            // 
            // txtSlaveServerTcpPort
            // 
            txtSlaveServerTcpPort.Location = new Point(233, 38);
            txtSlaveServerTcpPort.Name = "txtSlaveServerTcpPort";
            txtSlaveServerTcpPort.PlaceholderText = "port";
            txtSlaveServerTcpPort.Size = new Size(43, 23);
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
            txtSlaveServerSerialPortName.Size = new Size(60, 23);
            txtSlaveServerSerialPortName.TabIndex = 3;
            txtSlaveServerSerialPortName.Text = "COM3";
            // 
            // txtSlaveServerTcpSlaveId
            // 
            txtSlaveServerTcpSlaveId.Location = new Point(185, 38);
            txtSlaveServerTcpSlaveId.Name = "txtSlaveServerTcpSlaveId";
            txtSlaveServerTcpSlaveId.PlaceholderText = "station id";
            txtSlaveServerTcpSlaveId.Size = new Size(29, 23);
            txtSlaveServerTcpSlaveId.TabIndex = 3;
            txtSlaveServerTcpSlaveId.Text = "1";
            // 
            // txtConnectTcpHost
            // 
            txtConnectTcpHost.Location = new Point(224, 28);
            txtConnectTcpHost.Name = "txtConnectTcpHost";
            txtConnectTcpHost.PlaceholderText = "host";
            txtConnectTcpHost.Size = new Size(97, 23);
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
            txtConnectTcpPort.Location = new Point(325, 28);
            txtConnectTcpPort.Name = "txtConnectTcpPort";
            txtConnectTcpPort.PlaceholderText = "port";
            txtConnectTcpPort.Size = new Size(43, 23);
            txtConnectTcpPort.TabIndex = 3;
            txtConnectTcpPort.Text = "502";
            // 
            // txtSlaveServerSerialSlaveId
            // 
            txtSlaveServerSerialSlaveId.Location = new Point(185, 139);
            txtSlaveServerSerialSlaveId.Name = "txtSlaveServerSerialSlaveId";
            txtSlaveServerSerialSlaveId.PlaceholderText = "station id";
            txtSlaveServerSerialSlaveId.Size = new Size(29, 23);
            txtSlaveServerSerialSlaveId.TabIndex = 3;
            txtSlaveServerSerialSlaveId.Text = "1";
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
            groupBox1.Controls.Add(txtConnectSerialSlaveId);
            groupBox1.Controls.Add(statusMasterSerial);
            groupBox1.Controls.Add(txtConnectTcpSlaveId);
            groupBox1.Controls.Add(txtConnectTcpPort);
            groupBox1.Controls.Add(txtConnectTcpHost);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(445, 217);
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
            groupBox2.Controls.Add(txtSlaveServerSerialSlaveId);
            groupBox2.Controls.Add(statusSlaveServerSerial);
            groupBox2.Controls.Add(txtSlaveServerTcpSlaveId);
            groupBox2.Location = new Point(12, 249);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(445, 226);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "modbus slave server";
            // 
            // groupBox3
            // 
            groupBox3.Location = new Point(12, 685);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(445, 149);
            groupBox3.TabIndex = 8;
            groupBox3.TabStop = false;
            groupBox3.Text = "OPC UA";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(btnConnectS7);
            groupBox4.Controls.Add(btnDisconnectS7);
            groupBox4.Controls.Add(txtConnectS7Host);
            groupBox4.Controls.Add(comboBoxConnectS7CpuType);
            groupBox4.Controls.Add(txtConnectS7Slot);
            groupBox4.Controls.Add(txtConnectS7Rack);
            groupBox4.Controls.Add(statusMasterS7);
            groupBox4.Controls.Add(txtConnectS7Port);
            groupBox4.Location = new Point(12, 499);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(445, 149);
            groupBox4.TabIndex = 8;
            groupBox4.TabStop = false;
            groupBox4.Text = "Siemens S7";
            // 
            // btnConnectS7
            // 
            btnConnectS7.Location = new Point(15, 29);
            btnConnectS7.Name = "btnConnectS7";
            btnConnectS7.Size = new Size(161, 35);
            btnConnectS7.TabIndex = 0;
            btnConnectS7.Text = "连接S7";
            btnConnectS7.UseVisualStyleBackColor = true;
            // 
            // btnDisconnectS7
            // 
            btnDisconnectS7.Location = new Point(15, 95);
            btnDisconnectS7.Name = "btnDisconnectS7";
            btnDisconnectS7.Size = new Size(161, 35);
            btnDisconnectS7.TabIndex = 0;
            btnDisconnectS7.Text = "断开S7";
            btnDisconnectS7.UseVisualStyleBackColor = true;
            // 
            // txtConnectS7Host
            // 
            txtConnectS7Host.Location = new Point(182, 35);
            txtConnectS7Host.Name = "txtConnectS7Host";
            txtConnectS7Host.PlaceholderText = "host";
            txtConnectS7Host.Size = new Size(97, 23);
            txtConnectS7Host.TabIndex = 3;
            txtConnectS7Host.Text = "127.110.110.111";
            // 
            // comboBoxConnectS7CpuType
            // 
            comboBoxConnectS7CpuType.FormattingEnabled = true;
            comboBoxConnectS7CpuType.Location = new Point(182, 64);
            comboBoxConnectS7CpuType.Name = "comboBoxConnectS7CpuType";
            comboBoxConnectS7CpuType.Size = new Size(82, 25);
            comboBoxConnectS7CpuType.TabIndex = 4;
            comboBoxConnectS7CpuType.Text = "S71200";
            // 
            // txtConnectS7Slot
            // 
            txtConnectS7Slot.Location = new Point(374, 35);
            txtConnectS7Slot.Name = "txtConnectS7Slot";
            txtConnectS7Slot.PlaceholderText = "插槽：S7-300/400CPU=2；1200/1500=1；200SMART=1";
            txtConnectS7Slot.Size = new Size(29, 23);
            txtConnectS7Slot.TabIndex = 3;
            txtConnectS7Slot.Text = "1";
            // 
            // txtConnectS7Rack
            // 
            txtConnectS7Rack.Location = new Point(339, 35);
            txtConnectS7Rack.Name = "txtConnectS7Rack";
            txtConnectS7Rack.PlaceholderText = "机架：S7-300/400=0；S7-1200/1500/200SMART=0";
            txtConnectS7Rack.Size = new Size(29, 23);
            txtConnectS7Rack.TabIndex = 3;
            txtConnectS7Rack.Text = "1";
            // 
            // statusMasterS7
            // 
            statusMasterS7.AutoSize = true;
            statusMasterS7.Location = new Point(182, 104);
            statusMasterS7.Name = "statusMasterS7";
            statusMasterS7.Size = new Size(104, 17);
            statusMasterS7.TabIndex = 2;
            statusMasterS7.Text = "statusMasterTcp";
            // 
            // txtConnectS7Port
            // 
            txtConnectS7Port.Location = new Point(285, 35);
            txtConnectS7Port.Name = "txtConnectS7Port";
            txtConnectS7Port.PlaceholderText = "port";
            txtConnectS7Port.Size = new Size(43, 23);
            txtConnectS7Port.TabIndex = 3;
            txtConnectS7Port.Text = "102";
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
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
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
        private TextBox txtConnectTcpSlaveId;
        private TextBox txtConnectSerialSlaveId;
        private TextBox txtSlaveServerTcpPort;
        private Button btnStopSlaveServerTcp;
        private Button btnDisconnectTcp;
        private Button btnStopSlaveServerSerial;
        private Button btnDisconnectSerial;
        private TextBox txtSlaveServerSerialPortName;
        private TextBox txtSlaveServerTcpSlaveId;
        private TextBox txtConnectTcpHost;
        private TextBox txtConnectSerialPortName;
        private TextBox txtConnectTcpPort;
        private TextBox txtSlaveServerSerialSlaveId;
        private ComboBox comboBoxSlaveServerSerialMode;
        private ComboBox comboBoxConnectSerialMode;
        private TextBox txtComLog;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Button btnConnectS7;
        private Button btnDisconnectS7;
        private TextBox txtConnectS7Host;
        private TextBox txtConnectS7Slot;
        private TextBox txtConnectS7Rack;
        private Label statusMasterS7;
        private ComboBox comboBoxConnectS7CpuType;
        private TextBox txtConnectS7Port;
    }
}
