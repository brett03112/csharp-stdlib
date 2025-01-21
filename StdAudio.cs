using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace csharp_stdlib
{
    public static class StdAudio
    {
        public const int SAMPLE_RATE = 44100;
        private const int MAX_16_BIT = 32768;
        
        private static Queue<double>? recordedSamples = null;
        private static bool isRecording = false;
        private static readonly List<Thread> backgroundThreads = new();
        
        // Audio playback buffer
        private static readonly byte[] buffer = new byte[4096 * 2 / 3]; // 16-bit samples
        private static int bufferSize = 0;
        private static readonly SoundPlayer player = new();
        
        /// <summary>
        /// Plays a single audio sample (between -1.0 and +1.0)
        /// </summary>
        public static void Play(double sample)
        {
            if (double.IsNaN(sample))
                throw new ArgumentException("sample is NaN");

            // Clip if outside [-1, +1]
            sample = Math.Clamp(sample, -1.0, 1.0);

            // Save sample if recording
            if (isRecording)
            {
                recordedSamples?.Enqueue(sample);
            }

            // Convert to 16-bit PCM
            short pcm = (short)(sample * MAX_16_BIT);
            byte[] bytes = BitConverter.GetBytes(pcm);
            
            // Add to buffer
            if (bufferSize + 2 <= buffer.Length)
            {
                buffer[bufferSize++] = bytes[0];
                buffer[bufferSize++] = bytes[1];
            }
            
            // Play buffer if full
            if (bufferSize >= buffer.Length)
            {
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        // Write WAV header
                        WriteWavHeader(ms, bufferSize);
                        
                        // Write audio data
                        ms.Write(buffer, 0, bufferSize);
                        
                        // Rewind and play
                        ms.Seek(0, SeekOrigin.Begin);
                        player.Stream = ms;
                        player.PlaySync();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Audio playback error: {ex.Message}");
                }
                bufferSize = 0;
            }
        }

        private static void WriteWavHeader(Stream stream, int dataLength)
        {
            using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, true))
            {
                // RIFF header
                writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));
                writer.Write(36 + dataLength); // File size - 8
                writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));
                
                // fmt chunk
                writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));
                writer.Write(16); // Chunk size
                writer.Write((short)1); // PCM format
                writer.Write((short)1); // Mono
                writer.Write(SAMPLE_RATE); // Sample rate
                writer.Write(SAMPLE_RATE * 2); // Byte rate (SampleRate * NumChannels * BitsPerSample/8)
                writer.Write((short)2); // Block align (NumChannels * BitsPerSample/8)
                writer.Write((short)16); // Bits per sample
                
                // data chunk
                writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));
                writer.Write(dataLength); // Data size
            }
        }

        /// <summary>
        /// Plays an array of audio samples
        /// </summary>
        public static void Play(double[] samples)
        {
            if (samples == null)
                throw new ArgumentNullException(nameof(samples));

            foreach (var sample in samples)
            {
                Play(sample);
            }
        }

        /// <summary>
        /// Plays an audio file (blocks until complete)
        /// </summary>
        public static void Play(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Filename cannot be null or empty");

            try
            {
                using var player = new SoundPlayer(filename);
                player.PlaySync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not play audio file: {filename}", ex);
            }
        }

        /// <summary>
        /// Plays an audio file in a background thread
        /// </summary>
        public static void PlayInBackground(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Filename cannot be null or empty");

            var thread = new Thread(() => 
            {
                try
                {
                    using var player = new SoundPlayer(filename);
                    player.PlaySync();
                }
                catch
                {
                    // Ignore playback errors in background
                }
            });

            backgroundThreads.Add(thread);
            thread.Start();
        }

        /// <summary>
        /// Stops all background audio playback
        /// </summary>
        public static void StopInBackground()
        {
            foreach (var thread in backgroundThreads)
            {
                if (thread.IsAlive)
                {
                    thread.Abort();
                }
            }
            backgroundThreads.Clear();
        }

        /// <summary>
        /// Starts recording audio samples
        /// </summary>
        public static void StartRecording()
        {
            if (isRecording)
                throw new InvalidOperationException("Already recording");

            recordedSamples = new Queue<double>();
            isRecording = true;
        }

        /// <summary>
        /// Stops recording and returns recorded samples
        /// </summary>
        public static double[] StopRecording()
        {
            if (!isRecording)
                throw new InvalidOperationException("Not currently recording");

            isRecording = false;
            var result = recordedSamples.ToArray();
            recordedSamples = null;
            return result;
        }

        /// <summary>
        /// Drains any queued audio samples
        /// </summary>
        public static void Drain()
        {
            // Play any remaining samples in buffer
            if (bufferSize > 0)
            {
                // Pad buffer with silence if needed
                while (bufferSize < buffer.Length)
                {
                    buffer[bufferSize++] = 0;
                }
                
                using (var ms = new MemoryStream(buffer))
                {
                    player.Stream = ms;
                    player.PlaySync();
                }
                bufferSize = 0;
            }
            
            // Small delay to ensure playback completes
            Thread.Sleep(50);
        }

        /// <summary>
        /// Reads audio samples from a file
        /// </summary>
        public static double[] Read(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Filename cannot be null or empty");

            try
            {
                using var fs = new FileStream(filename, FileMode.Open);
                using var reader = new BinaryReader(fs);

                // Read WAV header
                string chunkID = new string(reader.ReadChars(4));
                if (chunkID != "RIFF")
                    throw new InvalidDataException("Not a valid WAV file");

                reader.ReadUInt32(); // File size
                string format = new string(reader.ReadChars(4));
                if (format != "WAVE")
                    throw new InvalidDataException("Not a valid WAV file");

                // Read fmt subchunk
                string subchunk1ID = new string(reader.ReadChars(4));
                if (subchunk1ID != "fmt ")
                    throw new InvalidDataException("Invalid WAV format");

                int subchunk1Size = reader.ReadInt32();
                int audioFormat = reader.ReadInt16();
                int numChannels = reader.ReadInt16();
                int sampleRate = reader.ReadInt32();
                reader.ReadInt32(); // Byte rate
                reader.ReadInt16(); // Block align
                int bitsPerSample = reader.ReadInt16();

                if (audioFormat != 1)
                    throw new InvalidDataException("Only PCM format supported");

                // Find data subchunk
                string subchunk2ID;
                do
                {
                    subchunk2ID = new string(reader.ReadChars(4));
                    int subchunk2Size = reader.ReadInt32();
                    if (subchunk2ID == "data")
                        break;
                    reader.BaseStream.Seek(subchunk2Size, SeekOrigin.Current);
                } while (true);

                // Read audio data
                int bytesPerSample = bitsPerSample / 8;
                int numSamples = (int)(reader.BaseStream.Length - reader.BaseStream.Position) / bytesPerSample;
                double[] samples = new double[numSamples];

                for (int i = 0; i < numSamples; i++)
                {
                    if (bitsPerSample == 16)
                    {
                        short sample = reader.ReadInt16();
                        samples[i] = sample / (double)short.MaxValue;
                    }
                    else if (bitsPerSample == 8)
                    {
                        byte sample = reader.ReadByte();
                        samples[i] = (sample - 128) / 128.0;
                    }
                    else
                    {
                        throw new InvalidDataException("Only 8-bit and 16-bit WAV files supported");
                    }
                }

                return samples;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not read audio file: {filename}", ex);
            }
        }

        /// <summary>
        /// Saves audio samples to a file
        /// </summary>
        public static void Save(string filename, double[] samples)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentException("Filename cannot be null or empty");
            if (samples == null)
                throw new ArgumentNullException(nameof(samples));

            try
            {
                using var fs = new FileStream(filename, FileMode.Create);
                using var writer = new BinaryWriter(fs);

                // WAV header
                writer.Write("RIFF".ToCharArray()); // Chunk ID
                writer.Write(0); // Placeholder for file size
                writer.Write("WAVE".ToCharArray()); // Format

                // fmt subchunk
                writer.Write("fmt ".ToCharArray());
                writer.Write(16); // Subchunk1Size (16 for PCM)
                writer.Write((short)1); // AudioFormat (1 = PCM)
                writer.Write((short)1); // NumChannels (1 = mono)
                writer.Write(SAMPLE_RATE); // SampleRate
                writer.Write(SAMPLE_RATE * 2); // ByteRate (SampleRate * NumChannels * BitsPerSample/8)
                writer.Write((short)2); // BlockAlign (NumChannels * BitsPerSample/8)
                writer.Write((short)16); // BitsPerSample

                // data subchunk
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2); // Subchunk2Size (NumSamples * NumChannels * BitsPerSample/8)

                // Write audio data
                foreach (var sample in samples)
                {
                    short pcm = (short)(Math.Clamp(sample, -1.0, 1.0) * short.MaxValue);
                    writer.Write(pcm);
                }

                // Update file size in header
                long fileSize = fs.Length;
                fs.Seek(4, SeekOrigin.Begin);
                writer.Write((int)(fileSize - 8));
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Could not save audio file: {filename}", ex);
            }
        }

        // Test client
        public static void RunTests()
        {
            // Play a 440 Hz tone (A4) for 1 second
            double freq = 440.0;
            for (int i = 0; i <= SAMPLE_RATE; i++)
            {
                Play(0.5 * Math.Sin(2 * Math.PI * freq * i / SAMPLE_RATE));
            }

            // Play a 880 Hz tone (A5) for 1 second
            freq = 880.0;
            for (int i = 0; i <= SAMPLE_RATE; i++)
            {
                Play(0.5 * Math.Sin(2 * Math.PI * freq * i / SAMPLE_RATE));
            }

            // Play a simple melody (C major scale)
            double[] notes = { 261.63, 293.66, 329.63, 349.23, 392.00, 440.00, 493.88, 523.25 };
            foreach (double note in notes)
            {
                for (int i = 0; i <= SAMPLE_RATE/4; i++)
                {
                    Play(0.5 * Math.Sin(2 * Math.PI * note * i / SAMPLE_RATE));
                }
            }

            Drain();
        }
    }
}
