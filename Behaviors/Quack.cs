namespace DuckSimulatorApp.Behaviors;

public class Quack : IQuackBehavior
{
    public string QuackText => "Quack!";
    public string? SoundFile => "quack.wav";
}
