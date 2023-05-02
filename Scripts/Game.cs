namespace GodotRTS;

public partial class Game : Node
{
    public static List<GameResource> Resources { get; } = new();
    public static Base Team1Base { get; private set; }

    private readonly static List<Unit> units = new();
    private static Node team1UnitsParent;

    public static void AddUnit(Unit unit)
    {
        team1UnitsParent.AddChild(unit);
        units.Add(unit);
    }

    public override void _Ready()
    {
        team1UnitsParent = GetNode("Team 1/Units");
        Team1Base = GetNode<Base>("Team 1/Base");
        GetResources();

        for (int i = 0; i < 3; i++)
        {
            Team1Base.CreateUnit();
        }
    }

    private void GetResources()
    {
        var nodes = GetNode("Environment/Resources");

        foreach (var resource in nodes.GetChildren<GameResource>())
            Resources.Add(resource);
    }
}
