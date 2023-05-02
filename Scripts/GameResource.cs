namespace GodotRTS;

public partial class GameResource : Node2D, IEntity
{
    public event Action OnDestroyed;

    private Area2D areaDetect;
    private int amount = 3; // the amount of resource this resource has

    public override void _Ready()
    {
        areaDetect = GetNode<Area2D>("Area2D");
        areaDetect.BodyEntered += body =>
        {
            if (body is Unit unit && unit.Target is GameResource)
            {
                amount--;

                unit.MoveToTarget(Game.Team1Base);

                // resource has been depleted, destroy the resource
                if (amount <= 0)
                    Destroy();
            }
        };
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
        Game.Resources.Remove(this);
        QueueFree();
    }
}
