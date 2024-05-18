using BrightIdeasSoftware;
using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace WaveZtream
{
    public partial class MusicPanel : Form
    {
        public static MusicPanel instance;
        public static ObjectListView objlistview;

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        public MusicPanel()
        {
            InitializeComponent();
            instance = this;
            objlistview = objectListView1;
            //ProgramManager.SetTitlebarDarkTheme(true);
        }

        public static List<Image> covers = new List<Image>();
        public static AudioDefinition currentDefinition = null;

        private void MusicPanel_Load(object sender, EventArgs e)
        {
            //AllocConsole();

            Start();

            // TODO: MOVE ALL THIS TO A LAYOUT MANAGER

            radDock1.Dock = DockStyle.Fill;

            // My Audio Window
            DocumentWindow myAudio = new DocumentWindow();
            radDock1.AddDocument(myAudio);
            myAudio.Show();
            myAudio.Controls.Add(kryptonPanel5);
            //myAudio.Controls.Add(kryptonButton5);
            //myAudio.Controls.Add(kryptonButton2);
            //myAudio.Controls.Add(kryptonButton4);
            myAudio.Text = "My Audio";
            myAudio.BackColor = Color.Black;
            //myAudio.Width = objectListView1.Width;
            int myAudioWidth = kryptonPanel5.Width;
            kryptonPanel5.Parent = myAudio;
            kryptonPanel5.Location = new Point(0, 0);
            kryptonPanel5.Dock = DockStyle.Fill;
            kryptonButton5.BringToFront();
            kryptonButton2.BringToFront();
            kryptonButton4.BringToFront();

            // Playback Manager Window
            DocumentWindow playbackManager = new DocumentWindow();
            radDock1.AddDocument(playbackManager);
            playbackManager.Show();
            playbackManager.Controls.Add(kryptonPanel1);
            playbackManager.Text = "Playback Manager";
            playbackManager.BackColor = Color.Black;
            //playbackManager.Width = kryptonPanel1.Width;
            int playbackManagerWidth = kryptonPanel1.Width;
            kryptonPanel1.Dock = DockStyle.Fill;

            //myAudio.DockManager.Dock = DockStyle.Left;
            //myAudio.DockState = DockState.Docked;
            //myAudio.Width = myAudioWidth;
            //playbackManager.TabStrip.SizeInfo.AbsoluteSize = new Size(myAudioWidth, playbackManager.TabStrip.SizeInfo.AbsoluteSize.Height);
            //myAudio.DockManager.Width = myAudioWidth;

            //playbackManager.DockState = DockState.Docked;
            //playbackManager.DockManager.Dock = DockStyle.Right;
            //playbackManager.DockManager.Width = playbackManagerWidth;

            this.radDock1.DockWindow(myAudio, DockPosition.Left);
            this.radDock1.DockWindow(playbackManager, DockPosition.Right);
            myAudio.TabStrip.SizeInfo.SizeMode = SplitPanelSizeMode.Relative;
            myAudio.TabStrip.SizeInfo.RelativeRatio = new SizeF(0.43f, 0);

            //objectListView1.SmallImageList.ImageSize = new Size(16, 16);
        }

        public async void Start()
        {
            foreach (OLVColumn item in objectListView1.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(Color.DarkBlue);
                headerstyle.SetForeColor(Color.SlateGray);
                item.HeaderFormatStyle = headerstyle;
            }

            foreach (OLVColumn item in objectListView2.Columns)
            {
                var headerstyle = new HeaderFormatStyle();
                headerstyle.SetBackColor(Color.DarkBlue);
                headerstyle.SetForeColor(Color.SlateGray);
                item.HeaderFormatStyle = headerstyle;
            }

            await LibraryManager.GetAudioFiles();
        }

        private void sld_AudioPosition_Click(object sender, EventArgs e)
        {

        }

        private void kryptonPictureBox2_Click(object sender, EventArgs e)
        {
            PlaybackManager.ReplayAudio();
        }

        private void objectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ///currentDefinition = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            ///PlaybackManager.PlayAudio(LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text)));
        }

        public void InitializeSong(int maxLength)
        {
            lab_audioTitle.Text = currentDefinition.audioTitle;
            lab_audioArtists.Text = currentDefinition.audioArtists;
            if(currentDefinition.audioCover != null) pic_audioImage.Image = currentDefinition.audioCover;

            sld_audioPosition.Minimum = 0;
            sld_audioPosition.Value = 0;
            sld_audioPosition.Maximum = maxLength + 1;

            if (audioUpdate.Enabled == false) audioUpdate.Start();
        }

        public void UpdateTrackPosition()
        {
            if (PlaybackManager.GetLatestOutput() != null && PlaybackManager.GetLatestOutput().output.PlaybackState == PlaybackState.Playing)
            {
                int val = (int)(PlaybackManager.positionOffset + PlaybackManager.GetLatestOutput().output.GetPositionTimeSpan().TotalMilliseconds);
                if(val <= sld_audioPosition.Maximum)
                {
                    sld_audioPosition.Value = val;
                }
            }
        }

        

        private void audioUpdate_Tick(object sender, EventArgs e)
        {
            UpdateTrackPosition();
            QueueManager.CheckForNextSongPreLoad();
            QueueManager.CheckForNextSongPrePlay();
        }

        private void kryptonTrackBar1_MouseEnter(object sender, EventArgs e)
        {

        }

        private void kryptonTrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            audioUpdate.Stop();
        }

        private void kryptonTrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            if(audioUpdate.Enabled == false && e.Button == MouseButtons.Right)
            {
                audioUpdate.Start();
            }
            else
            {
                PlaybackManager.ChangePosition(sld_audioPosition.Value);

                audioUpdate.Start();
            }
        }

        private void kryptonPictureBox3_Click(object sender, EventArgs e)
        {
            PlaybackManager.PauseAudio();
        }

        private void kryptonPictureBox1_Click(object sender, EventArgs e)
        {
            PlaybackManager.ResumeAudio();
        }

        private void sld_audioPosition_ValueChanged(object sender, EventArgs e)
        {

        }

        private void kryptonPictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void kryptonContextMenu1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void playNextToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fixIssuesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            PlaybackManager.LoadNextAudioFromBuffer();
            PlaybackManager.PlayNextAudioFromBuffer();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            AudioDefinition newdef = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            PlaybackManager.AddAudioToBuffer(newdef, new QueuedBuffer());
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            ///currentDefinition = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            ///PlaybackManager.PlayAudio(LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text)));
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            PlaybackManager.PlayNextAudioFromBuffer();
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            PlaybackManager.LoadNextAudioFromBuffer();
        }

        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            AudioDefinition newdef = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            PlaybackManager.AddAudioToBuffer(newdef, new QueuedBuffer());
            PlaybackManager.LoadNextAudioFromBuffer();
            PlaybackManager.PlayNextAudioFromBuffer();
        }

        private void objectListView1_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void addToMainQueueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AudioDefinition newdef = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            QueueManager.AddAudioToQueue(QueueManager.mainQueue, newdef);
        }

        private void kryptonButton1_Click_1(object sender, EventArgs e)
        {
            AutoPlay();
        }

        private async void AutoPlay()
        {
            // Add to buffer
            AudioDefinition newdef = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            PlaybackManager.AddAudioToBuffer(newdef, new RequestBuffer());
            //QueueManager.SortQueuedBuffers();

            await Task.Delay(500);

            // Load with autoplay
            PlaybackManager.AutoPlayNextAudioFromBuffer();
        }

        public void AddWaveOutputToList(string fpath)
        {
            listBox1.Items.Add(fpath);
        }

        public void RemoveWaveOutputFromList(string fpath)
        {
            listBox1.Items.Remove(fpath);
        }
    }

    //[Serializable]
    public class AudioDefinition
    {
        public int index = 0;

        // metadata
        public Image audioCover { get; set; }
        public string audioTitle { get; set; }
        public string audioArtists { get; set; }
        public string audioPrimaryArtist { get; set; }
        public string audioAlbumArtists { get; set; }
        public string audioAlbum { get; set; }

        // type
        public AudioSourceType audioSourceType { get; set; } = AudioSourceType.LocalFile;

        // file specific
        public string audioFileName { get; set; }
        public string audioFilePath { get; set; }
        public string audioFileExtension { get; set; }

        // stream specific

        public string audioFileSourceURL { get; set; }

        //misc
        public bool wasSeparated { get; set; }
        public bool usesMeta { get; set; }
    }

    public enum MetadataHadlingMode
    {
        CopyFileName,
        TrySeparate,
    }

    public enum AudioSourceType
    {
        LocalFile,
        Stream,
    }
}
