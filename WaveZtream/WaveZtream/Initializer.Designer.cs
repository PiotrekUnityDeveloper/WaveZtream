namespace WaveZtream
{
    partial class Initializer
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
            this.components = new System.ComponentModel.Container();
            this.LoadingMessage = new System.Windows.Forms.Label();
            this.WindowsExtender = new System.Windows.Forms.Timer(this.components);
            this.waveLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.waveLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadingMessage
            // 
            this.LoadingMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.LoadingMessage.Font = new System.Drawing.Font("My Font", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadingMessage.ForeColor = System.Drawing.Color.White;
            this.LoadingMessage.Location = new System.Drawing.Point(0, 343);
            this.LoadingMessage.Name = "LoadingMessage";
            this.LoadingMessage.Size = new System.Drawing.Size(672, 60);
            this.LoadingMessage.TabIndex = 1;
            this.LoadingMessage.Text = "INITIALIZING...";
            this.LoadingMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LoadingMessage.Visible = false;
            this.LoadingMessage.Click += new System.EventHandler(this.LoadingMessage_Click);
            // 
            // WindowsExtender
            // 
            this.WindowsExtender.Interval = 1;
            this.WindowsExtender.Tick += new System.EventHandler(this.WindowsExtender_Tick);
            // 
            // waveLogo
            // 
            this.waveLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.waveLogo.Image = global::WaveZtream.Properties.Resources.WAVEZTREAM_SHINE;
            this.waveLogo.Location = new System.Drawing.Point(0, 0);
            this.waveLogo.Name = "waveLogo";
            this.waveLogo.Size = new System.Drawing.Size(672, 388);
            this.waveLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.waveLogo.TabIndex = 0;
            this.waveLogo.TabStop = false;
            // 
            // Initializer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(672, 403);
            this.Controls.Add(this.LoadingMessage);
            this.Controls.Add(this.waveLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Initializer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WAVEZTREAM";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.waveLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox waveLogo;
        private System.Windows.Forms.Label LoadingMessage;
        private System.Windows.Forms.Timer WindowsExtender;
    }
}

