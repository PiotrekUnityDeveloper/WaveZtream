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

            // how should the audio be transitioned?
            public static TransitionType transitionTypeIn = TransitionType.Fade;
            public static TransitionType transitionTypeOut = TransitionType.Fade;
            /// None: the audio wont be transitioned and will keep playing
            /// Cutoff: the audio will be cutoff, and just stop playing
            /// Fade: the audio will be faded in/out <summary>
            /// None: the audio wont be transitioned and will keep playing
            /// 
            /// </summary>
             
            public static TransitionTiming transitionTimingOut = TransitionTiming.onPlay;

            // Transition Settings

            public static float customInTime;
            public static float customOutTime;

            public static float transitionInDuration = 2.0f;
            public static float transitionOutDuration = 3.0f;
        }

        public class LibrarySettings
        {
            public static MetadataHadlingMode metaHandling = MetadataHadlingMode.CopyFileName;
        }
    }

    public enum TransitionType
    {
        None,
        Cutoff,
        Fade,
    }

    public enum TransitionTiming
    {
        onLoad,
        onPlay,
        onFinishing,
        CustomTime,
    }
}
