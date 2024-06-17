using Accord.IO;
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
        public static LibraryGroup mainLibrary = new LibraryGroup()
        {
            groupName = "mainLib",
            customLibraries = new List<CustomLibrary>()
            {
                new CustomLibrary(){ libraryName = "DefaultWindowsMusicPath",
                    libraryAudioPath = "C:\\Users\\Piotr\\Music",
                    librarySearchOption = SearchOption.TopDirectoryOnly },
            }
        };

        public static Playlist.LibraryPlaylist libraryBulkPlaylist = new Playlist.LibraryPlaylist();

        public static async Task GetAudioFiles()
        {
            AudioContentLoadingDialog audDialog = MusicPanel.instance.AddAudioLoadingDialog();

            //audDialog.Parent = MusicPanel.instance.my

            int fileCount = 0;
            foreach (CustomLibrary s in mainLibrary.customLibraries)
            {
                fileCount += Directory.GetFiles(s.libraryAudioPath).Length;
            }

            //int fileCount = Directory.GetFiles(directoryPath, fileExtension).Length;
            audDialog.InitAudioList(fileCount);

            await Task.Run(() => {

                int index = 0;
                libraryBulkPlaylist.playlistItems.Clear();

                foreach (CustomLibrary s in mainLibrary.customLibraries)
                {
                    foreach (string file in Directory.GetFiles(s.libraryAudioPath, "*.mp3", s.librarySearchOption)
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.wav", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.flac", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.aiff", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.ogg", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.aac", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.wma", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.m4a", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.opus", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.mp1", s.librarySearchOption))
                                 .Union(Directory.GetFiles(s.libraryAudioPath, "*.mp2", s.librarySearchOption)))
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

                            //
                        }
                        catch (Exception ex)
                        {
                            usesmeta = false;

                            if (SettingsManager.LibrarySettings.metaHandling == MetadataHadlingMode.CopyFileName)
                            {
                                title = Path.GetFileNameWithoutExtension(file);
                                artists = "-";
                            }
                            else if (SettingsManager.LibrarySettings.metaHandling == MetadataHadlingMode.TrySeparate)
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

                        s.libraryPlaylist.playlistItems.Add(new AudioDefinition
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

                        try
                        {
                            //MemoryStream ms = new MemoryStream(audioFile.Tag.Pictures[0].Data.Data);
                            //System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            //audioCover = image;

                            MusicPanel.instance.Invoke((MethodInvoker)delegate
                            {
                                //ShowCurrentCover(image);
                                audDialog.UpdateAudioData(new AudioDefinition
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
                            });

                            //MusicPanel.covers.Add(WaveUtils.ResizeImage(image, 6, 6));
                        }
                        catch { }

                        index++;

                        //Console.WriteLine($"Found an MP3 file: {file}");
                    }

                    libraryBulkPlaylist.playlistItems.AddRange(s.libraryPlaylist.playlistItems);
                }

                //titleColumn.ImageAspectName = "audioCover"; // Specify the aspect name for images

                MusicPanel.objlistview.SetObjects(libraryBulkPlaylist.playlistItems);
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

            MusicPanel.objlistview.SmallImageList.ImageSize = new Size(16, 16);

            MusicPanel.objlistview.Refresh();
        }

        public static void ShowCurrentCover(Image img)
        {
            //Random rnd = new Random();
            //img.Save("C:\\Users\\Piotr\\Desktop\\image" + rnd.Next(000000, 999999) + ".png");
        }

        public static AudioDefinition GetDefinitionByIndex(int id)
        {
            return libraryBulkPlaylist.playlistItems[id];
        }

        public static AudioDefinition GetRandomAudioFromMainLibrary()
        {
            resortsearch:
            Random randomIndex = new Random();
            int randomLibrary = randomIndex.Next(0, mainLibrary.customLibraries.Count - 1);

            if (mainLibrary.customLibraries.Count > 0)
            {
                // TODO: ARE ALL LIBRARIES EMPTY CHECK
                AudioDefinition def = mainLibrary.customLibraries[randomLibrary].libraryPlaylist.GetRandomPlaylistItem();
                if (def == null)
                {
                    goto resortsearch;
                }
                else
                {
                    return def;
                }
            }
            else
            {
                return null;
            }
        }

    }

    public class LibraryGroup
    {
        public string groupName { get; set; }
        public List<CustomLibrary> customLibraries { get; set; } = new List<CustomLibrary>();
    }

    public class CustomLibrary
    {
        public string libraryName { get; set; }
        //public List<AudioDefinition> libraryAudio { get; set; }
        public string libraryAudioPath { get; set; }
        public SearchOption librarySearchOption { get; set; } = SearchOption.TopDirectoryOnly;
        public Playlist.LibraryPlaylist libraryPlaylist { get; set; } = new Playlist.LibraryPlaylist();
    }

    public class Playlist
    {
        public class AudioPlaylist
        {

        }

        public class LibraryPlaylist
        {
            public string playlistName { get; set; } = null;
            public CustomLibrary parentLibrary { get; set; }


            public List<AudioDefinition> playlistItems { get; set; } = new List<AudioDefinition>();

            public AudioDefinition GetRandomPlaylistItem()
            {
                if (playlistItems.Count == 0)
                {
                    return null;
                    //throw new InvalidOperationException("The playlist is empty.");
                }

                Random randomIndex = new Random();
                return playlistItems[randomIndex.Next(0, playlistItems.Count - 1)];
            }
        }
    }
}
