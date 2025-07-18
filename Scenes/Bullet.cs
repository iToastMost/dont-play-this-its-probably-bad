using Godot;
using System;

public partial class Bullet : Area2D
{
	private float bulletSpeed = 750f;
	private Vector2 _velocity;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public void SetDirection(Vector2 direction) 
	{
		_velocity = direction * bulletSpeed;
		//Rotation = direction.Angle();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
	{
		Position += _velocity * (float)delta;

    }

    //TODO Create two functions for OnEnemyBodyEntered and OnScreenExited to free bullets

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
		QueueFree();
	}

	private void OnBodyEntered(Node2D body) 
	{
		Node parent = body.GetParent();
        if (parent is Enemy enemy) 
		{
			enemy.Hit();
			QueueFree();
		}
		GD.Print("Hit");
	}
}
