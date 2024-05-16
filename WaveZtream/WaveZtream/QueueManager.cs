using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveZtream
{
    internal class QueueManager
    {
        public static AudioQueue mainQueue = new AudioQueue() { queueName = "main0" };

        public static void AddAudioToQueue(AudioQueue queue, AudioDefinition audio)
        {
            queue.queueItems.Add(new AudioQueueItem { queueItemType = AudioQueueItemType.Audio, 
                audioDefinition = audio,
                queueIndex = queue.queueItems.Count - 1 });

            PlaybackManager.AddAudioToBuffer(audio, new QueuedBuffer());
            SortQueuedBuffers();
        }

        public static void SortQueuedBuffers()
        {
            List<QueuedBuffer> queuedBuffers = new List<QueuedBuffer>();
            List<StreakBuffer> streakBuffers = new List<StreakBuffer>();
            List<RequestBuffer> requestBuffers = new List<RequestBuffer>();

            foreach (AudioBufferQueueItem aitem in PlaybackManager.loadedAudioBuffers)
            {
                if(aitem is QueuedBuffer qbuffer)
                {
                    queuedBuffers.Add(qbuffer);
                }
                else if(aitem is StreakBuffer sbuffer)
                {
                    streakBuffers.Add(sbuffer);
                }
                else if (aitem is RequestBuffer rbuffer)
                {
                    requestBuffers.Add(rbuffer);
                }
            }

            //List<AudioBufferQueueItem> sortedBuffers = new List<AudioBufferQueueItem>();
            //sortedBuffers.AddRange(queuedBuffers);
            //sortedBuffers.AddRange(streakBuffers);

            PlaybackManager.loadedAudioBuffers.Clear();
            //PlaybackManager.loadedAudioBuffers = sortedBuffers;
            PlaybackManager.loadedAudioBuffers.AddRange(requestBuffers);
            PlaybackManager.loadedAudioBuffers.AddRange(queuedBuffers);
            PlaybackManager.loadedAudioBuffers.AddRange(streakBuffers);
        }

        public static AudioDefinition GetNextSong()
        {
            if(mainQueue.queueItems.Count <= 0)
            {
                return GetRandomSongFromMainLibrary();
            }

            return GetNextSongFromQueue();
        }

        public static AudioDefinition GetNextSongFromQueue()
        {
            if (mainQueue.queueItems.Count > 0)
            {
                AudioDefinition def0 = mainQueue.queueItems[0].audioDefinition;
                mainQueue.queueItems.RemoveAt(0);
                return def0;
            }

            return null;
        }

        public static AudioDefinition GetRandomSongFromMainLibrary()
        {
            return LibraryManager.GetRandomAudioFromMainLibrary();
        }

        public static AudioOutput GetLatestOutput()
        {
            return PlaybackManager.audioHandlers[PlaybackManager.audioHandlers.Count - 1];
        }

        // PRELOADING BUFFERS

        private static bool buffersQueued = false;

        public static void CheckForNextSongPreLoad()
        {
            //Console.WriteLine("Checking for a preload...");

            if (GetLatestOutput().output.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
                return;

            if ((PlaybackManager.audioStreamReader.TotalTime.TotalMilliseconds -
                (GetLatestOutput().output.GetPositionTimeSpan().TotalMilliseconds + PlaybackManager.positionOffset)) <= SettingsManager.PlaybackSettings.preLoadDelay * 1000)
            {
                if(buffersQueued) return;

                Console.WriteLine("Preloading buffered audio...");

                int readyBuffers = PlaybackManager.GetTotalReadyBuffers(PlaybackManager.loadedAudioBuffers);

                if (readyBuffers < SettingsManager.PlaybackSettings.maxBuffersInQueue)
                {
                    for (int i = 0; i < (SettingsManager.PlaybackSettings.maxBuffersInQueue - readyBuffers); i++)
                    {
                        Console.WriteLine("Preloading buffered audio...  " + (i + 1) +"/"+ 
                            SettingsManager.PlaybackSettings.maxBuffersInQueue);

                        if (!PlaybackManager.IsWaitingAudioBufferEmpty())
                        {
                            PlaybackManager.LoadNextAudioFromBuffer();
                        }
                        else { /*TODO: GET RANDOM SONG OR FROM QUEUE*/
                            PlaybackManager.AddAudioToBuffer(GetNextSong(), new StreakBuffer());
                            PlaybackManager.LoadNextAudioFromBuffer();
                        }

                        //await Task.Delay(1000);
                    }

                    buffersQueued = true;
                }
            }
        }

        public static void CheckForNextSongPrePlay()
        {
            //Console.WriteLine("Loading preplay...");

            if (GetLatestOutput().output.PlaybackState == NAudio.Wave.PlaybackState.Stopped)
                return;

            if ((PlaybackManager.audioStreamReader.TotalTime.TotalMilliseconds -
                (GetLatestOutput().output.GetPositionTimeSpan().TotalMilliseconds + PlaybackManager.positionOffset)) <= SettingsManager.PlaybackSettings.prePlayDelay * 1000)
            {
                Console.WriteLine("Loading preplay...");

                if (PlaybackManager.IsThereAReadyAudioBuffer())
                {
                    Console.WriteLine("Preplay loaded...!");
                    PlaybackManager.PlayNextAudioFromBuffer();
                    buffersQueued = false;
                }
            }
        }

        public static void SetBufferStatus(bool status)
        {
            buffersQueued = status;
        }
    }

    public class AudioQueue
    {
        public string queueName { get; set; }
        public List<AudioQueueItem> queueItems = new List<AudioQueueItem>();
    }

    public class AudioQueueItem
    {
        public AudioQueueItemType queueItemType;
        public int queueIndex = 0;

        // script specific
        public string scriptLocation = "";

        // sleep specific
        public float sleepTime = 0; // sleep delay
        public SleepQueueAction sleepAction;
        // lets you set a timer that will stop the music after a certain amount of time

        // break specific
        public float breakTime = 0; // break length
        // break queue item
        // lets you have a little break from the music, throughout your long queue

        // timer specific
        public float timerTime = 0; // timer length
        public bool tryToFit = false; // tries to pick songs that are the closest to fit the timespan
        public bool forceCut; // cuts the currently played song, exactly when the time ends
        // timer queue item
        // lets you set a time in which the music will play

        //TODO: SwitchSource Properties

        //TODO: PlayRandom Properties

        //TODO: RunScript Properties


        public AudioDefinition audioDefinition { get; set; } = null;
    }
}

public enum AudioQueueItemType
{
    //audio
    Audio,
    //actions
    Break,
    Stop,
    Sleep,
    Timer,
    SwitchSource,
    PlayRandom,
    RunScript,
}

public enum SleepQueueAction
{
    Stop,
    Pause,
    PlaySoundAndStop,
    SleepSystem,
    HibernateSystem,
    LogoffSystem,
    RestartSystem,
    ShutdownSystem,
    RunScript,
    StopAndRunScript,
}
