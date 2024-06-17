namespace WaveZtream
{
    partial class AudioContentLoadingDialog
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.currentCover = new Krypton.Toolkit.KryptonPictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.currentAudioName = new Krypton.Toolkit.KryptonLabel();
            this.currentOperation = new System.Windows.Forms.Label();
            this.loadingProgress = new Krypton.Toolkit.KryptonProgressBar();
            this.totalFileProgress = new System.Windows.Forms.ProgressBar();
            this.currentFileName = new System.Windows.Forms.Label();
            this.currentArtist = new System.Windows.Forms.Label();
            this.fileCountProgress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.currentCover)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // currentCover
            // 
            this.currentCover.Image = global::WaveZtream.Properties.Resources.buttonBackground_Default;
            this.currentCover.Location = new System.Drawing.Point(6, 19);
            this.currentCover.Name = "currentCover";
            this.currentCover.Size = new System.Drawing.Size(171, 164);
            this.currentCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.currentCover.TabIndex = 0;
            this.currentCover.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.fileCountProgress);
            this.groupBox1.Controls.Add(this.currentArtist);
            this.groupBox1.Controls.Add(this.currentFileName);
            this.groupBox1.Controls.Add(this.totalFileProgress);
            this.groupBox1.Controls.Add(this.loadingProgress);
            this.groupBox1.Controls.Add(this.currentAudioName);
            this.groupBox1.Controls.Add(this.currentCover);
            this.groupBox1.Controls.Add(this.currentOperation);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(386, 189);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Please wait... Loading Audio List Data...";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // currentAudioName
            // 
            this.currentAudioName.AutoSize = false;
            this.currentAudioName.Location = new System.Drawing.Point(183, 42);
            this.currentAudioName.Name = "currentAudioName";
            this.currentAudioName.PaletteMode = Krypton.Toolkit.PaletteMode.Office2007BlackDarkMode;
            this.currentAudioName.Size = new System.Drawing.Size(197, 18);
            this.currentAudioName.TabIndex = 1;
            this.currentAudioName.Values.Text = "Audio";
            // 
            // currentOperation
            // 
            this.currentOperation.AutoSize = true;
            this.currentOperation.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.currentOperation.Location = new System.Drawing.Point(183, 19);
            this.currentOperation.Name = "currentOperation";
            this.currentOperation.Size = new System.Drawing.Size(173, 20);
            this.currentOperation.TabIndex = 2;
            this.currentOperation.Text = "Loading Audio List...";
            // 
            // loadingProgress
            // 
            this.loadingProgress.Location = new System.Drawing.Point(183, 156);
            this.loadingProgress.Name = "loadingProgress";
            this.loadingProgress.Size = new System.Drawing.Size(197, 27);
            this.loadingProgress.StateCommon.Back.Color1 = System.Drawing.Color.Green;
            this.loadingProgress.StateDisabled.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.OneNote;
            this.loadingProgress.StateNormal.Back.ColorStyle = Krypton.Toolkit.PaletteColorStyle.OneNote;
            this.loadingProgress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.loadingProgress.TabIndex = 3;
            this.loadingProgress.Text = "Loading...";
            this.loadingProgress.Values.Text = "Loading...";
            // 
            // totalFileProgress
            // 
            this.totalFileProgress.Location = new System.Drawing.Point(183, 149);
            this.totalFileProgress.Name = "totalFileProgress";
            this.totalFileProgress.Size = new System.Drawing.Size(197, 10);
            this.totalFileProgress.TabIndex = 4;
            this.totalFileProgress.Value = 75;
            // 
            // currentFileName
            // 
            this.currentFileName.AutoSize = true;
            this.currentFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.currentFileName.Location = new System.Drawing.Point(180, 124);
            this.currentFileName.Name = "currentFileName";
            this.currentFileName.Size = new System.Drawing.Size(43, 13);
            this.currentFileName.TabIndex = 5;
            this.currentFileName.Text = "file.mp3";
            // 
            // currentArtist
            // 
            this.currentArtist.AutoSize = true;
            this.currentArtist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.currentArtist.Location = new System.Drawing.Point(180, 63);
            this.currentArtist.Name = "currentArtist";
            this.currentArtist.Size = new System.Drawing.Size(29, 13);
            this.currentArtist.TabIndex = 6;
            this.currentArtist.Text = "artist";
            // 
            // fileCountProgress
            // 
            this.fileCountProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.fileCountProgress.Location = new System.Drawing.Point(194, 92);
            this.fileCountProgress.Name = "fileCountProgress";
            this.fileCountProgress.Size = new System.Drawing.Size(186, 13);
            this.fileCountProgress.TabIndex = 7;
            this.fileCountProgress.Text = "reading file 0 of 350";
            // 
            // AudioContentLoadingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "AudioContentLoadingDialog";
            this.Size = new System.Drawing.Size(392, 195);
            this.Load += new System.EventHandler(this.AudioContentLoadingDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.currentCover)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Krypton.Toolkit.KryptonPictureBox currentCover;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label fileCountProgress;
        private System.Windows.Forms.Label currentArtist;
        private System.Windows.Forms.Label currentFileName;
        private System.Windows.Forms.ProgressBar totalFileProgress;
        private Krypton.Toolkit.KryptonProgressBar loadingProgress;
        private Krypton.Toolkit.KryptonLabel currentAudioName;
        private System.Windows.Forms.Label currentOperation;
    }
}
