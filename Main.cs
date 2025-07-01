using Godot;
using System;

public partial class Main : Node
{
    [Signal]
    public delegate void FallEventHandler();

    [Signal]
    public delegate void stopFallEventHandler();

    [Export]
    public PackedScene PlatformScene { get; set; }

    PackedScene platforms;

    public override void _Ready()
    {
        NewGame();
        var playerPosition = GetNode<Player>("Player").Position;
        platforms = PlatformScene;
    }
    public void NewGame()
    {
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
    }

    private void OnArea2dBodyEntered(Node2D body)
    {
        EmitSignal(SignalName.Fall);
        GetTree().CallGroup("platforms", "Fall");
        GetNode<Timer>("PlatformTimer").Start();
        GD.Print("Entered");
    }

    private void OnArea2dBodyExited(Node2D body)
    {
        EmitSignal(SignalName.stopFall);
        GetNode<Timer>("PlatformTimer").Stop();
        GetTree().CallGroup("platforms", "StopFall");
        GD.Print("Exited");
    }

    private void OnPlatformTimerTimeout()
    {
        Platform platform = PlatformScene.Instantiate<Platform>();

        var platformSpawnLocation = GetNode<PathFollow2D>("PlatformPath/PlatformSpawnLocation");
        platformSpawnLocation.ProgressRatio = GD.Randf();

        platform.Position = platformSpawnLocation.Position;

        GD.Print("Platform Spawned");

        AddChild(platform);

        platforms = PlatformScene;
    }

}
