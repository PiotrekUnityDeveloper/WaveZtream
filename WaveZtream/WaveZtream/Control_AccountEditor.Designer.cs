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
            // 
            // Control_AccountEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.Controls.Add(this.contentList);
            this.Controls.Add(this.tabList);
            this.Name = "Control_AccountEditor";
            this.Size = new System.Drawing.Size(371, 229);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel tabList;
        private System.Windows.Forms.Panel contentList;
    }
}
