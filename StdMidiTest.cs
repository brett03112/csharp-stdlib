using Xunit;
using csharp_stdlib;

public class StdMidiTests
{
    [Fact]
    public void SetInstrument_ValidValue_SetsInstrument()
    {
        StdMidi.SetInstrument(1); // Acoustic Grand Piano
        StdMidi.SetInstrument(128); // Gunshot
        
        // No exception means success
        Assert.True(true);
    }

    [Fact]
    public void SetInstrument_InvalidValue_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => StdMidi.SetInstrument(0));
        Assert.Throws<ArgumentException>(() => StdMidi.SetInstrument(129));
    }

    [Fact] 
    public void SetTempo_ValidValue_SetsTempo()
    {
        StdMidi.SetTempo(60); // Largo
        StdMidi.SetTempo(200); // Prestissimo
        
        // No exception means success
        Assert.True(true);
    }

    [Fact]
    public void SetTempo_InvalidValue_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => StdMidi.SetTempo(0));
        Assert.Throws<ArgumentException>(() => StdMidi.SetTempo(-1));
    }

    [Fact]
    public void SetVelocity_ValidValue_SetsVelocity()
    {
        StdMidi.SetVelocity(0); // Silent
        StdMidi.SetVelocity(127); // Loudest
        
        // No exception means success
        Assert.True(true);
    }

    [Fact]
    public void SetVelocity_InvalidValue_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => StdMidi.SetVelocity(-1));
        Assert.Throws<ArgumentException>(() => StdMidi.SetVelocity(128));
    }

    [Fact]
    public void PlayNote_ValidValues_PlaysNote()
    {
        StdMidi.SetTempo(120);
        StdMidi.PlayNote(60, 1.0); // Middle C, quarter note
        StdMidi.PlayNote(72, 0.5); // C5, eighth note
        
        // No exception means success
        Assert.True(true);
    }

    [Fact]
    public void PlayNote_InvalidValues_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => StdMidi.PlayNote(-1, 1.0));
        Assert.Throws<ArgumentException>(() => StdMidi.PlayNote(128, 1.0));
        Assert.Throws<ArgumentException>(() => StdMidi.PlayNote(60, -1.0));
    }

    [Fact]
    public void AllNotesOff_ResetsMidi()
    {
        StdMidi.PlayNote(60, 0.1);
        StdMidi.AllNotesOff();
        
        // No exception means success
        Assert.True(true);
    }
}
