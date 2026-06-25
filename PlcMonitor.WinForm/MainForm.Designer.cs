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
            btnServerRunTcp = new Button();
            btnConnectSerialPort = new Button();
            btnServerRunSerialPort = new Button();
            statusSlaveTcp = new Label();
            statusMasterTcp = new Label();
            statusSlaveSerialPort = new Label();
            statusMasterSerialPort = new Label();
            txtSlaveStationNoTcp = new TextBox();
            txtSlaveStationNoSerialPort = new TextBox();
            txtServerSlavePortTcp = new TextBox();
            btnServerStopTcp = new Button();
            btnDisconnectTcp = new Button();
            btnServerStopSerialPort = new Button();
            btnDisconnectSerialPort = new Button();
            txtServerSlavePortSerialPort = new TextBox();
            txtServerSlaveStationNoTcp = new TextBox();
            txtSlaveHostTcp = new TextBox();
            txtSlavePortNameSerialPort = new TextBox();
            txtSlavePortTcp = new TextBox();
            txtServerSlavePortNameStationNoSerialPort = new TextBox();
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
            // btnServerRunTcp
            // 
            btnServerRunTcp.Location = new Point(12, 264);
            btnServerRunTcp.Name = "btnServerRunTcp";
            btnServerRunTcp.Size = new Size(161, 35);
            btnServerRunTcp.TabIndex = 1;
            btnServerRunTcp.Text = "启动Slave-Tcp";
            btnServerRunTcp.UseVisualStyleBackColor = true;
            // 
            // btnConnectSerialPort
            // 
            btnConnectSerialPort.Location = new Point(12, 111);
            btnConnectSerialPort.Name = "btnConnectSerialPort";
            btnConnectSerialPort.Size = new Size(161, 37);
            btnConnectSerialPort.TabIndex = 0;
            btnConnectSerialPort.Text = "连接Slave-SerialPort";
            btnConnectSerialPort.UseVisualStyleBackColor = true;
            // 
            // btnServerRunSerialPort
            // 
            btnServerRunSerialPort.Location = new Point(12, 365);
            btnServerRunSerialPort.Name = "btnServerRunSerialPort";
            btnServerRunSerialPort.Size = new Size(161, 35);
            btnServerRunSerialPort.TabIndex = 1;
            btnServerRunSerialPort.Text = "启动Slave-SerialPort";
            btnServerRunSerialPort.UseVisualStyleBackColor = true;
            // 
            // statusSlaveTcp
            // 
            statusSlaveTcp.AutoSize = true;
            statusSlaveTcp.Location = new Point(186, 314);
            statusSlaveTcp.Name = "statusSlaveTcp";
            statusSlaveTcp.Size = new Size(139, 17);
            statusSlaveTcp.TabIndex = 2;
            statusSlaveTcp.Text = "status-server-SlaveTcp";
            // 
            // statusMasterTcp
            // 
            statusMasterTcp.AutoSize = true;
            statusMasterTcp.Location = new Point(186, 62);
            statusMasterTcp.Name = "statusMasterTcp";
            statusMasterTcp.Size = new Size(159, 17);
            statusMasterTcp.TabIndex = 2;
            statusMasterTcp.Text = "status-connect-MasterTcp";
            // 
            // statusSlaveSerialPort
            // 
            statusSlaveSerialPort.AutoSize = true;
            statusSlaveSerialPort.Location = new Point(186, 415);
            statusSlaveSerialPort.Name = "statusSlaveSerialPort";
            statusSlaveSerialPort.Size = new Size(174, 17);
            statusSlaveSerialPort.TabIndex = 2;
            statusSlaveSerialPort.Text = "status-server-SlaveSerialPort";
            // 
            // statusMasterSerialPort
            // 
            statusMasterSerialPort.AutoSize = true;
            statusMasterSerialPort.Location = new Point(186, 164);
            statusMasterSerialPort.Name = "statusMasterSerialPort";
            statusMasterSerialPort.Size = new Size(194, 17);
            statusMasterSerialPort.TabIndex = 2;
            statusMasterSerialPort.Text = "status-connect-MasterSerialPort";
            // 
            // txtSlaveStationNoTcp
            // 
            txtSlaveStationNoTcp.Location = new Point(186, 18);
            txtSlaveStationNoTcp.Name = "txtSlaveStationNoTcp";
            txtSlaveStationNoTcp.PlaceholderText = "station id";
            txtSlaveStationNoTcp.Size = new Size(29, 23);
            txtSlaveStationNoTcp.TabIndex = 3;
            txtSlaveStationNoTcp.Text = "1";
            // 
            // txtSlaveStationNoSerialPort
            // 
            txtSlaveStationNoSerialPort.Location = new Point(186, 118);
            txtSlaveStationNoSerialPort.Name = "txtSlaveStationNoSerialPort";
            txtSlaveStationNoSerialPort.PlaceholderText = "station id";
            txtSlaveStationNoSerialPort.Size = new Size(29, 23);
            txtSlaveStationNoSerialPort.TabIndex = 3;
            txtSlaveStationNoSerialPort.Text = "2";
            // 
            // txtServerSlavePortTcp
            // 
            txtServerSlavePortTcp.Location = new Point(234, 270);
            txtServerSlavePortTcp.Name = "txtServerSlavePortTcp";
            txtServerSlavePortTcp.PlaceholderText = "port";
            txtServerSlavePortTcp.Size = new Size(57, 23);
            txtServerSlavePortTcp.TabIndex = 3;
            txtServerSlavePortTcp.Text = "502";
            // 
            // btnServerStopTcp
            // 
            btnServerStopTcp.Location = new Point(12, 305);
            btnServerStopTcp.Name = "btnServerStopTcp";
            btnServerStopTcp.Size = new Size(161, 35);
            btnServerStopTcp.TabIndex = 1;
            btnServerStopTcp.Text = "停止Slave-Tcp";
            btnServerStopTcp.UseVisualStyleBackColor = true;
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
            // btnServerStopSerialPort
            // 
            btnServerStopSerialPort.Location = new Point(12, 406);
            btnServerStopSerialPort.Name = "btnServerStopSerialPort";
            btnServerStopSerialPort.Size = new Size(161, 35);
            btnServerStopSerialPort.TabIndex = 1;
            btnServerStopSerialPort.Text = "停止Slave-SerialPort";
            btnServerStopSerialPort.UseVisualStyleBackColor = true;
            // 
            // btnDisconnectSerialPort
            // 
            btnDisconnectSerialPort.Location = new Point(12, 154);
            btnDisconnectSerialPort.Name = "btnDisconnectSerialPort";
            btnDisconnectSerialPort.Size = new Size(161, 37);
            btnDisconnectSerialPort.TabIndex = 0;
            btnDisconnectSerialPort.Text = "断开Slave-SerialPort";
            btnDisconnectSerialPort.UseVisualStyleBackColor = true;
            // 
            // txtServerSlavePortSerialPort
            // 
            txtServerSlavePortSerialPort.Location = new Point(234, 371);
            txtServerSlavePortSerialPort.Name = "txtServerSlavePortSerialPort";
            txtServerSlavePortSerialPort.PlaceholderText = "port";
            txtServerSlavePortSerialPort.Size = new Size(56, 23);
            txtServerSlavePortSerialPort.TabIndex = 3;
            txtServerSlavePortSerialPort.Text = "502";
            // 
            // txtServerSlaveStationNoTcp
            // 
            txtServerSlaveStationNoTcp.Location = new Point(186, 270);
            txtServerSlaveStationNoTcp.Name = "txtServerSlaveStationNoTcp";
            txtServerSlaveStationNoTcp.PlaceholderText = "station id";
            txtServerSlaveStationNoTcp.Size = new Size(29, 23);
            txtServerSlaveStationNoTcp.TabIndex = 3;
            txtServerSlaveStationNoTcp.Text = "1";
            // 
            // txtSlaveHostTcp
            // 
            txtSlaveHostTcp.Location = new Point(221, 18);
            txtSlaveHostTcp.Name = "txtSlaveHostTcp";
            txtSlaveHostTcp.PlaceholderText = "host";
            txtSlaveHostTcp.Size = new Size(80, 23);
            txtSlaveHostTcp.TabIndex = 3;
            txtSlaveHostTcp.Text = "127.0.0.1";
            // 
            // txtSlavePortNameSerialPort
            // 
            txtSlavePortNameSerialPort.Location = new Point(234, 118);
            txtSlavePortNameSerialPort.Name = "txtSlavePortNameSerialPort";
            txtSlavePortNameSerialPort.PlaceholderText = "portName";
            txtSlavePortNameSerialPort.Size = new Size(80, 23);
            txtSlavePortNameSerialPort.TabIndex = 3;
            txtSlavePortNameSerialPort.Text = "COM4";
            // 
            // txtSlavePortTcp
            // 
            txtSlavePortTcp.Location = new Point(307, 18);
            txtSlavePortTcp.Name = "txtSlavePortTcp";
            txtSlavePortTcp.PlaceholderText = "port";
            txtSlavePortTcp.Size = new Size(57, 23);
            txtSlavePortTcp.TabIndex = 3;
            txtSlavePortTcp.Text = "502";
            // 
            // txtServerSlavePortNameStationNoSerialPort
            // 
            txtServerSlavePortNameStationNoSerialPort.Location = new Point(186, 371);
            txtServerSlavePortNameStationNoSerialPort.Name = "txtServerSlavePortNameStationNoSerialPort";
            txtServerSlavePortNameStationNoSerialPort.PlaceholderText = "station id";
            txtServerSlavePortNameStationNoSerialPort.Size = new Size(29, 23);
            txtServerSlavePortNameStationNoSerialPort.TabIndex = 3;
            txtServerSlavePortNameStationNoSerialPort.Text = "1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtSlavePortNameSerialPort);
            Controls.Add(txtSlaveStationNoSerialPort);
            Controls.Add(txtServerSlavePortSerialPort);
            Controls.Add(txtSlavePortTcp);
            Controls.Add(txtServerSlavePortTcp);
            Controls.Add(txtServerSlavePortNameStationNoSerialPort);
            Controls.Add(txtServerSlaveStationNoTcp);
            Controls.Add(txtSlaveHostTcp);
            Controls.Add(txtSlaveStationNoTcp);
            Controls.Add(statusMasterSerialPort);
            Controls.Add(statusSlaveSerialPort);
            Controls.Add(statusMasterTcp);
            Controls.Add(statusSlaveTcp);
            Controls.Add(btnServerStopSerialPort);
            Controls.Add(btnServerRunSerialPort);
            Controls.Add(btnServerStopTcp);
            Controls.Add(btnServerRunTcp);
            Controls.Add(btnDisconnectSerialPort);
            Controls.Add(btnConnectSerialPort);
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
        private Button btnServerRunTcp;
        private Button btnConnectSerialPort;
        private Button btnServerRunSerialPort;
        private Label statusSlaveTcp;
        private Label statusMasterTcp;
        private Label statusSlaveSerialPort;
        private Label statusMasterSerialPort;
        private TextBox txtSlaveStationNoTcp;
        private TextBox txtSlaveStationNoSerialPort;
        private TextBox txtServerSlavePortTcp;
        private Button btnServerStopTcp;
        private Button btnDisconnectTcp;
        private Button btnServerStopSerialPort;
        private Button btnDisconnectSerialPort;
        private TextBox txtServerSlavePortSerialPort;
        private TextBox txtServerSlaveStationNoTcp;
        private TextBox txtSlaveHostTcp;
        private TextBox txtSlavePortNameSerialPort;
        private TextBox txtSlavePortTcp;
        private TextBox txtServerSlavePortNameStationNoSerialPort;
    }
}
