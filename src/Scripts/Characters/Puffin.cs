using Godot;
using System;

public partial class Puffin : Node3D
{
	[Export] AnimatedSprite3D sprite;
	[Export] public Timer spitTimer;
	[Export] AudioStreamPlayer spitSound;
	[Export] Node3D spitPoint;

	Random random = new Random();

	PackedScene gumScene;

	Level currentLevel;

	public override void _Ready()
	{
		spitTimer.WaitTime = random.Next(1, 5);
		spitTimer.Start();
		spitTimer.Timeout += Spit;

		sprite.AnimationFinished += ResetAnimation;

		gumScene = GD.Load<PackedScene>("res://Scenes/Objects/Gum.tscn");

		currentLevel = GetParent<Level>();
	}

	private void ResetAnimation()
	{
		sprite.Stop();
	}

	private void Spit()
	{
		if(currentLevel.currentState != Level.LevelState.PLAYING){ return; }

		spitSound.Play();

		spitTimer.WaitTime = random.Next(1, 5);
		spitTimer.Start();

		sprite.Play();

		Gum gum = gumScene.Instantiate<Gum>();
		gum.Position = spitPoint.GlobalPosition;

		GetParent().AddChild(gum);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
