using Godot;
using System;
using System.Collections.Generic;

public partial class VerticalPlatform : Node2D
{
	[Export]
	public Vector2 offset = new Vector2(0, -400);

	[Export]
	public Vector2 onset = new Vector2(0, 0);

	private double duration = 5;
	
	private Tween tween;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        offset.X = Position.X;
        onset.X = Position.X;
        StartTween();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void StartTween() 
	{
		var body = GetNode<AnimatableBody2D>("AnimatableBody2D");

		tween = GetTree().CreateTween().SetProcessMode(Tween.TweenProcessMode.Physics);

		tween.SetLoops().SetParallel(false);
		tween.TweenProperty(body, "global_position", offset, duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
        tween.TweenProperty(body, "global_position", onset, duration / 2).SetTrans(Tween.TransitionType.Sine).SetEase(Tween.EaseType.InOut);
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
		tween.Kill();
        QueueFree();
        GD.Print("Platform Freed");
    }
}
