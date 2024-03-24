using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.Mpeg;

namespace WaveZtream
{
    internal class LibraryManager
    {
        private static SearchOption searchOption = SearchOption.TopDirectoryOnly;

        public static async Task GetAudioFiles(List<string> filePaths)
        {
            await Task.Run(() => {

                int index = 0;

                foreach (string s in MusicPanel.musicLocation)
                {
                    foreach (string file in Directory.GetFiles(s, "*.mp3", searchOption)
                                 .Union(Directory.GetFiles(s, "*.wav", searchOption))
                                 .Union(Directory.GetFiles(s, "*.flac", searchOption))
                                 .Union(Directory.GetFiles(s, "*.aiff", searchOption))
                                 .Union(Directory.GetFiles(s, "*.ogg", searchOption))
                                 .Union(Directory.GetFiles(s, "*.aac", searchOption))
                                 .Union(Directory.GetFiles(s, "*.wma", searchOption))
                                 .Union(Directory.GetFiles(s, "*.m4a", searchOption))
                                 .Union(Directory.GetFiles(s, "*.opus", searchOption))
                                 .Union(Directory.GetFiles(s, "*.mp1", searchOption))
                                 .Union(Directory.GetFiles(s, "*.mp2", searchOption)))
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

                                MusicPanel.instance.Invoke((MethodInvoker)delegate
                                {
                                    ShowCurrentCover(image);
                                });

                                MusicPanel.covers.Add(WaveUtils.ResizeImage(image, 6, 6));
                            }
                            catch { }
                        }
                        catch (Exception ex)
                        {
                            usesmeta = false;

                            if (MusicPanel.metaHandling == MetadataHadlingMode.CopyFileName)
                            {
                                title = Path.GetFileNameWithoutExtension(file);
                                artists = "-";
                            }
                            else if (MusicPanel.metaHandling == MetadataHadlingMode.TrySeparate)
                            {
                                wasseparated = true;

                                string nameToSep = Path.GetFileNameWithoutExtension(file);
                                string[] nameSep = nameToSep.Split('-');
                                if (nameSep.Count() > 1 && nameSep.Count() < 3)
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
                                else if (nameSep.Count() == 1)
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

                        if (audioCover == null)
                        {
                            //switch with a default image which i dont have any atm
                        }

                        MusicPanel.audioFiles.Add(new AudioDefinition
                        {
                            audioFileName = Path.GetFileName(file),
                            audioCover = audioCover,
                            audioTitle = title,
                            audioArtists = artists,
                            //misc
                            usesMeta = usesmeta,
                            wasSeparated = wasseparated,
                            index = index,
                            audioFilePath = file,
                            audioFileExtension = Path.GetExtension(file),
                        });

                        index++;

                        //Console.WriteLine($"Found an MP3 file: {file}");
                    }
                }

                //titleColumn.ImageAspectName = "audioCover"; // Specify the aspect name for images

                MusicPanel.objlistview.SetObjects(MusicPanel.audioFiles);
                //titleColumn.ImageAspectName = "covers";

                MusicPanel.instance.Invoke((MethodInvoker)delegate
                {
                    LoadCoverImages();
                });

            });
        }

        public static void LoadCoverImages()
        {
            ImageList smallImageList = new ImageList();
            smallImageList.ImageSize = new Size(6, 6);
            smallImageList.Images.Clear();
            smallImageList.Images.AddRange(MusicPanel.covers.ToArray());
            MusicPanel.objlistview.SmallImageList = smallImageList;
            MusicPanel.covers.Clear();

            for (int i = 0; i < MusicPanel.objlistview.Items.Count; i++)
            {
                MusicPanel.objlistview.Items[i].ImageIndex = i;
            }

            MusicPanel.objlistview.Refresh();
        }

        public static void ShowCurrentCover(Image img)
        {
            //Random rnd = new Random();
            //img.Save("C:\\Users\\Piotr\\Desktop\\image" + rnd.Next(000000, 999999) + ".png");
        }

        public static AudioDefinition GetDefinitionByIndex(int id)
        {
            return MusicPanel.audioFiles[id];
        }

    }
}
