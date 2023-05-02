namespace GodotRTS;

public partial class Game : Node
{
    public static List<GameResource> Resources { get; } = new();
    public static Base Team1Base { get; private set; }

    private readonly static List<Unit> units = new();
    private static Node team1UnitsParent;

    public static GameResource FindNearestResource(Vector2 position)
    {
        GameResource nearestResource = null;
        float minDistance = Mathf.Inf;

        foreach (var resource in Resources)
        {
            var distance = position.DistanceTo(resource.Position);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestResource = resource;
            }
        }

        return nearestResource;
    }

    public static void AddUnit(Unit unit)
    {
        team1UnitsParent.AddChild(unit);
        units.Add(unit);
    }

    public override async void _Ready()
    {
        team1UnitsParent = GetNode("Team 1/Units");
        Team1Base = GetNode<Base>("Team 1/Base");
        GetResources();

        for (int i = 0; i < 3; i++)
        {
            Team1Base.CreateUnit();
            await Task.Delay(500);
        }
    }

    private void GetResources()
    {
        var nodes = GetNode("Environment/Resources");

        foreach (var resource in nodes.GetChildren<GameResource>())
            Resources.Add(resource);
    }
}
