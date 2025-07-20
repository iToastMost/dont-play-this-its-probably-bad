using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void GameOverEventHandler();

	[Export]
	public PackedScene BulletScene { get; set; } 

	public const float Speed = 300.0f;
	public float JumpVelocity = -550.0f;
	public float deathHeight = 0;
	public Vector2 ScreenSize;
	private bool platformsFall;
	private bool platformsStops;
	public bool gameOver;

	Node2D rotate;
	MeshInstance2D bulletSpawn;

    public override void _Ready()
    {
		ScreenSize = GetViewportRect().Size;
		gameOver = false;
		platformsFall = false;
		platformsStops = true;

		rotate = GetNode<Node2D>("Rotation");
		bulletSpawn = rotate.GetNode<MeshInstance2D>("spawnBullet");
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

		if (Input.IsActionJustPressed("shoot")) 
		{
			//GD.Print("Shooting");
			var bullet = BulletScene.Instantiate<Area2D>();
			bullet.GlobalPosition = bulletSpawn.GlobalPosition;
			
			var bulletDirection = (bulletSpawn.GlobalPosition - rotate.GlobalPosition).Normalized();

			if(bullet is Bullet bulletScript) 
			{
				bulletScript.SetDirection(bulletDirection);
			}

            GetTree().CurrentScene.AddChild(bullet);

        }

		rotate.LookAt(GetGlobalMousePosition());
		rotate.RotationDegrees = Mathf.Clamp(rotate.RotationDegrees, -135, -45);
		

		Velocity = velocity;
		MoveAndSlide();

		Position = new Vector2(
			x: Mathf.Wrap(Position.X, 0, ScreenSize.X),
			y: Position.Y);


		//if(Position.Y > deathHeight && gameOver == false)
		{
			//gameOver = true;
			//Die();
		}
	}

	public void Start(Vector2 position)
	{
		Position = position;
		Show();
	}

	public void Bounce(float force) 
	{
		Velocity = new Vector2(Velocity.X, force);
    }

	public void Die()
	{
		EmitSignal(SignalName.GameOver);
		//Velocity = Vector2.Zero;
		//Hide();
		//GD.Print("You died!");
	}
}
