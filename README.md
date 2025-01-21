# C# Standard Library

This repository contains a comprehensive standard library implementation in C#. The library provides utility classes for various domains including I/O, graphics, audio, statistics, and more.

It has been implemented from the Princeton stdlib.jar and the corresponding course.  This is a work in progress and should not be used until all tests and implementations are complete.
## Core Features

### Input/Output
- `BinaryIn.cs`: Binary input stream
- `BinaryOut.cs`: Binary output stream  
- `StdIn.cs`: Standard input wrapper
- `StdOut.cs`: Standard output wrapper
- `StdArrayIO.cs`: Array I/O utilities

### Graphics & Drawing
- `Draw.cs`: Basic drawing functions
- `StdDraw.cs`: Standard drawing library
- `Picture.cs`: Image manipulation
- `GrayscalePicture.cs`: Grayscale image processing
- `DrawListener.cs`: Drawing event handling
- `DrawMouseListener.cs`: Mouse input handling

### Audio & MIDI
- `StdAudio.cs`: Audio playback and recording
- `StdAudioStereo.cs`: Stereo audio support
- `StdMidi.cs`: MIDI playback and control
- `PlayMusic.cs`: Music playback utilities
- `MidiToWav.cs`: MIDI to WAV conversion

### Statistics & Randomness
- `StdStats.cs`: Statistical functions
- `StdRandom.cs`: Random number generation

### Utilities
- `Stopwatch.cs`: High-resolution timing
- `In.cs`: Input utilities
- `Out.cs`: Output utilities

### Testing
The library includes comprehensive test coverage with test classes for each major component:
- `BinaryInTest.cs`, `BinaryOutTest.cs`
- `DrawTest.cs`, `StdDrawTest.cs`
- `StdAudioTest.cs`, `StdMidiTest.cs`
- `StdRandomTest.cs`, `StdStatsTest.cs`
- `StopwatchTest.cs`

## Project Structure
- `csharp_stdlib.csproj`: Main project file
- `csharp_stdlib.sln`: Visual Studio solution file
- `Program.cs`: Example usage and demos

## Building and Testing
The project targets .NET 8.0 and includes both Windows-specific and cross-platform implementations.

To build and run tests:
```bash
dotnet build
dotnet test
```

## Dependencies
- NAudio for audio processing
- System.Drawing for graphics
- Windows Media Player interop for media playback
