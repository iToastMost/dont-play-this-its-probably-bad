using Godot;
using System;

public partial class Spring : Area2D
{
    public float bounceForce = -1200f;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }
    private void OnBodyEntered(Node body) 
    {
        if(body is Player player) 
        {
            if(player.Velocity.Y > 0) 
            {
                player.Bounce(bounceForce);
            }
        }
    }
}
