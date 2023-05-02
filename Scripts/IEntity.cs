namespace GodotRTS;

public interface IEntity
{
    public event Action OnDestroyed;

    public StringName Name { get; set; }
    public Vector2 Position { get; set; }
}
