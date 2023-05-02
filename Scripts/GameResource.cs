namespace GodotRTS;

public partial class GameResource : Node2D
{
    [Signal] public delegate void DestroyedEventHandler();

    private Area2D areaDetect;
    private int amount = 4; // the amount of resource this resource has

    public override void _Ready()
    {
        areaDetect = GetNode<Area2D>("Area2D");
        areaDetect.BodyEntered += body =>
        {
            if (body is Unit unit)
            {
                amount--;

                // resource has been depleted, destroy the resource
                if (amount <= 0)
                    Destroy();

                unit.MoveToBase();
            }
        };
    }

    public void Destroy()
    {
        EmitSignal(SignalName.Destroyed);
        QueueFree();
    }
}
