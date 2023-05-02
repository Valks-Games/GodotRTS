namespace GodotRTS;

public interface IEntity
{
    public event Action OnDestroyed;

    public Vector2 Position { get; set; }
}
