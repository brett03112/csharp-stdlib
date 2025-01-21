using System;
using NAudio.Midi;

namespace csharp_stdlib
{
    /// <summary>
    /// Provides basic MIDI playback functionality similar to the Java StdMidi library.
    /// Allows playing notes, setting instruments, tempo and velocity.
    /// </summary>
    public static class StdMidi
    {
        // Constants for MIDI notes, instruments, durations etc.
        // (Same constants as Java version, omitted for brevity)
        
        private static MidiOut? midiOut;
        private static int tempo = 120;
        private static int velocity = 96;
        private static int currentInstrument = 1;
        private const int MidiChannel = 1; // Use channel 1 by default
        
        /// <summary>
        /// Initializes MIDI on first static access.
        /// Tries to initialize first available MIDI device, sets instrument to default (1).
        /// If no MIDI devices are found, prints a message to stderr.
        /// </summary> 
        static StdMidi()
        {
            try
            {
                // Initialize first available MIDI device
                if (MidiOut.NumberOfDevices > 0)
                {
                    midiOut = new MidiOut(0);
                    SetInstrument(currentInstrument);
                }
                else
                {
                    Console.Error.WriteLine("No MIDI devices found");
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error initializing MIDI: " + e.Message);
            }
        }

        /// <summary>
        /// Sets the MIDI instrument (patch) for subsequent notes.
        /// </summary>
        /// <param name="instrument">The instrument number (1-128)</param>
        /// <exception cref="ArgumentException">Thrown if instrument is out of range</exception>
        public static void SetInstrument(int instrument)
        {
            if (instrument < 1 || instrument > 128)
                throw new ArgumentException("Instrument must be between 1 and 128");
                
            currentInstrument = instrument;
            midiOut?.Send(MidiMessage.ChangePatch(instrument - 1, MidiChannel).RawData);
        }

        /// <summary>
        /// Sets the playback tempo in beats per minute.
        /// </summary>
        /// <param name="beatsPerMinute">Tempo in BPM (must be positive)</param>
        /// <exception cref="ArgumentException">Thrown if tempo is not positive</exception>
        public static void SetTempo(int beatsPerMinute)
        {
            if (beatsPerMinute <= 0)
                throw new ArgumentException("Tempo must be positive");
                
            tempo = beatsPerMinute;
        }

        /// <summary>
        /// Sets the velocity (volume) for subsequent notes.
        /// </summary>
        /// <param name="vel">Velocity value (0-127)</param>
        /// <exception cref="ArgumentException">Thrown if velocity is out of range</exception>
        public static void SetVelocity(int vel)
        {
            if (vel < 0 || vel > 127)
                throw new ArgumentException("Velocity must be between 0 and 127");
                
            velocity = vel;
        }

        /// <summary>
        /// Plays a MIDI note for a specified duration.
        /// </summary>
        /// <param name="note">MIDI note number (0-127)</param>
        /// <param name="beats">Duration in beats</param>
        /// <exception cref="ArgumentException">Thrown if note or beats are invalid</exception>
        public static void PlayNote(int note, double beats)
        {
            if (note < 0 || note > 127)
                throw new ArgumentException("Note must be between 0 and 127");
            if (beats < 0)
                throw new ArgumentException("Beats must be non-negative");

            NoteOn(note);
            Pause(beats);
            NoteOff(note);
        }

        private static void NoteOn(int note)
        {
            midiOut?.Send(MidiMessage.StartNote(note, velocity, MidiChannel).RawData);
        }

        private static void NoteOff(int note)
        {
            midiOut?.Send(MidiMessage.StopNote(note, 0, MidiChannel).RawData);
        }

        private static void Pause(double beats)
        {
            int ms = (int)(60000.0 / tempo * beats);
            System.Threading.Thread.Sleep(ms);
        }

        /// <summary>
        /// Stops all currently playing notes.
        /// </summary>
        public static void AllNotesOff()
        {
            midiOut.Reset();
        }

        /// <summary>
        /// Releases MIDI resources. Should be called when done using MIDI functionality.
        /// </summary>
        public static void Dispose()
        {
            midiOut?.Dispose();
        }
    }
}
