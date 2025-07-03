using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void FallEventHandler();

	[Signal]
	public delegate void StopFallEventHandler();

	[Signal]
	public delegate void GameOverEventHandler();

	public const float Speed = 300.0f;
	public const float JumpVelocity = -450.0f;
	public float deathHeight = 1200;
	public Vector2 ScreenSize;
	private bool platformsFall;
	private bool platformsStops;

    public override void _Ready()
    {
		ScreenSize = GetViewportRect().Size;
		platformsFall = false;
		platformsStops = true;
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
			x: Mathf.Wrap(Position.X, 0, ScreenSize.X),
			y: Position.Y);


		if(Position.Y > deathHeight)
		{
			Die();
		}
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
	}

	private void Die()
	{
		EmitSignal(SignalName.GameOver);
		Hide();
		GD.Print("You died!");
	}
}
