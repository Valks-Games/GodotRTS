namespace GodotRTS;

public partial class Game : Node
{
    public static List<Resource> Resources { get; } = new();
    public static Base Base { get; private set; }

    public override void _Ready()
    {
        GetBase();
        GetResources();
    }

    private void GetBase()
    {
        Base = GetNode<Base>("Team 1/Base");
    }

    private void GetResources()
    {
        var nodes = GetNode("Environment/Resources");

        foreach (var resource in nodes.GetChildren<Resource>())
            Resources.Add(resource);
    }
}
