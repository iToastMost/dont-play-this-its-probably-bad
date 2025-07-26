using Godot;
using System;

public partial class OneJumpPlatform : Area2D
{
    public void Hit() 
    {
        QueueFree();
    }
}
