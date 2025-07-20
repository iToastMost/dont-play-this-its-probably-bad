using Godot;
using System;

public partial class MovingPlatform : Node2D
{
    [Export]
    private Vector2 offSet = new Vector2(405, 0);

    [Export]
    private Vector2 onSet = new Vector2(0, 0);

    [Export]
    private float duration = 5.0f;

    private Tween _initialTween;
    private Tween _loopingTween;

    public override void _Ready()
    {
        offSet.Y = Position.Y;
        onSet.Y = Position.Y;
        StartTween();
    }

    private void StartTween()
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

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        FreeMovingPlatform();
    }

    public void FreeMovingPlatform() 
    {
        if (_initialTween != null)
        {
            _initialTween.Kill();
        }
        if (_loopingTween != null)
        {
            _loopingTween.Kill();
        }
        QueueFree();
        GD.Print("Platform Freed");
    }

}
