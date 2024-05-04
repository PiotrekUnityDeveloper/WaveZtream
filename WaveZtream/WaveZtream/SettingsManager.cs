using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveZtream
{
    internal class SettingsManager
    {
        public class PlaybackSettings // PLAYBACK
        {
            // how much time before the end should the next song load? (in seconds)
            public static float preLoadDelay = 20.0f;

            // how much time before the end should the next song play? (in seconds)
            public static float prePlayDelay = 11.0f;

            // how many buffers should be loaded in advance to the currently playing one?
            public static int maxBuffersInQueue = 4;
        }

        public class LibrarySettings
        {
            public static MetadataHadlingMode metaHandling = MetadataHadlingMode.CopyFileName;
        }
    }
}
