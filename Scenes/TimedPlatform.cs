using Godot;
using System;

public partial class TimedPlatform : StaticBody2D
{
    public override void _Ready()
    {
           
    }

    public void Timeout() 
    {
        QueueFree();
    }
}
