using Godot;
using System;

public partial class Platform : CharacterBody2D
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
        //GD.Print("Platform Freed");
    }
}
