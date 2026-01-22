namespace DuckSimulatorApp.Behaviors;

public class Squeak : IQuackBehavior
{
    public string QuackText => "Squeak!";
    public string? SoundFile => "squeak.wav";
}
