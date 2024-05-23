using Accord.IO;
using Accord.Statistics.Kernels;
using NAudio.Gui;
using NAudio.Wave;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.Mpeg;

namespace WaveZtream
{
    internal class PlaybackManager
    {
        public static List<AudioBufferQueueItem> loadedAudioBuffers = new List<AudioBufferQueueItem>();
        public static List<AudioBufferQueueItem> playingAudioBuffers = new List<AudioBufferQueueItem>();
        //public static List<AudioDefinition> loadedAudio = new List<AudioDefinition>();
        //public static List<WaveOutEvent> loadedHandlers = new List<WaveOutEvent>();
        // TODO: ^^^^ do this (functions for loading before playing)

        public static List<AudioBufferQueueItem> audioBuffersToDispose = new List<AudioBufferQueueItem>();
        public static List<WaveOutEvent> audioOutputsToDispose = new List<WaveOutEvent>();
        
        public static AudioBufferQueueItem bufferedItem = null;
        public static AudioFileReader audioStreamReader = null;

        public static List<AudioOutput> audioHandlers = new List<AudioOutput>();
        //public static WaveOutEvent audioHandler = null;
        //public static WaveOutEvent oldAudioHandler = null;
        public static long positionOffset = 0;
        private static AudioDefinition usedAudioDef = null;
        public static List<OutputIDf> oldWaveOuts = new List<OutputIDf>();
        /*
        public static void PlayAudio(AudioDefinition auddef)
        {
            if(audioHandler != null && 
                audioHandler.PlaybackState == PlaybackState.Paused &&
                usedAudioDef == auddef)
            {
                audioHandler.Play();
                return;
            }
            else if(audioHandler != null && 
                usedAudioDef != auddef)
            {
                audioHandler.Stop();
                audioHandler.Dispose();
            }
            else if(usedAudioDef == auddef && audioHandler.PlaybackState == PlaybackState.Playing)
            {
                audioHandler.Stop();
            }

            usedAudioDef = auddef;
            positionOffset = 0;


            // Specify the path to your audio file
            string audioFile = auddef.audioFilePath;

            if(audioHandler == null)
            {

                AudioFileReader audioFileReader = new AudioFileReader(audioFile);
                WaveOutEvent waveOut = new WaveOutEvent();

                audioStreamReader = audioFileReader;

                waveOut.Init(audioFileReader);
                audioHandler = waveOut;

                bufferedItem = new AudioBufferQueueItem{ key = auddef.audioFileName, audioDefinition = auddef, audioOutput = audioHandler };

                MusicPanel.instance.InitializeSong((int)audioFileReader.TotalTime.TotalMilliseconds);
                waveOut.Play();
            }
            else
            {
                AudioFileReader audioFileReader = new AudioFileReader(audioFile);

                audioStreamReader = audioFileReader;

                bufferedItem = new AudioBufferQueueItem { key = auddef.audioFileName, audioDefinition = auddef, audioOutput = audioHandler };
                MusicPanel.instance.InitializeSong((int)audioFileReader.TotalTime.TotalMilliseconds);
                
                audioHandler.Init(audioFileReader);
                audioHandler.Play();
                
            }
        }*/

        public static AudioOutput GetLatestOutput()
        {
            return PlaybackManager.audioHandlers[PlaybackManager.audioHandlers.Count - 1];
        }

        public static AudioOutput GetOutputByAudioDefinition(AudioDefinition def)
        {
            /*
            foreach(AudioOutput ao in audioHandlers)
            {
                if(ao.definition == def)
                {
                    return ao;
                }
            }

            return null;
            */

            for (int i = audioHandlers.Count - 1; i >= 0; i--)
            {
                AudioOutput ao = audioHandlers[i];
                if (ao.definition.audioFilePath == def.audioFilePath)
                {
                    return ao;
                }
            }

            return null;

        }

