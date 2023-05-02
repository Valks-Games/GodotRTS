namespace GodotRTS;

public static class Prefabs
{
    public static PackedScene Options { get; } = Load("options");
    public static PackedScene Unit { get; } = Load("unit");

    private static PackedScene Load(string path) =>
        GD.Load<PackedScene>($"res://Scenes/Prefabs/{path}.tscn");
}
