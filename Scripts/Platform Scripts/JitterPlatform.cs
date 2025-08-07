using Godot;
using System;

public partial class JitterPlatform : AnimatableBody2D
{
	[Export]
	public float sineAmplitudeX = 10f;

	[Export]
	public float sineSpeedX = 10f;

    [Export]
    public float sineAmplitudeY = 10f;

    [Export]
    public float sineSpeedY = 10f;

    private float _time = 0;

	private Vector2 _startPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_startPosition = Position;
        AddToGroup("Platforms");
		AddToGroup("JitterPlatforms");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		Move(delta);
	}

	private void Move(double delta) 
	{
		_time += (float)delta;

		float jitterX = Mathf.Sin(_time * sineSpeedX) * sineAmplitudeX;
		float jitterY = Mathf.Sin(_time * sineSpeedY) * sineAmplitudeY;

		Position = _startPosition + new Vector2(jitterX, jitterY);
	}

	public void MoveAllPlatforms() 
	{
		var jitterPlatforms = GetTree().GetNodesInGroup("JitterPlatforms");

		foreach(JitterPlatform jitterPlatform in jitterPlatforms) 
		{
			//jitterPlatform.Position.X = new Vector2(GD.RandRange(0, 405), 0);
			jitterPlatform.Position = new Vector2(GD.RandRange(0, 405), Position.Y);
		}
	}
}
