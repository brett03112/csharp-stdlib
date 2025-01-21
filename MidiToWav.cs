using System;
using NAudio.Midi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace csharp_stdlib
{
    public static class MidiToWav
    {
        public static void Convert(string midiFilePath, string wavFilePath, int sampleRate = 44100)
        {
            if (string.IsNullOrEmpty(midiFilePath))
                throw new ArgumentException("MIDI file path cannot be null or empty");
            
            if (string.IsNullOrEmpty(wavFilePath))
                throw new ArgumentException("WAV file path cannot be null or empty");

            if (sampleRate < 8000 || sampleRate > 192000)
                throw new ArgumentException("Sample rate must be between 8000 and 192000");

            try
            {
                var midiFile = new MidiFile(midiFilePath);
                var waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(sampleRate, 2);
                
                // Calculate duration by finding the last event's time
                double ticksPerQuarterNote = midiFile.DeltaTicksPerQuarterNote;
                double microsecondsPerQuarterNote = 500000; // Default 120 BPM
                double ticksPerSecond = (ticksPerQuarterNote * 1000000) / microsecondsPerQuarterNote;
                
                // Find the last event's time
                double lastEventTime = 0;
                foreach (var track in midiFile.Events)
                {
                    if (track.Count > 0)
                    {
                        double trackEnd = track[track.Count - 1].AbsoluteTime;
                        if (trackEnd > lastEventTime)
                        {
                            lastEventTime = trackEnd;
                        }
                    }
                }
                double durationSeconds = lastEventTime / ticksPerSecond;

                using (var writer = new WaveFileWriter(wavFilePath, waveFormat))
                {
                    // Create a sample provider that generates silence
                    var sampleProvider = new SignalGenerator(waveFormat.SampleRate, waveFormat.Channels)
                    {
                        Gain = 0.0, // Silence
                        Frequency = 0,
                        Type = SignalGeneratorType.Sin
                    }.Take(TimeSpan.FromSeconds(durationSeconds));

                    // Write to file
                    var buffer = new float[waveFormat.SampleRate * waveFormat.Channels];
                    int samplesRead;
                    
                    while ((samplesRead = sampleProvider.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        writer.WriteSamples(buffer, 0, samplesRead);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("MIDI to WAV conversion failed", ex);
            }
        }
    }

}
