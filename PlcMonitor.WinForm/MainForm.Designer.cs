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
            btnConnectTcp = new Button();
            btnServerRunTcp = new Button();
            btnConnectSerialPort = new Button();
            btnServerRunSerialPort = new Button();
            statusSlaveTcp = new Label();
            statusMasterTcp = new Label();
            statusSlaveSerialPort = new Label();
            statusMasterSerialPort = new Label();
            SuspendLayout();
            // 
            // btnConnectTcp
            // 
            btnConnectTcp.Location = new Point(12, 82);
            btnConnectTcp.Name = "btnConnectTcp";
            btnConnectTcp.Size = new Size(161, 35);
            btnConnectTcp.TabIndex = 0;
            btnConnectTcp.Text = "连接Slave-Tcp";
            btnConnectTcp.UseVisualStyleBackColor = true;
            // 
            // btnServerRunTcp
            // 
            btnServerRunTcp.Location = new Point(12, 12);
            btnServerRunTcp.Name = "btnServerRunTcp";
            btnServerRunTcp.Size = new Size(161, 35);
            btnServerRunTcp.TabIndex = 1;
            btnServerRunTcp.Text = "启动Slave-Tcp";
            btnServerRunTcp.UseVisualStyleBackColor = true;
            // 
            // btnConnectSerialPort
            // 
            btnConnectSerialPort.Location = new Point(12, 306);
            btnConnectSerialPort.Name = "btnConnectSerialPort";
            btnConnectSerialPort.Size = new Size(161, 37);
            btnConnectSerialPort.TabIndex = 0;
            btnConnectSerialPort.Text = "连接Slave-SerialPort";
            btnConnectSerialPort.UseVisualStyleBackColor = true;
            // 
            // btnServerRunSerialPort
            // 
            btnServerRunSerialPort.Location = new Point(12, 223);
            btnServerRunSerialPort.Name = "btnServerRunSerialPort";
            btnServerRunSerialPort.Size = new Size(161, 35);
            btnServerRunSerialPort.TabIndex = 1;
            btnServerRunSerialPort.Text = "启动Slave-SerialPort";
            btnServerRunSerialPort.UseVisualStyleBackColor = true;
            // 
            // statusSlaveTcp
            // 
            statusSlaveTcp.AutoSize = true;
            statusSlaveTcp.Location = new Point(205, 21);
            statusSlaveTcp.Name = "statusSlaveTcp";
            statusSlaveTcp.Size = new Size(93, 17);
            statusSlaveTcp.TabIndex = 2;
            statusSlaveTcp.Text = "statusSlaveTcp";
            // 
            // statusMasterTcp
            // 
            statusMasterTcp.AutoSize = true;
            statusMasterTcp.Location = new Point(205, 91);
            statusMasterTcp.Name = "statusMasterTcp";
            statusMasterTcp.Size = new Size(104, 17);
            statusMasterTcp.TabIndex = 2;
            statusMasterTcp.Text = "statusMasterTcp";
            // 
            // statusSlaveSerialPort
            // 
            statusSlaveSerialPort.AutoSize = true;
            statusSlaveSerialPort.Location = new Point(205, 232);
            statusSlaveSerialPort.Name = "statusSlaveSerialPort";
            statusSlaveSerialPort.Size = new Size(128, 17);
            statusSlaveSerialPort.TabIndex = 2;
            statusSlaveSerialPort.Text = "statusSlaveSerialPort";
            // 
            // statusMasterSerialPort
            // 
            statusMasterSerialPort.AutoSize = true;
            statusMasterSerialPort.Location = new Point(205, 316);
            statusMasterSerialPort.Name = "statusMasterSerialPort";
            statusMasterSerialPort.Size = new Size(139, 17);
            statusMasterSerialPort.TabIndex = 2;
            statusMasterSerialPort.Text = "statusMasterSerialPort";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusMasterSerialPort);
            Controls.Add(statusSlaveSerialPort);
            Controls.Add(statusMasterTcp);
            Controls.Add(statusSlaveTcp);
            Controls.Add(btnServerRunSerialPort);
            Controls.Add(btnServerRunTcp);
            Controls.Add(btnConnectSerialPort);
            Controls.Add(btnConnectTcp);
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
    }
}
