using Godot;
using System;

public partial class FloatingEnemy : AnimatableBody2D
{
	[Export]
	private float _sineAmplitudeX = 200;

	[Export]
	private float _sineSpeedX = 1f;

	[Export]
	private float _sineSpeedY = 5f;

	[Export]
	private float _sineAmplitudeY = 25;

	private float _time = 0f;

	private float _tau;

	private Vector2 _startPosition;
	private Area2D _area2D;

	private AudioStreamPlayer2D _deathSound;
	private Timer _deathTimer;
	private Sprite2D _sprite;
	private FloatingEnemy _floatingEnemy;
	private Area2D _enemyArea;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddToGroup("Enemies");
		_startPosition = Position;
		_area2D = GetNode<Area2D>("Area2D");
		_tau = Mathf.Tau * 2;
		_deathSound = GetNode<AudioStreamPlayer2D>("DeathSound");
		_deathTimer = GetNode<Timer>("DeathTimer");
		_sprite = GetNode<Sprite2D>("Sprite2D");
		_enemyArea = GetNode<Area2D>("Area2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		_time += (float)delta;

		float xOffset = Mathf.Sin(_time * _sineSpeedX) * _sineAmplitudeX;
		float yOffset = Mathf.Sin(_time * _sineSpeedY) * _sineAmplitudeY;
		Position = _startPosition + new Vector2(xOffset, yOffset);
	}

	public void OnBodyEnter(Node2D body)
	{
		if (body is Player player)
		{
			player.Die();
		}
	}

	public void OnAreaEnter(Area2D area)
	{
		if (area.IsInGroup("Bullets"))
		{
			HideEnemy();
            PlayDeathSound();
			area.QueueFree();
			_deathTimer.Start();
		}
	}

	private void PlayDeathSound()
	{
		if (!_deathSound.Playing)
		{
			_deathSound.Play();
		}
	}
	public void Hit()
	{
		HideEnemy();
		PlayDeathSound();
		_deathTimer.Start();
	}

	private void DeathTimerTrigger() 
	{
		QueueFree();
	}

	private void HideEnemy() 
	{
        _sprite.Visible = false;
		_enemyArea.SetCollisionLayerValue(4, false);
    }
}
