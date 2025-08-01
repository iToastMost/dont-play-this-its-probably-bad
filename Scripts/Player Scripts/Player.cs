using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void GameOverEventHandler();

	[Export]
	public PackedScene BulletScene { get; set; }

	[Export]
	private float bounceForce = -1200f;

	[Export]
	private float bounceOffEnemyForce = -650f;

	[Export]
	public float JumpVelocity = -550.0f;

	public const float Speed = 300.0f;
	public float deathHeight = 0;
	public Vector2 ScreenSize;
	private bool platformsFall;
	private bool platformsStops;
	public bool gameOver;

	private float clampAimLeft = -135f;
	private float clampAimRight = -45f;



	private Vector2 _calculatedVelocity;
	Node2D rotate;
	MeshInstance2D bulletSpawn;
	RayCast2D landingCheck;

	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		gameOver = false;
		platformsFall = false;
		platformsStops = true;

		rotate = GetNode<Node2D>("Rotation");
		bulletSpawn = rotate.GetNode<MeshInstance2D>("spawnBullet");
		landingCheck = GetNode<RayCast2D>("LandingCheck");
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
			Shoot();
		}

		rotate.LookAt(GetGlobalMousePosition());
		rotate.RotationDegrees = Mathf.Clamp(rotate.RotationDegrees, clampAimLeft, clampAimRight);

		_calculatedVelocity = velocity;
		BounceCheck();

		Velocity = _calculatedVelocity;
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
		//GD.Print("Bouncing");
		_calculatedVelocity.Y = force;
	}

	public void BounceCheck()
	{
		if (landingCheck.IsColliding())
		{
			//GD.Print("Landing Check Hit: " + landingCheck.GetCollider());
			var collision = landingCheck.GetCollider();
			var node = collision as Node;
			var parent = node?.GetParent();
			if (collision is Spring spring && Velocity.Y > 0)
			{
				spring?.PlayAnimation();
				//GD.Print("Raycast hit: ", colission.GetType(), " - ", node.Name);
				Bounce(bounceForce);
			}

			if (parent is Enemy enemy && Velocity.Y > 0)
			{
				GD.Print("Raycast hit: ", collision.GetType());
				enemy?.Hit();
				Bounce(bounceOffEnemyForce);
			}

			if(parent is FlyingEnemy flying && Velocity.Y > 0) 
			{
				flying?.Hit();
				Bounce(bounceOffEnemyForce);
			}

			if(collision is OneJumpPlatform oneJumpPlatform && Velocity.Y > 0) 
			{
				oneJumpPlatform.Hit();
				Bounce(JumpVelocity);
			}

            if (parent is FloatingEnemy floatingEnemy && Velocity.Y > 0)
            {
				floatingEnemy?.Hit();
                Bounce(bounceOffEnemyForce);
            }

        }
	}

	private void Shoot()
	{
		//GD.Print("Shooting");
		var bullet = BulletScene.Instantiate<Area2D>();
		bullet.GlobalPosition = bulletSpawn.GlobalPosition;

		var bulletDirection = (bulletSpawn.GlobalPosition - rotate.GlobalPosition).Normalized();

		if (bullet is Bullet bulletScript)
		{
			bulletScript.SetDirection(bulletDirection);
		}

		GetTree().CurrentScene.AddChild(bullet);
	}

	private void OnBodyEntered(Node2D body)
	{
        if (body != null && (body.IsInGroup("Platforms") || body.IsInGroup("Enemies"))) 
		{
			GD.Print("Freed: " + body.GetType());
			body.SetProcess(false);
			body.QueueFree();
		}
	}

	public void Die()
	{
		EmitSignal(SignalName.GameOver);
	}
}
