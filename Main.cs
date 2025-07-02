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
        SpawnPlatform();
        platforms = PlatformScene;
    }
    public void NewGame()
    {
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
        var hud = GetNode<Hud>("HUD");
        hud.HideHud();
    }

    private void GameOver()
    {
        var hud = GetNode<Hud>("HUD");
        hud.ShowGameOver();
    }

    private void OnSpawnTriggerBodyEntered(Node2D body)
    {
        SpawnPlatform();
    }

    private void PlatformFall()
    {
        EmitSignal(SignalName.Fall);
        GetTree().CallGroup("platforms", "Fall");
        GetNode<Timer>("PlatformTimer").Start();
        GD.Print("Falling from main");
    }

    private void PlatformStop()
    {
        EmitSignal(SignalName.stopFall);
        GetNode<Timer>("PlatformTimer").Stop();
        GetTree().CallGroup("platforms", "StopFall");
        GD.Print("Stopping fall from main");
    }

    private void SpawnPlatform() {
        Platform platform = PlatformScene.Instantiate<Platform>();

        var platformSpawnLocation = GetNode<PathFollow2D>("PlatformPath/PlatformSpawnLocation");
        platformSpawnLocation.ProgressRatio = GD.Randf();

        platform.Position = platformSpawnLocation.Position;

        GD.Print("Platform Spawned");

        AddChild(platform);

        platforms = PlatformScene;
    }

}
