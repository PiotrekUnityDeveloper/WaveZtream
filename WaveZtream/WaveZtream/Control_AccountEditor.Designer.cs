namespace WaveZtream
{
    partial class Control_AccountEditor
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
            this.tabList = new System.Windows.Forms.Panel();
            this.contentList = new System.Windows.Forms.Panel();
            this.scrolldown = new System.Windows.Forms.Button();
            this.scrollup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tabList
            // 
            this.tabList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.tabList.Location = new System.Drawing.Point(3, 3);
            this.tabList.Name = "tabList";
            this.tabList.Size = new System.Drawing.Size(78, 223);
            this.tabList.TabIndex = 0;
            // 
            // contentList
            // 
            this.contentList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.contentList.Location = new System.Drawing.Point(87, 3);
            this.contentList.Name = "contentList";
            this.contentList.Size = new System.Drawing.Size(281, 223);
            this.contentList.TabIndex = 1;
            this.contentList.Paint += new System.Windows.Forms.PaintEventHandler(this.contentList_Paint);
            // 
            // scrolldown
            // 
            this.scrolldown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scrolldown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.scrolldown.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.scrolldown.Location = new System.Drawing.Point(0, 210);
            this.scrolldown.Name = "scrolldown";
            this.scrolldown.Size = new System.Drawing.Size(90, 19);
            this.scrolldown.TabIndex = 0;
            this.scrolldown.Text = "V   V   V";
            this.scrolldown.UseVisualStyleBackColor = true;
            this.scrolldown.Visible = false;
            this.scrolldown.Click += new System.EventHandler(this.scrolldown_Click);
            // 
            // scrollup
            // 
            this.scrollup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.scrollup.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.scrollup.ForeColor = System.Drawing.Color.LightSteelBlue;
            this.scrollup.Location = new System.Drawing.Point(0, 0);
            this.scrollup.Name = "scrollup";
            this.scrollup.Size = new System.Drawing.Size(90, 19);
            this.scrollup.TabIndex = 2;
            this.scrollup.Text = "^   ^   ^";
            this.scrollup.UseVisualStyleBackColor = true;
            this.scrollup.Visible = false;
            this.scrollup.Click += new System.EventHandler(this.scrollup_Click);
            // 
            // Control_AccountEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.Controls.Add(this.scrollup);
            this.Controls.Add(this.scrolldown);
            this.Controls.Add(this.contentList);
            this.Controls.Add(this.tabList);
            this.Name = "Control_AccountEditor";
            this.Size = new System.Drawing.Size(371, 229);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel tabList;
        private System.Windows.Forms.Panel contentList;
        private System.Windows.Forms.Button scrolldown;
        private System.Windows.Forms.Button scrollup;
    }
}
