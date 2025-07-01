using Godot;
using System;

public partial class Platform : CharacterBody2D
{
    public const float fallVelocity = 350.0f;

    private bool hasEntered;

    public override void _Ready()
    {
        hasEntered = false;
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        if (hasEntered == true)
        {
            //var coll = GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
            SetCollisionMaskValue(1, false);
            velocity.Y = fallVelocity;
            MoveAndSlide();
        }
        else
        {
            //var coll = GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
            SetCollisionMaskValue(1, true);
            velocity.Y = 0;
        }

        velocity.X = 0;

        Velocity = velocity;

        //MoveAndSlide();
    }

    public void Fall()
    {
        hasEntered = true;
    }

    public void StopFall()
    {
        hasEntered = false;
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
        GD.Print("Platform Freed");
    }
}
