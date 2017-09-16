namespace TCPChatServer
{
    partial class ServerWindow
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.StartSRV = new System.Windows.Forms.Button();
            this.StopSRV = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.statSRV = new System.Windows.Forms.Label();
            this.logBody = new System.Windows.Forms.RichTextBox();
            this.onlineUser = new System.Windows.Forms.ListBox();
            this.ShowLogs = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartSRV
            // 
            this.StartSRV.Location = new System.Drawing.Point(15, 79);
            this.StartSRV.Name = "StartSRV";
            this.StartSRV.Size = new System.Drawing.Size(75, 42);
            this.StartSRV.TabIndex = 0;
            this.StartSRV.Text = "Запустить сервер";
            this.StartSRV.UseVisualStyleBackColor = true;
            this.StartSRV.Click += new System.EventHandler(this.StartSRV_Click);
            // 
            // StopSRV
            // 
            this.StopSRV.Enabled = false;
            this.StopSRV.Location = new System.Drawing.Point(110, 79);
            this.StopSRV.Name = "StopSRV";
            this.StopSRV.Size = new System.Drawing.Size(75, 42);
            this.StopSRV.TabIndex = 1;
            this.StopSRV.Text = "Остановить сервер";
            this.StopSRV.UseVisualStyleBackColor = true;
            this.StopSRV.Click += new System.EventHandler(this.StopSRV_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Статус сервера:";
            // 
            // statSRV
            // 
            this.statSRV.AutoSize = true;
            this.statSRV.Location = new System.Drawing.Point(107, 23);
            this.statSRV.Name = "statSRV";
            this.statSRV.Size = new System.Drawing.Size(68, 13);
            this.statSRV.TabIndex = 3;
            this.statSRV.Text = "Остановлен";
            // 
            // logBody
            // 
            this.logBody.Location = new System.Drawing.Point(15, 144);
            this.logBody.Name = "logBody";
            this.logBody.ReadOnly = true;
            this.logBody.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.logBody.Size = new System.Drawing.Size(385, 163);
            this.logBody.TabIndex = 4;
            this.logBody.Text = "";
            // 
            // onlineUser
            // 
            this.onlineUser.FormattingEnabled = true;
            this.onlineUser.Location = new System.Drawing.Point(280, 13);
            this.onlineUser.Name = "onlineUser";
            this.onlineUser.Size = new System.Drawing.Size(120, 108);
            this.onlineUser.TabIndex = 5;
            // 
            // ShowLogs
            // 
            this.ShowLogs.Location = new System.Drawing.Point(199, 79);
            this.ShowLogs.Name = "ShowLogs";
            this.ShowLogs.Size = new System.Drawing.Size(75, 42);
            this.ShowLogs.TabIndex = 6;
            this.ShowLogs.Text = "Просмотр логов";
            this.ShowLogs.UseVisualStyleBackColor = true;
            this.ShowLogs.Click += new System.EventHandler(this.ShowLogs_Click);
            // 
            // ServerWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 333);
            this.Controls.Add(this.ShowLogs);
            this.Controls.Add(this.onlineUser);
            this.Controls.Add(this.logBody);
            this.Controls.Add(this.statSRV);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StopSRV);
            this.Controls.Add(this.StartSRV);
            this.Name = "ServerWindow";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerWindow_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartSRV;
        private System.Windows.Forms.Button StopSRV;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label statSRV;
        private System.Windows.Forms.RichTextBox logBody;
        private System.Windows.Forms.ListBox onlineUser;
        private System.Windows.Forms.Button ShowLogs;
    }
}

