using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaveZtream
{
    internal class QueueManager
    {
        public List<AudioQueueItem> mainQueueItems = new List<AudioQueueItem>();
    }

    public class AudioQueueItem
    {
        public AudioQueueItemType queueItemType;

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
