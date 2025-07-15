using Godot;
using System;

public partial class Bullet : Area2D
{
	private int bulletSpeed = 750;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
        var velocity = new Vector2(0, -1);

		velocity = velocity.Normalized() * bulletSpeed;

		Position += velocity * (float)delta;

    }

	//TODO Create two functions for OnEnemyBodyEntered and OnScreenExited to free bullets
}
