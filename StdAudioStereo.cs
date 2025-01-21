using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;
using NAudio.Wave.SampleProviders;

namespace csharp_stdlib
{
    public static class StdAudioStereo
    {
        public const int SAMPLE_RATE = 44100;
        private const int BYTES_PER_SAMPLE = 2;
        private const int BITS_PER_SAMPLE = 16;
        private const int MAX_16_BIT = 32768;
        private const int BYTES_PER_FRAME = 4;
        private const int SAMPLE_BUFFER_SIZE = 4096;

        private static SoundPlayer player;
        private static byte[] buffer;
        private static int bufferSize = 0;

        private static List<Thread> backgroundThreads = new List<Thread>();
        private static bool isRecording = false;
        private static Queue<double> recordedSamplesLeft = new Queue<double>();
        private static Queue<double> recordedSamplesRight = new Queue<double>();

        static StdAudioStereo()
        {
            Init();
        }

        private static void Init()
        {
            buffer = new byte[SAMPLE_BUFFER_SIZE * BYTES_PER_SAMPLE / 2];
            player = new SoundPlayer();
        }

        public static void Play(double sampleLeft, double sampleRight)
        {
            if (double.IsNaN(sampleLeft)) throw new ArgumentException("sampleLeft is NaN");
            if (double.IsNaN(sampleRight)) throw new ArgumentException("sampleRight is NaN");

            sampleLeft = Math.Clamp(sampleLeft, -1.0, 1.0);
            sampleRight = Math.Clamp(sampleRight, -1.0, 1.0);

            if (isRecording)
            {
                recordedSamplesLeft.Enqueue(sampleLeft);
                recordedSamplesRight.Enqueue(sampleRight);
            }

            short sLeft = (short)(MAX_16_BIT * sampleLeft);
            if (sampleLeft == 1.0) sLeft = short.MaxValue;
            buffer[bufferSize++] = (byte)sLeft;
            buffer[bufferSize++] = (byte)(sLeft >> 8);

            short sRight = (short)(MAX_16_BIT * sampleRight);
            if (sampleRight == 1.0) sRight = short.MaxValue;
            buffer[bufferSize++] = (byte)sRight;
            buffer[bufferSize++] = (byte)(sRight >> 8);

            if (bufferSize >= buffer.Length)
            {
                using (var ms = new MemoryStream(buffer))
                {
                    player.Stream = ms;
                    player.PlaySync();
                }
                bufferSize = 0;
            }
        }

        public static void Play(double sample) => Play(sample, sample);

        public static void Play(double[] samples)
        {
            if (samples == null) throw new ArgumentNullException(nameof(samples));
            foreach (var sample in samples) Play(sample);
        }

        public static void Play(double[] samplesLeft, double[] samplesRight)
        {
            if (samplesLeft == null || samplesRight == null || samplesLeft.Length != samplesRight.Length)
                throw new ArgumentException("Invalid input arrays");

            for (int i = 0; i < samplesLeft.Length; i++)
                Play(samplesLeft[i], samplesRight[i]);
        }

        public static void Play(string filename)
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentException("Filename cannot be null or empty");
            
            if (isRecording)
            {
                var left = ReadLeft(filename);
                var right = ReadRight(filename);
                foreach (var sample in left) recordedSamplesLeft.Enqueue(sample);
                foreach (var sample in right) recordedSamplesRight.Enqueue(sample);
            }

            player.SoundLocation = filename;
            player.PlaySync();
        }

        public static void Drain()
        {
            if (bufferSize > 0)
            {
                using (var ms = new MemoryStream(buffer, 0, bufferSize))
                {
                    player.Stream = ms;
                    player.PlaySync();
                }
                bufferSize = 0;
            }
        }

        public static void StartRecording()
        {
            if (isRecording) throw new InvalidOperationException("Recording already in progress");
            isRecording = true;
            recordedSamplesLeft.Clear();
            recordedSamplesRight.Clear();
        }

        public static void StopRecording()
        {
            if (!isRecording) throw new InvalidOperationException("No recording in progress");
            isRecording = false;
        }

        public static double[] GetRecordingLeft()
        {
            if (isRecording) throw new InvalidOperationException("Recording still in progress");
            return recordedSamplesLeft.ToArray();
        }

        public static double[] GetRecordingRight()
        {
            if (isRecording) throw new InvalidOperationException("Recording still in progress");
            return recordedSamplesRight.ToArray();
        }

        public static double[] GetRecordingMono()
        {
            var left = GetRecordingLeft();
            var right = GetRecordingRight();
            var result = new double[left.Length];
            for (int i = 0; i < left.Length; i++)
                result[i] = (left[i] + right[i]) / 2.0;
            return result;
        }

        public static double[] ReadLeft(string filename) => ReadChannel(filename, true);
        public static double[] ReadRight(string filename) => ReadChannel(filename, false);

        private static double[] ReadChannel(string filename, bool isLeftChannel)
        {
            using var reader = new NAudio.Wave.WaveFileReader(filename);
            if (reader.WaveFormat.SampleRate != SAMPLE_RATE)
                throw new ArgumentException($"Sample rate must be {SAMPLE_RATE}");
            if (reader.WaveFormat.BitsPerSample != BITS_PER_SAMPLE)
                throw new ArgumentException($"Bits per sample must be {BITS_PER_SAMPLE}");
            if (reader.WaveFormat.Channels != 2)
                throw new ArgumentException("Audio must be stereo");

            var samples = new double[reader.SampleCount];
            var buffer = new byte[reader.SampleCount * BYTES_PER_FRAME];
            int bytesRead = reader.Read(buffer, 0, buffer.Length);

            for (int i = 0; i < samples.Length; i++)
            {
                int offset = i * BYTES_PER_FRAME;
                short left = BitConverter.ToInt16(buffer, offset);
                short right = BitConverter.ToInt16(buffer, offset + 2);
                
                samples[i] = isLeftChannel ? 
                    left / (double)MAX_16_BIT : 
                    right / (double)MAX_16_BIT;
            }

            return samples;
        }

        public static void Save(string filename, double[] samples)
        {
            Save(filename, samples, samples);
        }

        public static void Save(string filename, double[] samplesLeft, double[] samplesRight)
        {
            if (samplesLeft.Length != samplesRight.Length)
                throw new ArgumentException("Left and right channels must have same length");

            var format = new NAudio.Wave.WaveFormat(SAMPLE_RATE, BITS_PER_SAMPLE, 2);
            using var writer = new NAudio.Wave.WaveFileWriter(filename, format);

            var buffer = new byte[samplesLeft.Length * BYTES_PER_FRAME];
            for (int i = 0; i < samplesLeft.Length; i++)
            {
                short left = (short)(Math.Clamp(samplesLeft[i], -1.0, 1.0) * MAX_16_BIT);
                short right = (short)(Math.Clamp(samplesRight[i], -1.0, 1.0) * MAX_16_BIT);

                buffer[i * 4] = (byte)(left & 0xFF);
                buffer[i * 4 + 1] = (byte)(left >> 8);
                buffer[i * 4 + 2] = (byte)(right & 0xFF);
                buffer[i * 4 + 3] = (byte)(right >> 8);
            }

            writer.Write(buffer, 0, buffer.Length);
        }

        public static void PlayInBackground(string filename)
        {
            var thread = new Thread(() => Play(filename));
            backgroundThreads.Add(thread);
            thread.Start();
        }

        public static void StopInBackground()
        {
            foreach (var thread in backgroundThreads)
            {
                if (thread.IsAlive) thread.Abort();
            }
            backgroundThreads.Clear();
        }
    }
}
