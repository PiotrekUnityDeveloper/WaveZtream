﻿using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WaveZtream
{
    internal class AudioTransition
    {
        private static List<VolumeFadeIn> activeFadeIns = new List<VolumeFadeIn>();

        public class VolumeFadeOut
        {
            private readonly WaveOutEvent waveOut;
            private readonly AudioFileReader waveReader;
            private readonly float initialVolume;
            private readonly float targetVolume;
            private readonly TimeSpan duration;
            private readonly int fadeSteps;
            private const float VolumeThreshold = 0.01f;

            public VolumeFadeOut(WaveOutEvent waveOut, TimeSpan duration, AudioFileReader reader)
            {
                this.waveOut = waveOut;
                this.duration = duration;
                this.waveReader = reader;
                this.initialVolume = reader.Volume;
                this.targetVolume = 0.0f;
                this.fadeSteps = 100; // Adjust as needed for smoother or quicker fade
            }

            public async void Start()
            {
                await Task.Run(() =>
                {
                    float stepSize = (initialVolume - targetVolume) / fadeSteps;
                    TimeSpan stepDuration = TimeSpan.FromTicks(duration.Ticks / fadeSteps);

                    for (int i = 0; i < fadeSteps; i++)
                    {
                        if (Math.Abs(waveReader.Volume - targetVolume) < VolumeThreshold)
                        //if ((waveReader.Volume - stepSize) <= 0)
                        {
                            //waveReader.Volume = targetVolume;
                            break;
                        }
                        else
                        {
                            //waveOut.Volume -= stepSize;
                            //waveReader.Volume -= stepSize;
                            waveReader.Volume = 1.0f - (stepSize * i);
                            //Console.WriteLine("fading out the song... vol: " + waveReader.Volume);
                        }

                        //Console.WriteLine("vol: " + waveOut.Volume + " nextvol: " + (waveOut.Volume - stepSize));

                        Thread.Sleep(stepDuration);
                    }

                    // Ensure the volume reaches the target volume exactly
                    //waveOut.Volume = targetVolume;
                    waveReader.Volume = targetVolume;

                    // Stop the playback after fade out
                    waveOut.Stop();
                    waveOut.Dispose();
                    this.waveReader.Dispose();
                });
            }
        }

        public class VolumeFadeIn
        {
            public readonly WaveOutEvent waveOut;
            private readonly AudioFileReader waveReader;
            private readonly float initialVolume;
            private readonly float targetVolume;
            private readonly TimeSpan duration;
            private readonly int fadeSteps;
            private const float VolumeThreshold = 0.01f; // Adjust as needed
            
            public bool isRunning = false;

            private CancellationTokenSource cancellationTokenSource;
            private CancellationToken cancellationToken;

            public VolumeFadeIn(WaveOutEvent waveOut, TimeSpan duration, AudioFileReader reader)
            {
                this.waveOut = waveOut;
                this.duration = duration;
                this.waveReader = reader;
                this.initialVolume = 0.0f;
                this.targetVolume = 1.0f;
                this.fadeSteps = 100; // Adjust as needed for smoother or quicker fade

                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;
            }

            public async void Start()
            {
                await Task.Run(() =>
                {
                    AudioTransition.activeFadeIns.Add(this);
                    isRunning = true;

                    float stepSize = (targetVolume - initialVolume) / fadeSteps;
                    TimeSpan stepDuration = TimeSpan.FromTicks(duration.Ticks / fadeSteps);

                    
                    //waveOut.Volume = initialVolume;
                    waveReader.Volume = initialVolume;
                    waveOut.Play();

                    for (int i = 0; i < fadeSteps; i++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            break;
                        }

                        if (Math.Abs(waveReader.Volume - targetVolume) < VolumeThreshold)
                        {
                            break;
                        }
                        else
                        {
                            waveReader.Volume += stepSize;
                        }

                        Thread.Sleep(stepDuration);
                    }


                    if (!cancellationToken.IsCancellationRequested)
                    {
                        // Ensure the volume reaches the target volume exactly
                        waveReader.Volume = targetVolume;
                    }

                    isRunning = false;
                    AudioTransition.activeFadeIns.Remove(this);
                });
            }

            public void CancelFadeIn()
            {
                cancellationTokenSource.Cancel();
            }
        }

        public static bool isVolumeTransitionInRunning(WaveOutEvent id, out VolumeFadeIn targetOutput)
        {
            foreach(VolumeFadeIn vfi in activeFadeIns)
            {
                if(vfi.waveOut == id)
                {
                    targetOutput = vfi;
                    return true;
                }
            }

            targetOutput = null;
            return false;
        }
    }
}
