namespace GodotRTS;

public partial class Unit : RigidBody2D, IEntity
{
    public event Action OnDestroyed;

    public IEntity Target { get; private set; }
    public Vector2 Direction { get; private set; } = Vector2.Zero;

    private IEntity prevTarget;
    private float speed = 150;
    private GTimer timerUpdate;
    private readonly HashSet<Unit> toSkip = new();

    public override void _Ready()
    {
        timerUpdate = new(this, Update, 500);
        timerUpdate.Loop = true;
        SetPhysicsProcess(false);

        var area = GetNode<Area2D>("Area2D");
        area.BodyEntered += body =>
        {
            if (body is Unit unit)
            {
                if (toSkip.Contains(unit))
                {
                    toSkip.Remove(unit);
                    return;
                }

                var dirA = Direction;
                var dirB = unit.Direction;

                Direction = dirB.Orthogonal();
                unit.Direction = -dirA.Orthogonal();

                unit.toSkip.Add(this);
            }
        };
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyCentralForce(Direction * speed);
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
        QueueFree();
    }

    public void MoveToTarget(IEntity target)
    {
        timerUpdate.Start();
        SetPhysicsProcess(true);

        prevTarget = this.Target;
        this.Target = target;

        if (prevTarget != null)
            prevTarget.OnDestroyed -= DoSomethingIfTargetDestroyed;

        // Target could be destroyed in the future, lets listen for this
        target.OnDestroyed += DoSomethingIfTargetDestroyed;

        // Do not wait for timer to update direction, update direction instantly
        UpdateDirection();
    }

    private void Update()
    {
        UpdateDirection();
    }

    private void DoSomethingIfTargetDestroyed()
    {
        // maybe do something more interesting later on
        timerUpdate.Stop();
        SetPhysicsProcess(false);
    }

    private void UpdateDirection()
    {
        Direction = (Target.Position - Position).Normalized();
    }
}
