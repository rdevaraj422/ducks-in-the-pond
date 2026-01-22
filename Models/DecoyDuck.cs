using DuckSimulatorApp.Behaviors;

namespace DuckSimulatorApp.Models;

public class DecoyDuck : Duck
{
    public DecoyDuck() : base(new MuteQuack(), new SwimNormally()) { }

    public override string Name => "Decoy Duck";
    public override string Emoji => "🪵";
    public override string Description => "🪵 I'm a Decoy Duck - silent wooden duck!";

    public override string Display() => "👀 Displaying Decoy Duck";
}
