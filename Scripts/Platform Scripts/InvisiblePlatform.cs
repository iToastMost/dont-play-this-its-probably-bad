using Godot;
using System;

public partial class InvisiblePlatform : StaticBody2D
{
    private float _duration = 2;

    private Tween _tween;
    public override void _Ready()
    {
        AddToGroup("Platforms");
        StartTween();
    }

    private void StartTween() 
    {
        var opacity = GetNode<Sprite2D>("Platform");
        float opacityLow = 0.2f;
        float opacityHigh = 0.5f;

        _tween = GetTree().CreateTween().SetProcessMode(Tween.TweenProcessMode.Physics);

        _tween.SetLoops().SetParallel(false);
        _tween.TweenProperty(opacity, "modulate:a", opacityLow, _duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        _tween.TweenProperty(opacity, "modulate:a", opacityHigh, _duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
    }

    private void FreePlatform() 
    {
        _tween?.Kill();
        SetProcess(false);
    }

    public override void _ExitTree()
    {
        FreePlatform();
    }

}
