namespace GodotRTS;

public partial class GameResource : Node2D
{
    [Signal] public delegate void DestroyedEventHandler();

    private Area2D areaDetect;

    public override void _Ready()
    {
        areaDetect = GetNode<Area2D>("Area2D");
        areaDetect.BodyEntered += body =>
        {
            if (body is Unit unit)
            {
                unit.MoveToBase();
            }
        };
    }

    public void Destroy()
    {
        EmitSignal(SignalName.Destroyed);
    }
}