        public static void StartAudioTransition(bool In, AudioDefinition targetdef = null)
        {           

            if (In)
            {
              
                if (SettingsManager.PlaybackSettings.transitionTypeIn == TransitionType.Fade)
                {
                    Console.WriteLine("fading in... " + GetOutputByAudioDefinition(usedAudioDef).definition.audioTitle);
                    AudioOutput ao1 = GetOutputByAudioDefinition(usedAudioDef);
                    AudioTransition.VolumeFadeIn inFadeTransition = new AudioTransition.VolumeFadeIn(/*GetLatestOutput().output*/ao1.output, TimeSpan.FromSeconds(SettingsManager.PlaybackSettings.transitionOutDuration), ao1.reader);
                    inFadeTransition.Start();
                }
            }
            else
            {
                //if (audioHandlers.Count <= 1) { return; }
                
                AudioOutput ao1;
                ao1 = GetOutputByAudioDefinition(targetdef);
                if(ao1 == null) return;

                if (SettingsManager.PlaybackSettings.transitionTypeOut == TransitionType.Fade)
                {
                    Console.WriteLine("fading out... " + ao1.definition.audioTitle);
                    AudioTransition.VolumeFadeOut outFadeTransition = new AudioTransition.VolumeFadeOut(ao1.output, TimeSpan.FromSeconds(SettingsManager.PlaybackSettings.transitionOutDuration), ao1.reader);
                    outFadeTransition.Start();
                    //remove from list?
                    //MusicPanel.instance.RemoveWaveOutputFromList();
                }
            }
        }

        public static OutputIDf GetOutputBySource(string src, bool remove = false)
        {
            OutputIDf removable = null;

            foreach(OutputIDf idf in oldWaveOuts)
            {
                if(idf.source == src)
                {
                    if (remove)
                    {
                        removable = idf;
                        break;
                    }
                    else
                    {
                        return idf;
                    }
                    
                }
            }

            oldWaveOuts.Remove(removable);
            return removable;

            //return null;
        }

        public static void PerformCustomFadeOut(WaveOutEvent outevent, AudioFileReader reader)
        {
            if (SettingsManager.PlaybackSettings.transitionTypeOut == TransitionType.Fade)
            {
                AudioTransition.VolumeFadeOut outFadeTransition = new AudioTransition.VolumeFadeOut(outevent, TimeSpan.FromSeconds(SettingsManager.PlaybackSettings.transitionOutDuration), reader);
                outFadeTransition.Start();
            }
        }

        /*
        public static WaveOutEvent RetrieveOldAudioOutput(WaveOutEvent woeTarget)
        {
            foreach(WaveOutEvent woe in oldWaveOuts)
            {
                if(woe == woeTarget)
                {
                    return woe;
                }
            }

            return null;
        }
        */

        public static void PlayNextAudioFromBuffer()
        {
            Console.WriteLine("Playing audio...");

            //WaveOutEvent oldevent = audioHandler;
            //AudioFileReader afl = new AudioFileReader(usedAudioDef.audioFilePath);
            //oldevent.Init(afl);
            //audioHandler.Stop();
            //oldWaveOuts.Add(oldevent);

            if (SettingsManager.PlaybackSettings.transitionTimingOut == TransitionTiming.onPlay)
            {
                if(audioHandlers.Count > 0)
                {
                    //string outputSrcPath = usedAudioDef.audioFilePath;
                    
                    //StartAudioTransition(false, (AudioDefinition)usedAudioDef.DeepClone());
                    StartAudioTransition(false, usedAudioDef);
                    //PerformCustomFadeOut(oldWaveOuts[0]);
                }
            }

            //oldAudioHandler = bufferedItem.audioOutput;

            AudioBufferQueueItem bufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Ready, playingAudioBuffers);

            if (MusicPanel.currentDefinition == null)
            {
                MusicPanel.currentDefinition = bufferItem.audioDefinition;
            }

            QueueManager.SetBufferStatus(false);

            positionOffset = 0;

