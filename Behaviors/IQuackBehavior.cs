namespace DuckSimulatorApp.Behaviors;

public interface IQuackBehavior
{
    string QuackText { get; }        
    string? SoundFile { get; }       
}
