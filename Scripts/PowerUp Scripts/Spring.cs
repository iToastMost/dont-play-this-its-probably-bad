using Godot;
using System;

public partial class Spring : Area2D
{
    public override void _Ready()
    {
        
    }
    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
        GD.Print("Spring Freed");
    }

}
