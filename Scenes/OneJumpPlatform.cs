using Godot;
using System;

public partial class OneJumpPlatform : Area2D
{
    public override void _Ready()
    {
       
    }

    public void Hit() 
    {
        QueueFree();
    }
}
