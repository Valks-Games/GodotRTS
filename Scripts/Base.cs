using GodotUtils;

namespace GodotRTS;

public partial class Base : Node2D, IEntity
{
    public event Action OnDestroyed;

    private Vector2 unitSpawnPos;
    private Area2D areaDetect;

    public override void _Ready()
    {
        unitSpawnPos = GetNode<Marker2D>("UnitSpawnPos").GlobalPosition;
        areaDetect = GetNode<Area2D>("Area2D");
        areaDetect.BodyEntered += body =>
        {
            if (body is Unit unit)
            {
                var nearestResource = Game.FindNearestResource(unit.Position);

                if (nearestResource != null)
                    unit.MoveToTarget(nearestResource);
            } 
        };
    }

    public void CreateUnit()
    {
        var unit = (Unit)Prefabs.Unit.Instantiate();
        unit.Position = unitSpawnPos;
        Game.AddUnit(unit);
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
        QueueFree();
    }
}
