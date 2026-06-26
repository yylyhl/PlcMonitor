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
            SuspendLayout();
            // 
            // btnConnectTcp
            // 
            btnConnectTcp.Location = new Point(12, 12);
            btnConnectTcp.Name = "btnConnectTcp";
            btnConnectTcp.Size = new Size(161, 35);
            btnConnectTcp.TabIndex = 0;
            btnConnectTcp.Text = "连接Slave-Tcp";
            btnConnectTcp.UseVisualStyleBackColor = true;
            // 
            // btnStartSlaveServerTcp
            // 
            btnStartSlaveServerTcp.Location = new Point(12, 264);
            btnStartSlaveServerTcp.Name = "btnStartSlaveServerTcp";
            btnStartSlaveServerTcp.Size = new Size(161, 35);
            btnStartSlaveServerTcp.TabIndex = 1;
            btnStartSlaveServerTcp.Text = "启动Slave-Tcp";
            btnStartSlaveServerTcp.UseVisualStyleBackColor = true;
            // 
            // btnConnectSerial
            // 
            btnConnectSerial.Location = new Point(12, 111);
            btnConnectSerial.Name = "btnConnectSerial";
            btnConnectSerial.Size = new Size(161, 37);
            btnConnectSerial.TabIndex = 0;
            btnConnectSerial.Text = "连接Slave-SerialPort";
            btnConnectSerial.UseVisualStyleBackColor = true;
            // 
            // btnStartSlaveServerSerial
            // 
            btnStartSlaveServerSerial.Location = new Point(12, 365);
            btnStartSlaveServerSerial.Name = "btnStartSlaveServerSerial";
            btnStartSlaveServerSerial.Size = new Size(161, 35);
            btnStartSlaveServerSerial.TabIndex = 1;
            btnStartSlaveServerSerial.Text = "启动Slave-SerialPort";
            btnStartSlaveServerSerial.UseVisualStyleBackColor = true;
            // 
            // statusSlaveServerTcp
            // 
            statusSlaveServerTcp.AutoSize = true;
            statusSlaveServerTcp.Location = new Point(186, 314);
            statusSlaveServerTcp.Name = "statusSlaveServerTcp";
            statusSlaveServerTcp.Size = new Size(130, 17);
            statusSlaveServerTcp.TabIndex = 2;
            statusSlaveServerTcp.Text = "statusSlaveServerTcp";
            // 
            // statusMasterTcp
            // 
            statusMasterTcp.AutoSize = true;
            statusMasterTcp.Location = new Point(186, 62);
            statusMasterTcp.Name = "statusMasterTcp";
            statusMasterTcp.Size = new Size(104, 17);
            statusMasterTcp.TabIndex = 2;
            statusMasterTcp.Text = "statusMasterTcp";
            // 
            // statusSlaveServerSerial
            // 
            statusSlaveServerSerial.AutoSize = true;
            statusSlaveServerSerial.Location = new Point(186, 415);
            statusSlaveServerSerial.Name = "statusSlaveServerSerial";
            statusSlaveServerSerial.Size = new Size(141, 17);
            statusSlaveServerSerial.TabIndex = 2;
            statusSlaveServerSerial.Text = "statusSlaveServerSerial";
            // 
            // statusMasterSerial
            // 
            statusMasterSerial.AutoSize = true;
            statusMasterSerial.Location = new Point(186, 164);
            statusMasterSerial.Name = "statusMasterSerial";
            statusMasterSerial.Size = new Size(115, 17);
            statusMasterSerial.TabIndex = 2;
            statusMasterSerial.Text = "statusMasterSerial";
            // 
            // txtConnectTcpStationNo
            // 
            txtConnectTcpStationNo.Location = new Point(186, 18);
            txtConnectTcpStationNo.Name = "txtConnectTcpStationNo";
            txtConnectTcpStationNo.PlaceholderText = "station id";
            txtConnectTcpStationNo.Size = new Size(29, 23);
            txtConnectTcpStationNo.TabIndex = 3;
            txtConnectTcpStationNo.Text = "1";
            // 
            // txtConnectSerialStationNo
            // 
            txtConnectSerialStationNo.Location = new Point(186, 118);
            txtConnectSerialStationNo.Name = "txtConnectSerialStationNo";
            txtConnectSerialStationNo.PlaceholderText = "station id";
            txtConnectSerialStationNo.Size = new Size(29, 23);
            txtConnectSerialStationNo.TabIndex = 3;
            txtConnectSerialStationNo.Text = "2";
            // 
            // txtSlaveServerTcpPort
            // 
            txtSlaveServerTcpPort.Location = new Point(234, 270);
            txtSlaveServerTcpPort.Name = "txtSlaveServerTcpPort";
            txtSlaveServerTcpPort.PlaceholderText = "port";
            txtSlaveServerTcpPort.Size = new Size(57, 23);
            txtSlaveServerTcpPort.TabIndex = 3;
            txtSlaveServerTcpPort.Text = "502";
            // 
            // btnStopSlaveServerTcp
            // 
            btnStopSlaveServerTcp.Location = new Point(12, 305);
            btnStopSlaveServerTcp.Name = "btnStopSlaveServerTcp";
            btnStopSlaveServerTcp.Size = new Size(161, 35);
            btnStopSlaveServerTcp.TabIndex = 1;
            btnStopSlaveServerTcp.Text = "停止Slave-Tcp";
            btnStopSlaveServerTcp.UseVisualStyleBackColor = true;
            // 
            // btnDisconnectTcp
            // 
            btnDisconnectTcp.Location = new Point(12, 53);
            btnDisconnectTcp.Name = "btnDisconnectTcp";
            btnDisconnectTcp.Size = new Size(161, 35);
            btnDisconnectTcp.TabIndex = 0;
            btnDisconnectTcp.Text = "断开Slave-Tcp";
            btnDisconnectTcp.UseVisualStyleBackColor = true;
            // 
            // btnStopSlaveServerSerial
            // 
            btnStopSlaveServerSerial.Location = new Point(12, 406);
            btnStopSlaveServerSerial.Name = "btnStopSlaveServerSerial";
            btnStopSlaveServerSerial.Size = new Size(161, 35);
            btnStopSlaveServerSerial.TabIndex = 1;
            btnStopSlaveServerSerial.Text = "停止Slave-SerialPort";
            btnStopSlaveServerSerial.UseVisualStyleBackColor = true;
            // 
            // btnDisconnectSerial
            // 
            btnDisconnectSerial.Location = new Point(12, 154);
            btnDisconnectSerial.Name = "btnDisconnectSerial";
            btnDisconnectSerial.Size = new Size(161, 37);
            btnDisconnectSerial.TabIndex = 0;
            btnDisconnectSerial.Text = "断开Slave-SerialPort";
            btnDisconnectSerial.UseVisualStyleBackColor = true;
            // 
            // txtSlaveServerSerialPortName
            // 
            txtSlaveServerSerialPortName.Location = new Point(234, 371);
            txtSlaveServerSerialPortName.Name = "txtSlaveServerSerialPortName";
            txtSlaveServerSerialPortName.PlaceholderText = "portName";
            txtSlaveServerSerialPortName.Size = new Size(56, 23);
            txtSlaveServerSerialPortName.TabIndex = 3;
            txtSlaveServerSerialPortName.Text = "COM3";
            // 
            // txtSlaveServerTcpStationNo
            // 
            txtSlaveServerTcpStationNo.Location = new Point(186, 270);
            txtSlaveServerTcpStationNo.Name = "txtSlaveServerTcpStationNo";
            txtSlaveServerTcpStationNo.PlaceholderText = "station id";
            txtSlaveServerTcpStationNo.Size = new Size(29, 23);
            txtSlaveServerTcpStationNo.TabIndex = 3;
            txtSlaveServerTcpStationNo.Text = "1";
            // 
            // txtConnectTcpHost
            // 
            txtConnectTcpHost.Location = new Point(221, 18);
            txtConnectTcpHost.Name = "txtConnectTcpHost";
            txtConnectTcpHost.PlaceholderText = "host";
            txtConnectTcpHost.Size = new Size(80, 23);
            txtConnectTcpHost.TabIndex = 3;
            txtConnectTcpHost.Text = "127.0.0.1";
            // 
            // txtConnectSerialPortName
            // 
            txtConnectSerialPortName.Location = new Point(234, 118);
            txtConnectSerialPortName.Name = "txtConnectSerialPortName";
            txtConnectSerialPortName.PlaceholderText = "portName";
            txtConnectSerialPortName.Size = new Size(56, 23);
            txtConnectSerialPortName.TabIndex = 3;
            txtConnectSerialPortName.Text = "COM4";
            // 
            // txtConnectTcpPort
            // 
            txtConnectTcpPort.Location = new Point(307, 18);
            txtConnectTcpPort.Name = "txtConnectTcpPort";
            txtConnectTcpPort.PlaceholderText = "port";
            txtConnectTcpPort.Size = new Size(57, 23);
            txtConnectTcpPort.TabIndex = 3;
            txtConnectTcpPort.Text = "502";
            // 
            // txtSlaveServerSerialStationNo
            // 
            txtSlaveServerSerialStationNo.Location = new Point(186, 371);
            txtSlaveServerSerialStationNo.Name = "txtSlaveServerSerialStationNo";
            txtSlaveServerSerialStationNo.PlaceholderText = "station id";
            txtSlaveServerSerialStationNo.Size = new Size(29, 23);
            txtSlaveServerSerialStationNo.TabIndex = 3;
            txtSlaveServerSerialStationNo.Text = "1";
            // 
            // comboBoxSlaveServerSerialMode
            // 
            comboBoxSlaveServerSerialMode.FormattingEnabled = true;
            comboBoxSlaveServerSerialMode.Location = new Point(307, 371);
            comboBoxSlaveServerSerialMode.Name = "comboBoxSlaveServerSerialMode";
            comboBoxSlaveServerSerialMode.Size = new Size(121, 25);
            comboBoxSlaveServerSerialMode.TabIndex = 4;
            // 
            // comboBoxConnectSerialMode
            // 
            comboBoxConnectSerialMode.FormattingEnabled = true;
            comboBoxConnectSerialMode.Location = new Point(307, 118);
            comboBoxConnectSerialMode.Name = "comboBoxConnectSerialMode";
            comboBoxConnectSerialMode.Size = new Size(121, 25);
            comboBoxConnectSerialMode.TabIndex = 4;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(comboBoxConnectSerialMode);
            Controls.Add(comboBoxSlaveServerSerialMode);
            Controls.Add(txtConnectSerialPortName);
            Controls.Add(txtConnectSerialStationNo);
            Controls.Add(txtSlaveServerSerialPortName);
            Controls.Add(txtConnectTcpPort);
            Controls.Add(txtSlaveServerTcpPort);
            Controls.Add(txtSlaveServerSerialStationNo);
            Controls.Add(txtSlaveServerTcpStationNo);
            Controls.Add(txtConnectTcpHost);
            Controls.Add(txtConnectTcpStationNo);
            Controls.Add(statusMasterSerial);
            Controls.Add(statusSlaveServerSerial);
            Controls.Add(statusMasterTcp);
            Controls.Add(statusSlaveServerTcp);
            Controls.Add(btnStopSlaveServerSerial);
            Controls.Add(btnStartSlaveServerSerial);
            Controls.Add(btnStopSlaveServerTcp);
            Controls.Add(btnStartSlaveServerTcp);
            Controls.Add(btnDisconnectSerial);
            Controls.Add(btnConnectSerial);
            Controls.Add(btnDisconnectTcp);
            Controls.Add(btnConnectTcp);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            Text = "MainForm";
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
    }
}
