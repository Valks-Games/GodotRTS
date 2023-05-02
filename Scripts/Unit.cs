namespace GodotRTS;

public partial class Unit : RigidBody2D
{
    private GTimer timerLogic;
    private Vector2 direction = Vector2.Zero;
    private float speed = 25;
    private State state = State.GatheringResources;

    public override void _Ready()
    {
        timerLogic = new GTimer(this, Logic, 1000) {
            Loop = true
        };
        timerLogic.StartMs();
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyCentralForce(direction * speed);
    }

    private void Logic()
    {
        if (state == State.GatheringResources)
        {
            if (Game.Resources.Count > 0)
            {
                var resourcePos = Game.Resources[0].Position;

                if (Position.DistanceTo(resourcePos) < 100)
                {
                    state = State.DroppingResourcesOffAtBase;
                }
                else
                {
                    direction = (resourcePos - Position).Normalized();
                }
            }
        }
        else if (state == State.DroppingResourcesOffAtBase)
        {
            var basePos = Game.Base.Position;
            direction = (basePos - Position).Normalized();

            if (Position.DistanceTo(basePos) < 100)
            {
                state = State.GatheringResources;
            }
        }
    }
}

public enum State
{
    GatheringResources,
    DroppingResourcesOffAtBase
}
