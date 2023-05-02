namespace GodotRTS;

public partial class Unit : RigidBody2D, IEntity
{
    public event Action OnDestroyed;

    private Vector2 direction = Vector2.Zero;
    private IEntity prevTarget, target;
    private float speed = 25;
    private GRepeatingTimer timerUpdate;

    public override void _Ready()
    {
        timerUpdate = new(this, Update, 500);
        MoveToTarget(Game.Resources[0]);
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyCentralForce(direction * speed);
    }

    public void Destroy()
    {
        OnDestroyed?.Invoke();
        QueueFree();
    }

    public void MoveToTarget(IEntity target)
    {
        prevTarget = this.target;
        this.target = target;

        UnsubscribeFromOldEvents();

        // Resource could be destroyed in the future, lets listen for this
        target.OnDestroyed += DoSomethingIfTargetDestroyed;

        UpdateDirection();
    }

    private void Update()
    {
        UpdateDirection();
    }

    private void UnsubscribeFromOldEvents()
    {
        if (prevTarget is GameResource)
        {
            if (Game.Resources[0] != null)
                Game.Resources[0].OnDestroyed -= DoSomethingIfTargetDestroyed;
        }
        else if (prevTarget is Base)
        {
            if (Game.Team1Base != null)
                Game.Team1Base.OnDestroyed -= DoSomethingIfTargetDestroyed;
        }
    }

    private void DoSomethingIfTargetDestroyed()
    {
        // maybe do something more interesting later on
        timerUpdate.Stop();
        SetPhysicsProcess(false);
    }

    private void UpdateDirection()
    {
        direction = (target.Position - Position).Normalized();
    }
}
