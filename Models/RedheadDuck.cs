using DuckSimulatorApp.Behaviors;

namespace DuckSimulatorApp.Models;

public class RedheadDuck : Duck
{
    public RedheadDuck() : base(new Quack(), new SwimNormally()) { }

    public override string Name => "Redhead Duck";
    public override string Emoji => "🦆";
    public override string Description => "🦆 I'm a Redhead Duck - reddish head, classic look!";

    public override string Display() => "👀 Displaying Redhead Duck";
}
