using Godot;
using System;

public partial class Main : Node
{
    [Export]
    public PackedScene PlatformScene { get; set; }

    public override void _Ready()
    {
        NewGame();
        var MidPoint = GetNode<Marker2D>("MidPoint").Position;
        var playerPosition = GetNode<Player>("Player").Position;
    }
    public void NewGame()
    {
        var player = GetNode<Player>("Player");
        var startPosition = GetNode<Marker2D>("StartPosition");
        player.Start(startPosition.Position);
    }
}
