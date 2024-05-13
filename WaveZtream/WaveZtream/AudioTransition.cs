using NAudio.Wave;
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
        public class VolumeFadeOut
        {
            private readonly WaveOutEvent waveOut;
            private readonly float initialVolume;
            private readonly float targetVolume;
            private readonly TimeSpan duration;
            private readonly int fadeSteps;
            private const float VolumeThreshold = 0.01f;

            public VolumeFadeOut(WaveOutEvent waveOut, TimeSpan duration)
            {
                this.waveOut = waveOut;
                this.duration = duration;
                this.initialVolume = 1.0f;
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
                        if (Math.Abs(waveOut.Volume - targetVolume) < VolumeThreshold)
                        {
                            break;
                        }
                        else
                        {
                            waveOut.Volume -= stepSize;
                        }

                        //Console.WriteLine("vol: " + waveOut.Volume + " nextvol: " + (waveOut.Volume - stepSize));

                        Thread.Sleep(stepDuration);
                    }

                    // Ensure the volume reaches the target volume exactly
                    waveOut.Volume = targetVolume;

                    // Stop the playback after fade out
                    waveOut.Stop();
                });
            }
        }

        public class VolumeFadeIn
        {
            private readonly WaveOutEvent waveOut;
            private readonly float initialVolume;
            private readonly float targetVolume;
            private readonly TimeSpan duration;
            private readonly int fadeSteps;
            private const float VolumeThreshold = 0.01f; // Adjust as needed

            public VolumeFadeIn(WaveOutEvent waveOut, TimeSpan duration)
            {
                this.waveOut = waveOut;
                this.duration = duration;
                this.initialVolume = 0.0f;
                this.targetVolume = 1.0f;
                this.fadeSteps = 100; // Adjust as needed for smoother or quicker fade
            }

            public async void Start()
            {
                await Task.Run(() =>
                {
                    float stepSize = (targetVolume - initialVolume) / fadeSteps;
                    TimeSpan stepDuration = TimeSpan.FromTicks(duration.Ticks / fadeSteps);

                    waveOut.Volume = initialVolume;
                    waveOut.Play();

                    for (int i = 0; i < fadeSteps; i++)
                    {
                        if (Math.Abs(waveOut.Volume - targetVolume) < VolumeThreshold)
                        {
                            break;
                        }
                        else
                        {
                            waveOut.Volume += stepSize;
                        }

                        Thread.Sleep(stepDuration);
                    }

                    // Ensure the volume reaches the target volume exactly
                    waveOut.Volume = targetVolume;
                });
            }
        }
    }
}
