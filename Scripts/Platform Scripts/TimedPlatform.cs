using Godot;
using System;

public partial class TimedPlatform : StaticBody2D
{
    Timer timer;
    private bool timerStarted;
    private double duration;

    [Export]
    private double timeRangeStart = 0;

    [Export]
    private double timeRangeEnd = 0;

    Tween tween;
    public override void _Ready()
    {
        duration = GD.RandRange(timeRangeStart, timeRangeEnd);
        timer = GetNode<Timer>("Timer");
        timerStarted = false;
        timer.WaitTime = duration;

        ColorTween(duration);
    }

    public void Timeout() 
    {
        QueueFree();
    }

    private void ColorTween(double duration) 
    {
        var body = GetNode<MeshInstance2D>("MeshInstance2D");
        tween = GetTree().CreateTween().SetProcessMode(Tween.TweenProcessMode.Physics);

        var color = new Color(1f, 0f, 0f, 1f);
        color.R = 255;
        color.G = 0;
        color.B = 0;
        color.A = 255;

        tween.SetLoops(1).SetParallel(false);
        tween.TweenProperty(body, "modulate", color, duration).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.In);
    }

    public void OnScreenEntered() 
    {
        if(timerStarted == false) 
        {
            timer.Start();
            timerStarted = true;
        }
        
    }
}
