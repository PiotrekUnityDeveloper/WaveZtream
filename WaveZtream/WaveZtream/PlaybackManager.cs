﻿using Accord.Statistics.Kernels;
using NAudio.Gui;
using NAudio.Wave;
using System;
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

        public static WaveOutEvent audioHandler = null;
        public static long positionOffset = 0;
        private static AudioDefinition usedAudioDef = null;

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

                waveOut.Init(audioFileReader);
                audioHandler = waveOut;

                bufferedItem = new AudioBufferQueueItem{ key = auddef.audioFileName, audioDefinition = auddef, audioOutput = audioHandler };

                MusicPanel.instance.InitializeSong((int)audioFileReader.TotalTime.TotalMilliseconds);
                waveOut.Play();
            }
            else
            {
                AudioFileReader audioFileReader = new AudioFileReader(audioFile);

                bufferedItem = new AudioBufferQueueItem { key = auddef.audioFileName, audioDefinition = auddef, audioOutput = audioHandler };
                MusicPanel.instance.InitializeSong((int)audioFileReader.TotalTime.TotalMilliseconds);
                
                audioHandler.Init(audioFileReader);
                audioHandler.Play();
                
            }
        }

        public static void PlayNextAudioFromBuffer()
        {
            Console.WriteLine("Playing audio...");

            AudioBufferQueueItem bufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Ready, playingAudioBuffers);

            if (MusicPanel.currentDefinition == null)
            {
                MusicPanel.currentDefinition = bufferItem.audioDefinition;
            }

            positionOffset = 0;

            audioHandler = bufferItem.audioOutput;
            usedAudioDef = bufferedItem.audioDefinition;

            bufferedItem.bufferStatus = AudioBufferStatus.Playing;
            bufferItem.bufferStatus = AudioBufferStatus.Playing;
            //GetFirstMatchingBuffer(AudioBufferStatus.Ready).bufferStatus = AudioBufferStatus.Playing;

            playingAudioBuffers.Remove(bufferItem);
            MusicPanel.currentDefinition = usedAudioDef;

            MusicPanel.instance.InitializeSong(bufferItem.audioLength);

            audioHandler.Play();

            loadedAudioBuffers.Remove(bufferItem);
            Console.WriteLine("Playing a new buffered audio");
        }

        public static void AddAudioToBuffer(AudioDefinition audio)
        {
            // THREADED
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                if (audio.audioSourceType == AudioSourceType.LocalFile)
                {
                    loadedAudioBuffers.Add(new AudioBufferQueueItem
                    {
                        key = audio.audioFileName,
                        audioDefinition = audio,
                        audioOutput = new WaveOutEvent(), // MODIFIED
                        bufferStatus = AudioBufferStatus.Waiting
                    });

                    Console.WriteLine("Added a new localfile audio to the buffer queue");
                }
                
            }).Start();
        }

        public static void LoadNextAudioFromBuffer()
        {
            // THREADED
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                AudioBufferQueueItem finishingBufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Playing, loadedAudioBuffers);
                AudioBufferQueueItem loadedBufferItem = GetFirstMatchingBuffer(AudioBufferStatus.Waiting, loadedAudioBuffers);

                if(loadedBufferItem == null) return;

                if (loadedBufferItem.audioDefinition.audioSourceType == AudioSourceType.LocalFile)
                {
                    AudioFileReader audioFileReader = new AudioFileReader(loadedAudioBuffers[0].audioDefinition.audioFilePath);
                    WaveOutEvent waveOut = new WaveOutEvent();

                    waveOut.Init(audioFileReader);

                    if(audioHandler == null)
                    {
                        Console.WriteLine("Loading the next audio from the buffered queue...");

                        audioBuffersToDispose.Add(finishingBufferItem);
                        //audioHandler = waveOut;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loading;
                        loadedBufferItem.audioOutput = waveOut;
                        loadedBufferItem.audioLength = (int)audioFileReader.TotalTime.TotalMilliseconds;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loaded;
                        bufferedItem = loadedBufferItem;
                        //usedAudioDef = loadedBufferItem.audioDefinition;
                        //loadedAudioBuffers.RemoveAt(0);

                        loadedBufferItem.bufferStatus = AudioBufferStatus.Ready;

                        playingAudioBuffers.Add(loadedBufferItem);
                        //loadedAudioBuffers.Remove(loadedBufferItem);

                        Console.WriteLine("Loaded the next audio from the buffered queue");
                    }
                    else
                    {
                        Console.WriteLine("Loading the next audio from the buffered queue...");

                        WaveOutEvent oldAudioInstance = new WaveOutEvent();
                        oldAudioInstance = audioHandler;

                        audioBuffersToDispose.Add(finishingBufferItem);
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loading;
                        loadedBufferItem.audioOutput = waveOut;
                        loadedBufferItem.audioLength = (int)audioFileReader.TotalTime.TotalMilliseconds;
                        loadedBufferItem.bufferStatus = AudioBufferStatus.Loaded;
                        bufferedItem = loadedBufferItem;
                        //usedAudioDef = loadedBufferItem.audioDefinition;
                        //loadedAudioBuffers.RemoveAt(0); //?

                        AddAudioOutputToDisposal(oldAudioInstance);

                        loadedBufferItem.bufferStatus = AudioBufferStatus.Ready;

                        playingAudioBuffers.Add(loadedBufferItem);
                        //loadedAudioBuffers.Remove(loadedBufferItem);

                        Console.WriteLine("Loaded the next audio from the buffered queue");
                    }

                }
            }).Start();
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

        public static void ChangePosition(long pos)
        {
            positionOffset = pos;
            AudioFileReader audioFileReader = new AudioFileReader(usedAudioDef.audioFilePath);
            //AudioFileReader audioFileReader = new AudioFileReader(bufferedItem.audioDefinition.audioFilePath);
            long desiredPositionInBytes = (long)(pos * (audioFileReader.WaveFormat.AverageBytesPerSecond / 1000.0));
            audioFileReader.Position = desiredPositionInBytes;
            audioHandler.Stop();

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

                audioHandler = new WaveOutEvent();
                audioHandler.Init(audioFileReader);
            }
            catch //(Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                audioHandler = new WaveOutEvent();
                audioHandler.Init(audioFileReader);
            }

            audioHandler.Play();
        }

        public static void StopAudio()
        {
            audioHandler?.Stop();
        }

        public static void PauseAudio()
        {
            audioHandler?.Pause();
        }

        public static void ResumeAudio()
        {
            audioHandler?.Play();
        }

        public static void ReplayAudio()
        {
            positionOffset = 0;
            AudioFileReader audioFileReader = new AudioFileReader(usedAudioDef.audioFilePath);
            long desiredPositionInBytes = (long)(0 * (audioFileReader.WaveFormat.AverageBytesPerSecond / 1000.0));
            audioFileReader.Position = desiredPositionInBytes;
            audioHandler.Stop();
            audioHandler.Init(audioFileReader);
            audioHandler.Play();
        }

        


    }


    public class AudioBufferQueueItem
    {
        public string key;
        public AudioDefinition audioDefinition;
        public WaveOutEvent audioOutput;
        public int audioLength;
        public AudioBufferStatus bufferStatus;
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
