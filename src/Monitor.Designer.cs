namespace ForzaTrackIR
{
    partial class Monitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Monitor));
            this.btnStartStop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboControllers = new System.Windows.Forms.ComboBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblTrackIR = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartStop
            // 
            this.btnStartStop.Enabled = false;
            this.btnStartStop.Location = new System.Drawing.Point(277, 98);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 3;
            this.btnStartStop.Text = "Start";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboControllers);
            this.groupBox1.Controls.Add(this.btnRefresh);
            this.groupBox1.Controls.Add(this.lblTrackIR);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 79);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Devices";
            // 
            // cboControllers
            // 
            this.cboControllers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboControllers.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboControllers.FormattingEnabled = true;
            this.cboControllers.Location = new System.Drawing.Point(78, 18);
            this.cboControllers.Name = "cboControllers";
            this.cboControllers.Size = new System.Drawing.Size(230, 25);
            this.cboControllers.TabIndex = 1;
            // 
            // btnRefresh
            // 
            this.btnRefresh.AccessibleDescription = "Refresh controller list";
            this.btnRefresh.AccessibleName = "Refresh";
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnRefresh.Location = new System.Drawing.Point(314, 18);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(25, 25);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // lblTrackIR
            // 
            this.lblTrackIR.AutoSize = true;
            this.lblTrackIR.Location = new System.Drawing.Point(78, 52);
            this.lblTrackIR.Name = "lblTrackIR";
            this.lblTrackIR.Size = new System.Drawing.Size(94, 17);
            this.lblTrackIR.TabIndex = 3;
            this.lblTrackIR.Text = "Not connected";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "TrackIR";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Controller";
            // 
            // Monitor
            // 
            this.AllowDrop = true;
            this.ClientSize = new System.Drawing.Size(373, 130);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnStartStop);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Monitor";
            this.Text = "TrackIR Wrapper";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblTrackIR;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ComboBox cboControllers;
    }
}

