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
        QueueFree();
    }

    public void MoveToResource()
    {
        // There are no resources, do nothing
        if (Game.Resources.Count == 0)
            return;

        SwitchState(State.MovingToResource);

        if (prevState == State.MovingToBase)
        {
            // This base could have been destroyed before
            if (Game.Team1Base != null)
                // No longer need to listen to this event
                Game.Team1Base.Destroyed -= StopIfTargetDestroyed;
        }

        // Resource could be destroyed in the future, lets listen for this
        Game.Resources[0].Destroyed += StopIfTargetDestroyed;

        // Update movement direction
        targetPos = Game.Resources[0].Position;
        UpdateDirection();
    }

    public void MoveToBase()
    {
        // There is no base, do nothing
        if (Game.Team1Base == null)
            return;

        SwitchState(State.MovingToBase);

        if (prevState == State.MovingToResource)
        {
            // This resource could have been destroyed before
            if (Game.Resources[0] != null)
                // No longer need to listen to this event
                Game.Resources[0].Destroyed -= StopIfTargetDestroyed;
        }

        // Base could be destroyed in the future, lets listen for this
        Game.Team1Base.Destroyed += StopIfTargetDestroyed;

        // Update movement direction
        targetPos = Game.Team1Base.Position;
        UpdateDirection();
    }

    private void StopIfTargetDestroyed()
    {
        // maybe switch to something like State.FindSomethingToDo later on
        state = State.Idle;
        SetPhysicsProcess(false);
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
