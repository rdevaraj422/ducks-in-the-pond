namespace DuckSimulatorApp.Behaviors;

public interface IQuackBehavior
{
    string QuackText { get; }        // "Quack!", "Squeak!", ""
    string? SoundFile { get; }       // "quack.wav", "squeak.wav", or null
}
