namespace ForzaTrackIR
{
    partial class Monitor
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblPitch = new System.Windows.Forms.Label();
            this.lblYaw = new System.Windows.Forms.Label();
            this.lblButtonA = new System.Windows.Forms.Label();
            this.lblButtonB = new System.Windows.Forms.Label();
            this.lblButtonX = new System.Windows.Forms.Label();
            this.lblButtonY = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Pitch";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Yaw";
            // 
            // lblPitch
            // 
            this.lblPitch.AutoSize = true;
            this.lblPitch.Location = new System.Drawing.Point(51, 13);
            this.lblPitch.Name = "lblPitch";
            this.lblPitch.Size = new System.Drawing.Size(0, 13);
            this.lblPitch.TabIndex = 2;
            // 
            // lblYaw
            // 
            this.lblYaw.AutoSize = true;
            this.lblYaw.Location = new System.Drawing.Point(54, 37);
            this.lblYaw.Name = "lblYaw";
            this.lblYaw.Size = new System.Drawing.Size(0, 13);
            this.lblYaw.TabIndex = 3;
            // 
            // lblButtonA
            // 
            this.lblButtonA.AutoSize = true;
            this.lblButtonA.Location = new System.Drawing.Point(13, 91);
            this.lblButtonA.Name = "lblButtonA";
            this.lblButtonA.Size = new System.Drawing.Size(48, 13);
            this.lblButtonA.TabIndex = 4;
            this.lblButtonA.Text = "A Button";
            // 
            // lblButtonB
            // 
            this.lblButtonB.AutoSize = true;
            this.lblButtonB.Location = new System.Drawing.Point(13, 114);
            this.lblButtonB.Name = "lblButtonB";
            this.lblButtonB.Size = new System.Drawing.Size(48, 13);
            this.lblButtonB.TabIndex = 5;
            this.lblButtonB.Text = "B Button";
            // 
            // lblButtonX
            // 
            this.lblButtonX.AutoSize = true;
            this.lblButtonX.Location = new System.Drawing.Point(13, 138);
            this.lblButtonX.Name = "lblButtonX";
            this.lblButtonX.Size = new System.Drawing.Size(48, 13);
            this.lblButtonX.TabIndex = 6;
            this.lblButtonX.Text = "X Button";
            // 
            // lblButtonY
            // 
            this.lblButtonY.AutoSize = true;
            this.lblButtonY.Location = new System.Drawing.Point(13, 161);
            this.lblButtonY.Name = "lblButtonY";
            this.lblButtonY.Size = new System.Drawing.Size(48, 13);
            this.lblButtonY.TabIndex = 7;
            this.lblButtonY.Text = "Y Button";
            // 
            // btnStartStop
            // 
            this.btnStartStop.Location = new System.Drawing.Point(197, 226);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(75, 23);
            this.btnStartStop.TabIndex = 8;
            this.btnStartStop.Text = "Start/Stop";
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // Monitor
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblButtonY);
            this.Controls.Add(this.lblButtonX);
            this.Controls.Add(this.lblButtonB);
            this.Controls.Add(this.lblButtonA);
            this.Controls.Add(this.lblYaw);
            this.Controls.Add(this.lblPitch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "Monitor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblPitch;
        private System.Windows.Forms.Label lblYaw;
        private System.Windows.Forms.Label lblButtonA;
        private System.Windows.Forms.Label lblButtonB;
        private System.Windows.Forms.Label lblButtonX;
        private System.Windows.Forms.Label lblButtonY;
        private System.Windows.Forms.Button btnStartStop;
    }
}

