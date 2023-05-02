namespace GodotRTS;

public partial class Unit : RigidBody2D
{
    [Signal] public delegate void DestroyedEventHandler();

    private Vector2 direction = Vector2.Zero;
    private Vector2 targetPos = Vector2.Zero;
    private float speed = 25;
    private State prevState = State.Idle;
    private State state;

    public override void _Ready()
    {
        MoveToResource();
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyCentralForce(direction * speed);
    }

    public void Update()
    {
        UpdateDirection();
    }

    public void Destroy()
    {
        EmitSignal(SignalName.Destroyed);
    }

    public void MoveToResource()
    {
        if (Game.Resources.Count == 0)
            return;

        SwitchState(State.MovingToResource);

        if (prevState == State.MovingToBase)
        {
            Game.Team1Base.Destroyed -= StopIfTargetDestroyed;
        }

        Game.Resources[0].Destroyed += StopIfTargetDestroyed;

        targetPos = Game.Resources[0].Position;
        UpdateDirection();
    }

    public void MoveToBase()
    {
        if (Game.Team1Base == null)
            return;

        SwitchState(State.MovingToBase);

        if (prevState == State.MovingToResource)
        {
            Game.Resources[0].Destroyed -= StopIfTargetDestroyed;
        }

        Game.Team1Base.Destroyed += StopIfTargetDestroyed;

        targetPos = Game.Team1Base.Position;
        UpdateDirection();
    }

    private void StopIfTargetDestroyed()
    {
        if (state == State.MovingToResource)
        {
            SetPhysicsProcess(false);
        }
    }

    private void UpdateDirection()
    {
        direction = (targetPos - Position).Normalized();
    }

    private void SwitchState(State state)
    {
        prevState = this.state;
        this.state = state;
    }
}

public enum State
{
    MovingToResource,
    MovingToBase,
    Idle
}
