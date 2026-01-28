using DuckSimulatorApp.Behaviors;
using DuckSimulatorApp.Behaviors.Fly;

namespace DuckSimulatorApp.Models;

public class RubberDuck : Duck
{
    public RubberDuck() : base(new Squeak(), new SwimNormally(),new FlyNoWay()) { }

    public override string Name => "Rubber Duck";
    public override string Emoji => "🐥";
    public override string Description => "🐥 I'm a Rubber Duck - I squeak instead of quack!";

    public override string Display() => "👀 Displaying Rubber Duck";
}
