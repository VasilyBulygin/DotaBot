namespace bot
{
    partial class MainView
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbLog = new System.Windows.Forms.ListBox();
            this.dgvWorkers = new System.Windows.Forms.DataGridView();
            this.btnAddWindow = new System.Windows.Forms.Button();
            this.lblPID = new System.Windows.Forms.Label();
            this.pgWorkerProperties = new System.Windows.Forms.PropertyGrid();
            this.cbPID = new System.Windows.Forms.ComboBox();
            this.WorkerPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartWorker = new System.Windows.Forms.DataGridViewButtonColumn();
            this.StopWorker = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RemoveWorker = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnRefreshProcesses = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkers)).BeginInit();
            this.SuspendLayout();
            // 
            // lbLog
            // 
            this.lbLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLog.FormattingEnabled = true;
            this.lbLog.ItemHeight = 16;
            this.lbLog.Location = new System.Drawing.Point(4, 187);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(363, 84);
            this.lbLog.TabIndex = 2;
            // 
            // dgvWorkers
            // 
            this.dgvWorkers.AllowUserToAddRows = false;
            this.dgvWorkers.AllowUserToDeleteRows = false;
            this.dgvWorkers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWorkers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvWorkers.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvWorkers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.WorkerPID,
            this.StartWorker,
            this.StopWorker,
            this.RemoveWorker});
            this.dgvWorkers.Location = new System.Drawing.Point(4, 31);
            this.dgvWorkers.Name = "dgvWorkers";
            this.dgvWorkers.ReadOnly = true;
            this.dgvWorkers.RowHeadersVisible = false;
            this.dgvWorkers.RowTemplate.Height = 24;
            this.dgvWorkers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkers.Size = new System.Drawing.Size(363, 150);
            this.dgvWorkers.TabIndex = 6;
            this.dgvWorkers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkers_CellContentClick);
            this.dgvWorkers.SelectionChanged += new System.EventHandler(this.dgvWorkers_SelectionChanged);
            // 
            // btnAddWindow
            // 
            this.btnAddWindow.Location = new System.Drawing.Point(214, 3);
            this.btnAddWindow.Name = "btnAddWindow";
            this.btnAddWindow.Size = new System.Drawing.Size(44, 24);
            this.btnAddWindow.TabIndex = 7;
            this.btnAddWindow.Text = "+";
            this.btnAddWindow.UseVisualStyleBackColor = true;
            this.btnAddWindow.Click += new System.EventHandler(this.btnAddWindow_Click);
            // 
            // lblPID
            // 
            this.lblPID.AutoSize = true;
            this.lblPID.Location = new System.Drawing.Point(1, 6);
            this.lblPID.Name = "lblPID";
            this.lblPID.Size = new System.Drawing.Size(80, 17);
            this.lblPID.TabIndex = 9;
            this.lblPID.Text = "Process ID:";
            // 
            // pgWorkerProperties
            // 
            this.pgWorkerProperties.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgWorkerProperties.Location = new System.Drawing.Point(373, 3);
            this.pgWorkerProperties.Name = "pgWorkerProperties";
            this.pgWorkerProperties.Size = new System.Drawing.Size(339, 268);
            this.pgWorkerProperties.TabIndex = 1;
            // 
            // cbPID
            // 
            this.cbPID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPID.FormattingEnabled = true;
            this.cbPID.Location = new System.Drawing.Point(87, 3);
            this.cbPID.Name = "cbPID";
            this.cbPID.Size = new System.Drawing.Size(121, 24);
            this.cbPID.TabIndex = 11;
            // 
            // WorkerPID
            // 
            this.WorkerPID.DataPropertyName = "PID";
            this.WorkerPID.HeaderText = "PID";
            this.WorkerPID.Name = "WorkerPID";
            this.WorkerPID.ReadOnly = true;
            // 
            // StartWorker
            // 
            this.StartWorker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StartWorker.FillWeight = 15F;
            this.StartWorker.HeaderText = "";
            this.StartWorker.Name = "StartWorker";
            this.StartWorker.ReadOnly = true;
            this.StartWorker.Text = "Start";
            this.StartWorker.UseColumnTextForButtonValue = true;
            this.StartWorker.Width = 44;
            // 
            // StopWorker
            // 
            this.StopWorker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StopWorker.FillWeight = 15F;
            this.StopWorker.HeaderText = "";
            this.StopWorker.Name = "StopWorker";
            this.StopWorker.ReadOnly = true;
            this.StopWorker.Text = "Stop";
            this.StopWorker.UseColumnTextForButtonValue = true;
            this.StopWorker.Width = 52;
            // 
            // RemoveWorker
            // 
            this.RemoveWorker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.RemoveWorker.FillWeight = 20F;
            this.RemoveWorker.HeaderText = "";
            this.RemoveWorker.Name = "RemoveWorker";
            this.RemoveWorker.ReadOnly = true;
            this.RemoveWorker.Text = "Del";
            this.RemoveWorker.UseColumnTextForButtonValue = true;
            this.RemoveWorker.Width = 44;
            // 
            // btnRefreshProcesses
            // 
            this.btnRefreshProcesses.Location = new System.Drawing.Point(264, 3);
            this.btnRefreshProcesses.Name = "btnRefreshProcesses";
            this.btnRefreshProcesses.Size = new System.Drawing.Size(49, 26);
            this.btnRefreshProcesses.TabIndex = 12;
            this.btnRefreshProcesses.Text = "R";
            this.btnRefreshProcesses.UseVisualStyleBackColor = true;
            this.btnRefreshProcesses.Click += new System.EventHandler(this.btnRefreshProcesses_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(715, 274);
            this.Controls.Add(this.btnRefreshProcesses);
            this.Controls.Add(this.pgWorkerProperties);
            this.Controls.Add(this.cbPID);
            this.Controls.Add(this.lblPID);
            this.Controls.Add(this.btnAddWindow);
            this.Controls.Add(this.dgvWorkers);
            this.Controls.Add(this.lbLog);
            this.MinimumSize = new System.Drawing.Size(733, 321);
            this.Name = "MainView";
            this.Text = "Bot for Shmot pre-pre-pre-alpha";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.DataGridView dgvWorkers;
        private System.Windows.Forms.Button btnAddWindow;
        private System.Windows.Forms.Label lblPID;
        private System.Windows.Forms.PropertyGrid pgWorkerProperties;
        private System.Windows.Forms.ComboBox cbPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkerPID;
        private System.Windows.Forms.DataGridViewButtonColumn StartWorker;
        private System.Windows.Forms.DataGridViewButtonColumn StopWorker;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveWorker;
        private System.Windows.Forms.Button btnRefreshProcesses;
    }
}

