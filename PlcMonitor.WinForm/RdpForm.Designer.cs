namespace PlcMonitor.WinForm
{
    partial class RdpForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RdpForm));
            axMsRdpClient11NotSafeForScripting1 = new AxMSTSCLib.AxMsRdpClient11NotSafeForScripting();
            BTN_Connect = new Button();
            TB_Port = new TextBox();
            TB_Host = new TextBox();
            TB_UserName = new TextBox();
            TB_Pwd = new TextBox();
            BTN_Disconnect = new Button();
            TB_Log = new TextBox();
            ((System.ComponentModel.ISupportInitialize)axMsRdpClient11NotSafeForScripting1).BeginInit();
            SuspendLayout();
            // 
            // axMsRdpClient11NotSafeForScripting1
            // 
            axMsRdpClient11NotSafeForScripting1.Enabled = true;
            axMsRdpClient11NotSafeForScripting1.Location = new Point(1, 41);
            axMsRdpClient11NotSafeForScripting1.Name = "axMsRdpClient11NotSafeForScripting1";
            axMsRdpClient11NotSafeForScripting1.OcxState = (AxHost.State)resources.GetObject("axMsRdpClient11NotSafeForScripting1.OcxState");
            axMsRdpClient11NotSafeForScripting1.Size = new Size(929, 669);
            axMsRdpClient11NotSafeForScripting1.TabIndex = 0;
            // 
            // BTN_Connect
            // 
            BTN_Connect.Location = new Point(12, 12);
            BTN_Connect.Name = "BTN_Connect";
            BTN_Connect.Size = new Size(75, 23);
            BTN_Connect.TabIndex = 1;
            BTN_Connect.Text = "连接";
            BTN_Connect.UseVisualStyleBackColor = true;
            BTN_Connect.Click += BTN_Connect_Click;
            // 
            // TB_Port
            // 
            TB_Port.Location = new Point(244, 12);
            TB_Port.Name = "TB_Port";
            TB_Port.Size = new Size(58, 23);
            TB_Port.TabIndex = 2;
            // 
            // TB_Host
            // 
            TB_Host.Location = new Point(108, 12);
            TB_Host.Name = "TB_Host";
            TB_Host.Size = new Size(130, 23);
            TB_Host.TabIndex = 2;
            // 
            // TB_UserName
            // 
            TB_UserName.Location = new Point(328, 12);
            TB_UserName.Name = "TB_UserName";
            TB_UserName.Size = new Size(130, 23);
            TB_UserName.TabIndex = 2;
            // 
            // TB_Pwd
            // 
            TB_Pwd.Location = new Point(464, 12);
            TB_Pwd.Name = "TB_Pwd";
            TB_Pwd.Size = new Size(130, 23);
            TB_Pwd.TabIndex = 2;
            // 
            // BTN_Disconnect
            // 
            BTN_Disconnect.Location = new Point(600, 12);
            BTN_Disconnect.Name = "BTN_Disconnect";
            BTN_Disconnect.Size = new Size(75, 23);
            BTN_Disconnect.TabIndex = 1;
            BTN_Disconnect.Text = "断开";
            BTN_Disconnect.UseVisualStyleBackColor = true;
            BTN_Disconnect.Click += BTN_Disconnect_Click;
            // 
            // TB_Log
            // 
            TB_Log.Location = new Point(951, 1);
            TB_Log.Multiline = true;
            TB_Log.Name = "TB_Log";
            TB_Log.Size = new Size(345, 709);
            TB_Log.TabIndex = 3;
            // 
            // RdpForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1297, 712);
            Controls.Add(TB_Log);
            Controls.Add(TB_Pwd);
            Controls.Add(TB_UserName);
            Controls.Add(TB_Host);
            Controls.Add(TB_Port);
            Controls.Add(BTN_Disconnect);
            Controls.Add(BTN_Connect);
            Controls.Add(axMsRdpClient11NotSafeForScripting1);
            Name = "RdpForm";
            Text = "RdpForm";
            Load += RdpForm_Load;
            ((System.ComponentModel.ISupportInitialize)axMsRdpClient11NotSafeForScripting1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private AxMSTSCLib.AxMsRdpClient11NotSafeForScripting axMsRdpClient11NotSafeForScripting1;
        private Button BTN_Connect;
        private TextBox TB_Port;
        private TextBox TB_Host;
        private TextBox TB_UserName;
        private TextBox TB_Pwd;
        private Button BTN_Disconnect;
        private TextBox TB_Log;
    }
}