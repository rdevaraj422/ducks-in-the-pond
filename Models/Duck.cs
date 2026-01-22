using DuckSimulatorApp.Behaviors;

namespace DuckSimulatorApp.Models;

public abstract class Duck
{
    protected Duck(IQuackBehavior quackBehavior, ISwimBehavior swimBehavior)
    {
        QuackBehavior = quackBehavior;
        SwimBehavior = swimBehavior;
    }

    public IQuackBehavior QuackBehavior { get; set; }
    public ISwimBehavior SwimBehavior { get; set; }

    public abstract string Name { get; }
    public abstract string Emoji { get; }
    public abstract string Description { get; }

    public virtual (string text, string? soundFile) PerformQuack()
        => (QuackBehavior.QuackText, QuackBehavior.SoundFile);

    public virtual string PerformSwim()
        => SwimBehavior.SwimText;

    public abstract string Display(); // each subtype implements
}
