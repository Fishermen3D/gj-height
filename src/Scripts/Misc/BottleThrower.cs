using Godot;
using System;

public partial class BottleThrower : Node3D
{
	[Export] int throwDirection = 1;

	Timer throwTimer;

	PackedScene bottleScene;

	Random random = new Random();

	public override void _Ready()
	{
		throwTimer = GetNode<Timer>("Timer");
		throwTimer.Timeout += ThrowBottle;

		bottleScene = GD.Load<PackedScene>("res://Scenes/Objects/Bottle.tscn");

		throwTimer.WaitTime = random.Next(2, 8);
		throwTimer.CallDeferred("start");
	}

	private void ThrowBottle()
	{
		Bottle bottle = bottleScene.Instantiate<Bottle>();
		bottle.Position = GlobalPosition;
		bottle.moveDirection = throwDirection;

		GetParent().AddChild(bottle);
		
		throwTimer.WaitTime = random.Next(2, 8);
		throwTimer.CallDeferred("start");
	}

	public override void _Process(double delta)
	{
		
	}
}
