using Godot;
using System;

public partial class PlayerHitbox : Area3D
{
	
	Player player;

	public override void _Ready()
	{
		player = GetParent<Player>();
	}

	public void GetHit()
	{
		player.PushDown();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
