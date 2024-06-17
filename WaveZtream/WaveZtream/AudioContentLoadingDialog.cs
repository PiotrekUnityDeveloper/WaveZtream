using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveZtream
{
    public partial class AudioContentLoadingDialog : UserControl
    {
        //temp
        public int totalFileCount = 0;
        public int fileCount = 0;

        public AudioContentLoadingDialog()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void AudioContentLoadingDialog_Load(object sender, EventArgs e)
        {

        }

        public void InitAudioList(int totalFiles)
        {
            fileCount = 0;
            totalFileCount = totalFiles;
            totalFileProgress.Minimum = 0;
            totalFileProgress.Maximum = totalFiles + 1;
        }

        public void UpdateAudioData(AudioDefinition def)
        {
            currentAudioName.Text = def.audioTitle;
            currentArtist.Text = def.audioArtists;
            currentCover.Image = def.audioCover;
            currentFileName.Text = def.audioFileName;
            fileCountProgress.Text = "loading file " + fileCount + " out of " + totalFileCount;
            
            totalFileProgress.Value = fileCount;

            fileCount++;

            if(fileCount >= totalFileCount)
            {
                this.Hide();
                this.Dispose();
            }
        }
    }
}