            audioStreamReader = new AudioFileReader(bufferItem.audioDefinition.audioFilePath);
            audioHandlers.Add(new AudioOutput { output= bufferItem.audioOutput, definition = bufferItem.audioDefinition, reader = bufferItem.audioFileReader });
            usedAudioDef = bufferedItem.audioDefinition;

            bufferedItem.bufferStatus = AudioBufferStatus.Playing;
            bufferItem.bufferStatus = AudioBufferStatus.Playing;
            //GetFirstMatchingBuffer(AudioBufferStatus.Ready).bufferStatus = AudioBufferStatus.Playing;

            playingAudioBuffers.Remove(bufferItem);
            MusicPanel.currentDefinition = usedAudioDef;

            MusicPanel.instance.InitializeSong(bufferItem.audioLength);

            StartAudioTransition(true);
            audioHandlers[audioHandlers.Count - 1].output.Play();

            loadedAudioBuffers.Remove(bufferItem);
            Console.WriteLine("Playing a new buffered audio");
        }

        public static void AddAudioToBuffer(AudioDefinition audio, AudioBufferQueueItem bufferListType)
        {
            // THREADED
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                if (audio.audioSourceType == AudioSourceType.LocalFile)
                {
                    if(bufferListType.GetType() == typeof(QueuedBuffer))
                    {
                        loadedAudioBuffers.Add(new QueuedBuffer
                        {
                            key = audio.audioFileName,
                            audioDefinition = audio,
                            audioOutput = new WaveOutEvent(), // MODIFIED
                            bufferStatus = AudioBufferStatus.Waiting
                        });
                    }
                    else if (bufferListType.GetType() == typeof(StreakBuffer))
                    {
                        loadedAudioBuffers.Add(new StreakBuffer
                        {
                            key = audio.audioFileName,
                            audioDefinition = audio,
                            audioOutput = new WaveOutEvent(), // MODIFIED
                            bufferStatus = AudioBufferStatus.Waiting
                        });
                    }
                    else if (bufferListType.GetType() == typeof(RequestBuffer))
                    {
                        loadedAudioBuffers.Add(new RequestBuffer
                        {
                            key = audio.audioFileName,
                            audioDefinition = audio,
                            audioOutput = new WaveOutEvent(), // MODIFIED
                            bufferStatus = AudioBufferStatus.Waiting
                        });
                    }

                    Console.WriteLine("Added a new localfile audio to the buffer queue");

                    //QueueManager.SortQueuedBuffers();
                }
                
            }).Start();
        }

        private BlockingCollection<Action> _actions = new BlockingCollection<Action>();

        public static void LoadNextAudioFromBuffer(bool autoPlay = false)
        {
            // THREADED
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                AudioBufferQueueItem finishingBufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Playing, playingAudioBuffers);
                AudioBufferQueueItem loadedBufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Waiting, loadedAudioBuffers);

                if(loadedBufferItem == null) return;

                if (loadedBufferItem.audioDefinition.audioSourceType == AudioSourceType.LocalFile)
                {
                    AudioFileReader audioFileReader = new AudioFileReader(loadedAudioBuffers[0].audioDefinition.audioFilePath);
                    WaveOutEvent waveOut = new WaveOutEvent();

                    //audioStreamReader = audioFileReader;

                    waveOut.Init(audioFileReader);

                    //oldWaveOuts.Add(new OutputIDf { waveOutput = waveOut, source = loadedBufferItem.audioDefinition.audioFilePath });
                    //MusicPanel.instance.AddWaveOutputToList(loadedAudioBuffers[0].audioDefinition.audioFilePath);

                    if (audioHandlers.Count > 0)
                    {
                        Console.WriteLine("Loading the next audio from the buffered queue...");

                        //finishingBufferItem.bufferStatus = AudioBufferStatus.Finishing;
                        //audioBuffersToDispose.Add(finishingBufferItem);

                        //audioHandler = waveOut;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loading;
                        loadedBufferItem.audioOutput = waveOut;
                        loadedBufferItem.audioLength = (int)audioFileReader.TotalTime.TotalMilliseconds;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loaded;
                        loadedBufferItem.audioFileReader = audioFileReader;
                        bufferedItem = loadedBufferItem;
                        //usedAudioDef = loadedBufferItem.audioDefinition;
                        //loadedAudioBuffers.RemoveAt(0);

                        loadedBufferItem.bufferStatus = AudioBufferStatus.Ready;

                        playingAudioBuffers.Add(loadedBufferItem);
                        //loadedAudioBuffers.Remove(loadedBufferItem);

                        Console.WriteLine("Loaded the next audio from the buffered queue");

                        if (autoPlay)
                        {
                            //PlaybackManager.PlayNextAudioFromBuffer();

                            /*
                             System.Threading.SynchronizationContext.Current.Post(_ =>
                             {
                                 PlaybackManager.PlayNextAudioFromBuffer();
                             }, null);*/

                            
                        }

                        return;
                    }
                    else
                    {
                        Console.WriteLine("Loading the next audio from the buffered queue...");

                        //WaveOutEvent oldAudioInstance = new WaveOutEvent();
                        //oldAudioInstance = audioHandlers;

                        //finishingBufferItem.bufferStatus = AudioBufferStatus.Finishing; needed?
                        audioBuffersToDispose.Add(finishingBufferItem);
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loading;
                        loadedBufferItem.audioOutput = waveOut;
                        loadedBufferItem.audioLength = (int)audioFileReader.TotalTime.TotalMilliseconds;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loaded;
                        loadedBufferItem.audioFileReader = audioFileReader;
                        bufferedItem = loadedBufferItem;
                        //usedAudioDef = loadedBufferItem.audioDefinition;
                        //loadedAudioBuffers.RemoveAt(0); //?

                        //AddAudioOutputToDisposal(oldAudioInstance);

                        loadedBufferItem.bufferStatus = AudioBufferStatus.Ready;

                        playingAudioBuffers.Add(loadedBufferItem);
                        //loadedAudioBuffers.Remove(loadedBufferItem);

                        Console.WriteLine("Loaded the next audio from the buffered queue");

                        if (autoPlay)
                        {
                            //PlaybackManager.PlayNextAudioFromBuffer();

                            /*
                            System.Threading.SynchronizationContext.Current.Post(_ =>
                            {
                                PlaybackManager.PlayNextAudioFromBuffer();
                            }, null);*/
                        }

                        return;
                    }

                }

            }).Start();
        }

        public static async void AutoPlayNextAudioFromBuffer()
        {
            // THREADED
            await Task.Run(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                AudioBufferQueueItem finishingBufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Playing, playingAudioBuffers);
                AudioBufferQueueItem loadedBufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Waiting, loadedAudioBuffers);

                if (loadedBufferItem == null) return;

                if (loadedBufferItem.audioDefinition.audioSourceType == AudioSourceType.LocalFile)
                {
                    AudioFileReader audioFileReader = new AudioFileReader(loadedAudioBuffers[0].audioDefinition.audioFilePath);
                    WaveOutEvent waveOut = new WaveOutEvent();

                    //audioStreamReader = audioFileReader;

                    waveOut.Init(audioFileReader);

                    oldWaveOuts.Add(new OutputIDf { waveOutput = waveOut, source = loadedBufferItem.audioDefinition.audioFilePath });
                    //MusicPanel.instance.AddWaveOutputToList(loadedAudioBuffers[0].audioDefinition.audioFilePath);

                    if (audioHandlers.Count > 0)
                    {
                        Console.WriteLine("Loading the next audio from the buffered queue...");

                        audioBuffersToDispose.Add(finishingBufferItem);
                        //audioHandler = waveOut;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loading;
                        loadedBufferItem.audioOutput = waveOut;
                        loadedBufferItem.audioLength = (int)audioFileReader.TotalTime.TotalMilliseconds;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loaded;
                        loadedBufferItem.audioFileReader = audioFileReader;
                        bufferedItem = loadedBufferItem;
                        //usedAudioDef = loadedBufferItem.audioDefinition;
                        //loadedAudioBuffers.RemoveAt(0);

                        loadedBufferItem.bufferStatus = AudioBufferStatus.Ready;

                        playingAudioBuffers.Add(loadedBufferItem);
                        //loadedAudioBuffers.Remove(loadedBufferItem);

                        Console.WriteLine("Loaded the next audio from the buffered queue");

                        MusicPanel.instance.Invoke((MethodInvoker)delegate
                        {
                            PlaybackManager.PlayNextAudioFromBuffer();
                        });

                        return;
                    }
                    else
                    {
                        Console.WriteLine("Loading the next audio from the buffered queue...");

                        //WaveOutEvent oldAudioInstance = new WaveOutEvent();
                        //oldAudioInstance = audioHandler;

                        audioBuffersToDispose.Add(finishingBufferItem);
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loading;
                        loadedBufferItem.audioOutput = waveOut;
                        loadedBufferItem.audioLength = (int)audioFileReader.TotalTime.TotalMilliseconds;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loaded;
                        loadedBufferItem.audioFileReader = audioFileReader;
                        bufferedItem = loadedBufferItem;
                        //usedAudioDef = loadedBufferItem.audioDefinition;
                        //loadedAudioBuffers.RemoveAt(0); //?

                        //AddAudioOutputToDisposal(oldAudioInstance);

                        loadedBufferItem.bufferStatus = AudioBufferStatus.Ready;

                        playingAudioBuffers.Add(loadedBufferItem);
                        //loadedAudioBuffers.Remove(loadedBufferItem);

                        Console.WriteLine("Loaded the next audio from the buffered queue");

                        MusicPanel.instance.Invoke((MethodInvoker)delegate
                        {
                            PlaybackManager.PlayNextAudioFromBuffer();
                        });

                        return;
                    }

                }

            });
        }

        public void Invoke(Action action)
        {
            _actions.Add(action);
        }

        public static bool IsWaitingAudioBufferEmpty()
        {
            if(GetFirstMatchingBuffer(AudioBufferStatus.Waiting, loadedAudioBuffers) == null)
            {
                return true;
            } else { return false; }
        }

        public static void AddAudioOutputToDisposal(WaveOutEvent audioOutput)
        {
            audioOutputsToDispose.Add(audioOutput);
            audioOutput.PlaybackStopped += AudioOutput_PlaybackStopped;
        }

        private static void AudioOutput_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            //throw new NotImplementedException();

            if(e.Exception == null)
            {
                audioOutputsToDispose.Remove((WaveOutEvent)sender);
                ((WaveOutEvent)(sender)).Stop();
                ((WaveOutEvent)(sender)).Dispose();
            }
        }

        public static AudioBufferQueueItem GetFirstMatchingBuffer(AudioBufferStatus targetStatus, List<AudioBufferQueueItem> bufferList)
        {
            foreach (AudioBufferQueueItem audbuffer in loadedAudioBuffers)
            {
                if(audbuffer.bufferStatus == targetStatus)
                {
                    return audbuffer;
                }
            }

            return null;
        }

        public static bool IsThereAReadyAudioBuffer() // TODO: FIX
        {
            AudioBufferQueueItem abqi = GetFirstMatchingBuffer(AudioBufferStatus.Ready, playingAudioBuffers);
            if(abqi != null) return true; else return false;
        }

        public static int GetTotalReadyBuffers(List<AudioBufferQueueItem> bufferList)
        {
            int readyCount = 0;
            foreach (AudioBufferQueueItem audbuffer in loadedAudioBuffers)
            {
                if (audbuffer.bufferStatus == AudioBufferStatus.Ready)
                {
                    readyCount++;
                }
            }

            return readyCount;
        }

        public static void ChangePosition(long pos)
        {
            positionOffset = pos;
            AudioFileReader audioFileReader = new AudioFileReader(usedAudioDef.audioFilePath);
            audioStreamReader = audioFileReader;
            //AudioFileReader audioFileReader = new AudioFileReader(bufferedItem.audioDefinition.audioFilePath);
            long desiredPositionInBytes = (long)(pos * (audioFileReader.WaveFormat.AverageBytesPerSecond / 1000.0));
            audioFileReader.Position = desiredPositionInBytes;
            audioHandlers[audioHandlers.Count - 1].output.Stop();

            try
            {
                /*
                if(audioHandler != null && audioFileReader != null && audioFileReader.FileName != null)
                {
                    audioHandler.Init(audioFileReader);
                }
                else
                {
                    audioHandler = new WaveOutEvent();
                    audioHandler.Init(audioFileReader);
                }*/

                audioHandlers[audioHandlers.Count - 1].output.Dispose();
                audioHandlers[audioHandlers.Count - 1].output = new WaveOutEvent();
                audioHandlers[audioHandlers.Count - 1].output.Init(audioFileReader);
                GetLatestOutput().reader = audioFileReader;
            }
            catch //(Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                audioHandlers[audioHandlers.Count - 1].output.Dispose();
                audioHandlers[audioHandlers.Count - 1].output = new WaveOutEvent();
                audioHandlers[audioHandlers.Count - 1].output.Init(audioFileReader);
                GetLatestOutput().reader = audioFileReader;
            }

            audioHandlers[audioHandlers.Count - 1].output.Play();
        }

        public static void StopAudio()
        {
            audioHandlers[audioHandlers.Count - 1].output?.Stop();
        }

        public static void PauseAudio()
        {
            audioHandlers[audioHandlers.Count - 1].output?.Pause();
        }

        public static void ResumeAudio()
        {
            audioHandlers[audioHandlers.Count - 1].output?.Play();
        }

        public static void ReplayAudio()
        {
            positionOffset = 0;
            AudioFileReader audioFileReader = new AudioFileReader(usedAudioDef.audioFilePath);
            long desiredPositionInBytes = (long)(0 * (audioFileReader.WaveFormat.AverageBytesPerSecond / 1000.0));
            audioFileReader.Position = desiredPositionInBytes;
            audioHandlers[audioHandlers.Count - 1].output.Stop();
            audioHandlers[audioHandlers.Count - 1].output.Dispose();
            audioHandlers[audioHandlers.Count - 1].output = new WaveOutEvent();
            audioHandlers[audioHandlers.Count - 1].output.Init(audioFileReader);
            audioHandlers[audioHandlers.Count - 1].output.Play();
            GetLatestOutput().reader = audioFileReader;
        }

        


    }

    public class OutputIDf
    {
        public WaveOutEvent waveOutput;
        public string source;
    }

    public class AudioBufferQueueItem
    {
        public string key;
        public AudioDefinition audioDefinition;
        public AudioFileReader audioFileReader;
        public WaveOutEvent audioOutput;
        public int audioLength;
        public AudioBufferStatus bufferStatus;
    }

    public class QueuedBuffer : AudioBufferQueueItem // the audio from the queue type of buffer
    {
    }// for identification

    public class StreakBuffer : AudioBufferQueueItem // the random audio LibraryManager returns when the queue is empty,
                                                    // and the infinite play is on
    {
    }// for indentification

    public class RequestBuffer : AudioBufferQueueItem // this buffer type is used when, the audio is requested on demand.
                                                     //  it means, when there is a queue and/or a streakqueue, they wont be affected
                                                     //  and the audio will play independently of it.
                                                     //  (NOTE: there should be only one of this type at a time in a queue buffer)
    {
    }// for indentification

    public class AudioOutput
    {
        public AudioDefinition definition;
        public AudioFileReader reader;
        public WaveOutEvent output;
    }

    public enum AudioBufferStatus
    {
        Waiting,  // The audio was just added to the buffer queue
        Loading,  // The audio is being loaded
        Loaded,   // The audio has been loaded
        Ready,    // The audio will play any second now
        Playing,  // The audio is playing
        Finishing,// The audio is 'fading out' and another one (if enabled) is being played
        Unloading,// The audio completely ended, disposing it to free the memory
    }
}
