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
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI.Docking;

namespace WaveZtream
{
    public partial class MusicPanel : Form
    {
        public static MusicPanel instance;
        public static ObjectListView objlistview;

        public MusicPanel()
        {
            InitializeComponent();
            instance = this;
            objlistview = objectListView1;
        }

        public static string[] musicLocation = { "C:\\Users\\Piotr\\Desktop\\Music",};
        public static List<AudioDefinition> audioFiles = new List<AudioDefinition>();
        public static MetadataHadlingMode metaHandling = MetadataHadlingMode.TrySeparate;
        public static List<Image> covers = new List<Image>();
        public static AudioDefinition currentDefinition = null;

        private void MusicPanel_Load(object sender, EventArgs e)
        {
            Start();
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

            await LibraryManager.GetAudioFiles(musicLocation.ToList());
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
            currentDefinition = LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text));
            PlaybackManager.PlayAudio(LibraryManager.GetDefinitionByIndex(Convert.ToInt32(objectListView1.FocusedItem.SubItems[0].Text)));
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
            if (PlaybackManager.audioHandler != null && PlaybackManager.audioHandler.PlaybackState == PlaybackState.Playing)
            {
                int val = (int)(PlaybackManager.positionOffset + PlaybackManager.audioHandler.GetPositionTimeSpan().TotalMilliseconds);
                if(val <= sld_audioPosition.Maximum)
                {
                    sld_audioPosition.Value = val;
                }
            }
        }

        private void audioUpdate_Tick(object sender, EventArgs e)
        {
            UpdateTrackPosition();
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
    }

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

        // file specific
        public string audioFileName { get; set; }
        public string audioFilePath { get; set; }
        public string audioFileExtension { get; set; }

        //misc
        public bool wasSeparated { get; set; }
        public bool usesMeta { get; set; }
    }

    public enum MetadataHadlingMode
    {
        CopyFileName,
        TrySeparate,
    }
}
