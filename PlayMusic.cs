using System;
using NAudio.Wave;

namespace csharp_stdlib
{
    public sealed class PlayMusic : IDisposable
    {
        private readonly WaveOutEvent outputDevice;
        private readonly AudioFileReader audioFile;
        private bool isDisposed;

        public PlayMusic(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Filename cannot be null or empty");

            try
            {
                audioFile = new AudioFileReader(filename);
                outputDevice = new WaveOutEvent();
                outputDevice.Init(audioFile);
            }
            catch (Exception ex) when (ex is System.IO.FileNotFoundException ||
                                     ex is System.IO.DirectoryNotFoundException)
            {
                throw new ArgumentException("Audio file not found", ex);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Could not initialize audio player", ex);
            }
        }

        public void Play()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(PlayMusic));

            outputDevice.Play();
        }

        public void Pause()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(PlayMusic));

            outputDevice.Pause();
        }

        public void Stop()
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(PlayMusic));

            outputDevice.Stop();
            audioFile.Position = 0;
        }

        public void SetVolume(float volume)
        {
            if (isDisposed)
                throw new ObjectDisposedException(nameof(PlayMusic));

            volume = Math.Clamp(volume, 0f, 1f);
            outputDevice.Volume = volume;
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                outputDevice?.Stop();
                audioFile?.Dispose();
                outputDevice?.Dispose();
                isDisposed = true;
            }
        }
    }
}
