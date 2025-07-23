using Godot;
using System;

public partial class Easy : Node2D
{
    [Export]
    public PackedScene PlatformScene { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		
	}

	private void SpawnPlatforms()
	{
		var children = GetNode<Node>("PlatformPaths").GetChildren();
		for(int i = 0; i < children.Count; i++)
		{
			Platform platform = PlatformScene.Instantiate<Platform>();

			GD.Print("Spawning Platform from Easy Scene" + children[i].Name);

			var platformSpawnLocation = GetNode<PathFollow2D>("PlatformPaths/" + children[i].Name + "/SpawnPath");
			platformSpawnLocation.ProgressRatio = GD.Randf();

			platform.Position = platformSpawnLocation.Position;

			AddChild(platform);
		}
	}
}
