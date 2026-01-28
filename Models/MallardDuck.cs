using DuckSimulatorApp.Behaviors;
using DuckSimulatorApp.Behaviors.Fly;

namespace DuckSimulatorApp.Models;

public class MallardDuck : Duck
{
    public MallardDuck() : base(new Quack(), new SwimNormally(),new FlyWithWings()) { }

    public override string Name => "Mallard Duck";
    public override string Emoji => "🦆";
    public override string Description => "🦆 I'm a Mallard Duck - green head, brown body!";

    public override string Display() => "👀 Displaying Mallard Duck";
}
