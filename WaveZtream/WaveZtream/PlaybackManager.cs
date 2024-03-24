using Accord.Statistics.Kernels;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib.Mpeg;

namespace WaveZtream
{
    internal class PlaybackManager
    {
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

                MusicPanel.instance.InitializeSong((int)audioFileReader.TotalTime.TotalMilliseconds);
                waveOut.Play();
            }
            else
            {
                AudioFileReader audioFileReader = new AudioFileReader(audioFile);
                MusicPanel.instance.InitializeSong((int)audioFileReader.TotalTime.TotalMilliseconds);
                audioHandler.Init(audioFileReader);
                audioHandler.Play();
                
            }
        }

        public static void ChangePosition(long pos)
        {
            positionOffset = pos;
            AudioFileReader audioFileReader = new AudioFileReader(usedAudioDef.audioFilePath);
            long desiredPositionInBytes = (long)(pos * (audioFileReader.WaveFormat.AverageBytesPerSecond / 1000.0));
            audioFileReader.Position = desiredPositionInBytes;
            audioHandler.Stop();
            audioHandler.Init(audioFileReader);
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
    }
}
