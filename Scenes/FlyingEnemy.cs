using Godot;
using System;
using System.Collections.Generic;

public partial class FlyingEnemy : Node2D
{
    private float _time = 0f;

    [Export]
    private float SineAmplitude = 200;

    [Export]
    private float SineSpeed = 1f;

    [Export]
    private float UpwardSpeed = 100f;

    private Vector2 _StartPosition;


    private Tween tween;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddToGroup("Enemies");
        _StartPosition = GlobalPosition;
    }

    public override void _Process(double delta)
    {
        _time += (float)delta;

        float xOffset = Mathf.Sin(_time * SineSpeed) * SineAmplitude;
        float yOffset = -(float)_time * UpwardSpeed;

        GlobalPosition = _StartPosition + new Vector2(xOffset, yOffset);

    }

    public void OnBodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Die();
        }
    }

    public void Hit()
    {
        //GD.Print("Hit called");
        QueueFree();
    }
}
