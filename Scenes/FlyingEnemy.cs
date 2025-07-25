using Godot;
using System;
using System.Collections.Generic;

public partial class FlyingEnemy : Node2D
{
    [Export]
    private Vector2 offSet = new Vector2(405, 0);

    [Export]
    private Vector2 onSet = new Vector2(0, 0);

    [Export]
    private float duration = 5.0f;

    private float verticalSpeed = 100f;

    private Tween _initialTween;
    private Tween _loopingTween;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddToGroup("Enemies");
        offSet.Y = Position.Y;
        onSet.Y = Position.Y;
        //StartTween();
    }

    public override void _Process(double delta)
    {
        GlobalPosition += new Vector2(0, -(float)delta * verticalSpeed);


    }

    private void StartTween()
    {
        InitialTween();
    }

    private void InitialTween()
    {
        var body = GetNode<AnimatableBody2D>("AnimatableBody2D");
        Vector2 start = body.GlobalPosition;
        Vector2 end = offSet;

        float fullDistance = offSet.DistanceTo(onSet);
        float baseSpeed = fullDistance / duration;

        float initialDistance = start.DistanceTo(end);
        float initialDuration = initialDistance / baseSpeed;

        _initialTween = GetTree().CreateTween().SetProcessMode(Tween.TweenProcessMode.Physics);


        _initialTween.TweenProperty(body, "global_position", offSet, initialDuration).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);

        _initialTween.TweenCallback(Callable.From(() => StartLoopTween(body)));
    }

    private void StartLoopTween(AnimatableBody2D body)
    {
        _loopingTween = GetTree().CreateTween().SetProcessMode(Tween.TweenProcessMode.Physics);

        _loopingTween.SetLoops().SetParallel(false);
        _loopingTween.TweenProperty(body, "global_position", onSet, duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        _loopingTween.TweenProperty(body, "global_position", offSet, duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
    }

    public void FreeMovingPlatform()
    {
        _initialTween?.Kill();
        _loopingTween?.Kill();

        SetProcess(false);
    }

    public override void _ExitTree()
    {
        FreeMovingPlatform();
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
