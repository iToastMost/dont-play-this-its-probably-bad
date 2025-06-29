using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -450.0f;
	public Vector2 ScreenSize;

    public override void _Ready()
    {
		ScreenSize = GetViewportRect().Size;
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();

		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X), 
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y));
	}

	public void Start(Vector2 position)
	{
		Position = position;
	}
}
