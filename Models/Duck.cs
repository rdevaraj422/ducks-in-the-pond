using DuckSimulatorApp.Behaviors;
using DuckSimulatorApp.Behaviors.Fly;

namespace DuckSimulatorApp.Models;

public abstract class Duck
{
    protected Duck(
        IQuackBehavior quackBehavior,
        ISwimBehavior swimBehavior,
        IFlyBehavior flyBehavior)
    {
        QuackBehavior = quackBehavior;
        SwimBehavior = swimBehavior;
        FlyBehavior = flyBehavior;
    }

    public IQuackBehavior QuackBehavior { get; set; }
    public ISwimBehavior SwimBehavior { get; set; }
    public IFlyBehavior FlyBehavior { get; set; }

    public abstract string Name { get; }
    public abstract string Emoji { get; }
    public abstract string Description { get; }

    public virtual (string text, string? soundFile) PerformQuack()
        => (QuackBehavior.QuackText, QuackBehavior.SoundFile);

    public virtual string PerformSwim()
        => SwimBehavior.SwimText;

    public virtual string PerformFly()
        => FlyBehavior.Fly();

    public abstract string Display();
}
