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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.dgvWorkers = new System.Windows.Forms.DataGridView();
            this.WorkerPID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartWorker = new System.Windows.Forms.DataGridViewButtonColumn();
            this.StopWorker = new System.Windows.Forms.DataGridViewButtonColumn();
            this.RemoveWorker = new System.Windows.Forms.DataGridViewButtonColumn();
            this.btnAddWindow = new System.Windows.Forms.Button();
            this.tbPID = new System.Windows.Forms.TextBox();
            this.lblPID = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkers)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(4, 187);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(477, 84);
            this.listBox1.TabIndex = 2;
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
            this.dgvWorkers.RowHeadersVisible = false;
            this.dgvWorkers.RowTemplate.Height = 24;
            this.dgvWorkers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkers.Size = new System.Drawing.Size(477, 150);
            this.dgvWorkers.TabIndex = 6;
            this.dgvWorkers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkers_CellContentClick);
            // 
            // WorkerPID
            // 
            this.WorkerPID.DataPropertyName = "PID";
            this.WorkerPID.HeaderText = "PID";
            this.WorkerPID.Name = "WorkerPID";
            // 
            // StartWorker
            // 
            this.StartWorker.HeaderText = "";
            this.StartWorker.Name = "StartWorker";
            this.StartWorker.Text = "Start";
            this.StartWorker.UseColumnTextForButtonValue = true;
            // 
            // StopWorker
            // 
            this.StopWorker.HeaderText = "";
            this.StopWorker.Name = "StopWorker";
            this.StopWorker.Text = "Stop";
            this.StopWorker.UseColumnTextForButtonValue = true;
            // 
            // RemoveWorker
            // 
            this.RemoveWorker.HeaderText = "";
            this.RemoveWorker.Name = "RemoveWorker";
            this.RemoveWorker.Text = "Remove";
            this.RemoveWorker.UseColumnTextForButtonValue = true;
            // 
            // btnAddWindow
            // 
            this.btnAddWindow.Location = new System.Drawing.Point(189, 3);
            this.btnAddWindow.Name = "btnAddWindow";
            this.btnAddWindow.Size = new System.Drawing.Size(44, 22);
            this.btnAddWindow.TabIndex = 7;
            this.btnAddWindow.Text = "+";
            this.btnAddWindow.UseVisualStyleBackColor = true;
            this.btnAddWindow.Click += new System.EventHandler(this.btnAddWindow_Click);
            // 
            // tbPID
            // 
            this.tbPID.Location = new System.Drawing.Point(83, 3);
            this.tbPID.Name = "tbPID";
            this.tbPID.Size = new System.Drawing.Size(100, 22);
            this.tbPID.TabIndex = 8;
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
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 274);
            this.Controls.Add(this.lblPID);
            this.Controls.Add(this.tbPID);
            this.Controls.Add(this.btnAddWindow);
            this.Controls.Add(this.dgvWorkers);
            this.Controls.Add(this.listBox1);
            this.Name = "MainView";
            this.Text = "Bot for Shmot pre-pre-pre-alpha";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DataGridView dgvWorkers;
        private System.Windows.Forms.Button btnAddWindow;
        private System.Windows.Forms.TextBox tbPID;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkerPID;
        private System.Windows.Forms.DataGridViewButtonColumn StartWorker;
        private System.Windows.Forms.DataGridViewButtonColumn StopWorker;
        private System.Windows.Forms.DataGridViewButtonColumn RemoveWorker;
        private System.Windows.Forms.Label lblPID;
    }
}

