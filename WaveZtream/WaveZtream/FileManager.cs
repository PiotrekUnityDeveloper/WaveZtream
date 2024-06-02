using Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveZtream
{
    internal class FileManager
    {
        public static void LoadLyricsForAudioDefinition(AudioDefinition definition)
        {

        }
    }

    public class Lyrics
    {
        public class StandardLyrics
        {
            public string lyricString;
            public List<LyricEntry> lyricEntries;

            //file specifics
            public string fileExtension;
            // Supported file types:
            // /text formats:                     .txt .text .rtf
            // /lyric-focused formats:            .lrc .kra .audlrc
            // /subtitle-focused formats:         .srt .sub .smi

            public string timestampFormat = "hh:mm:ss:xx";
            //h- hours, m- minutes, s- seconds, x- miliseconds


            private TimeSpan lyricsDuration;

            public StandardLyrics() { InitializeLyrics(); }

            public void InitializeLyrics()
            {

            }
        }

        public class WaveLyrics //aka. AdvancedLyrics/Visualized Lyrics
        {
            //TODO
        }
    }

    public class LyricEntry
    {
        public TimeSpan entryTime { get; set; } 
        public string entryText { get; set; }
    }
}
