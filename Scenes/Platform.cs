using Godot;
using System;

public partial class Platform : Node2D
{

    public override void _Ready()
    {
        
    }
    public override void _PhysicsProcess(double delta)
    {
        
    }

    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }
}
