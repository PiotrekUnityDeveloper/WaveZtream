using BrightIdeasSoftware;
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

namespace WaveZtream
{
    public partial class MusicPanel : Form
    {
        public MusicPanel()
        {
            InitializeComponent();
        }

        private static string[] musicLocation = { "C:\\Users\\Piotr\\Desktop\\Music", "C:\\Users\\Piotr\\Documents\\AudioStreamer\\library\\audio_only" };
        public static List<AudioDefinition> audioFiles = new List<AudioDefinition>();
        public static MetadataHadlingMode metaHandling = MetadataHadlingMode.TrySeparate;
        public static List<Image> covers = new List<Image>();

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

            await GetAudioFiles(musicLocation.ToList());
        }

        public async Task GetAudioFiles(List<string> filePaths)
        {
            await Task.Run(() => { 

                int index = 0;

                foreach (string s in musicLocation)
                {
                    foreach (string file in Directory.GetFiles(s, "*.mp3"))
                    {
                        //  Structure the audio definition here

                        string title = "";
                        string artists = "";
                        string albumartists = "";
                        string primaryartist = "";
                        string album = "";
                        Image audioCover = null;

                        bool wasseparated = false;
                        bool usesmeta = true;

                        try
                        {
                            TagLib.File audioFile = TagLib.File.Create(file);

                            if (String.IsNullOrWhiteSpace(audioFile.Tag.Title))
                            {
                                title = Path.GetFileNameWithoutExtension(file);
                            }
                            else
                            {
                                title = audioFile.Tag.Title;
                            }

                            if (String.IsNullOrWhiteSpace(string.Join(", ", audioFile.Tag.Performers)))
                            {
                                artists = "-";
                            }
                            else
                            {
                                artists = string.Join(", ", audioFile.Tag.Performers);
                            }

                            try
                            {
                                MemoryStream ms = new MemoryStream(audioFile.Tag.Pictures[0].Data.Data);
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                audioCover = image;

                                this.Invoke((MethodInvoker)delegate
                                {
                                    ShowCurrentCover(image);
                                });

                                covers.Add(image);
                            }
                            catch { }
                        }
                        catch (Exception ex)
                        {
                            usesmeta = false;

                            if(metaHandling == MetadataHadlingMode.CopyFileName)
                            {
                                title = Path.GetFileNameWithoutExtension(file);
                                artists = "-";
                            }
                            else if(metaHandling == MetadataHadlingMode.TrySeparate)
                            {
                                wasseparated = true;

                                string nameToSep = Path.GetFileNameWithoutExtension(file);
                                string[] nameSep = nameToSep.Split('-');
                                if(nameSep.Count() > 1 && nameSep.Count() < 3)
                                {
                                    // there is an '-'
                                    artists = nameSep[0];
                                    title = nameSep[1];
                                }
                                else if (nameSep.Count() > 1)
                                {
                                    // there are multiple '-', we should probably skip the third part of the separation.
                                    // but im going to leave it here
                                    // maybe thats a thing to add to the settings, idk
                                    artists = nameSep[0];
                                    title = nameSep[1];
                                }
                                else if(nameSep.Count() == 1)
                                {
                                    // there is probably only the title
                                    title = nameSep[0];
                                    artists = "-";
                                }

                                // artist is usually first, so assume the first string is an artist
                                // unless there is a single artist, then they probably put their names after the title
                                // what do i know, name your files correctly, kids.
                            }
                        }

                        if(audioCover == null)
                        {
                            //switch with a default image which i dont have any atm
                        }

                        audioFiles.Add(new AudioDefinition { audioFileName = Path.GetFileName(file), 
                                                             audioTitle = title, 
                                                             audioArtists = artists, 
                                                             //misc
                                                             usesMeta = usesmeta, 
                                                             wasSeparated = wasseparated,
                                                             index = index });

                        index++;

                        //Console.WriteLine($"Found an MP3 file: {file}");
                    }
                }

                titleColumn.ImageAspectName = "audioCover"; // Specify the aspect name for images
                
                objectListView1.SetObjects(audioFiles);
                //titleColumn.ImageAspectName = "covers";

                this.Invoke((MethodInvoker)delegate
                {
                    LoadCoverImages();
                });

            });
        }

        public void LoadCoverImages()
        {
            ImageList smallImageList = new ImageList();
            smallImageList.ImageSize = new Size(16, 16);
            smallImageList.Images.AddRange(covers.ToArray());
            objectListView1.SmallImageList = smallImageList;
            covers.Clear();

            titleColumn.ImageIndex = 0;
            for (int i = 0; i < objectListView1.Items.Count; i++)
            {
                objectListView1.Items[i].ImageIndex = i;
            }

            objectListView1.Refresh();
        }

        public void ShowCurrentCover(Image img)
        {
            pictureBox1.Image = img;
        }

        private void sld_AudioPosition_Click(object sender, EventArgs e)
        {

        }

        private void kryptonPictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void objectListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
