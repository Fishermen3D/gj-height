using Godot;
using System;

public partial class Bottle : CharacterBody3D
{
	public int moveDirection = 1;
	float moveSpeed = 10.0f;

	double gravity = 10.0;
	double fallVelocity = 0.0;

	Sprite3D sprite;

	Area3D hitbox;

	Random random = new Random();

	public override void _Ready()
	{
		sprite = GetNode<Sprite3D>("Sprite3D");
		hitbox = GetNode<Area3D>("Area3D");
		hitbox.BodyEntered += CheckCollision;

		moveSpeed = random.Next(10, 20);
	}

	private void CheckCollision(Node3D body)
	{
		if(body is Player player)
		{
			player.knockDown();

			PackedScene breakScene = GD.Load<PackedScene>("res://Scenes/Effects/HeadHitEffect.tscn");
			Node3D effect = breakScene.Instantiate<Node3D>();
			effect.Position = new Vector3(GlobalPosition.X, GlobalPosition.Y, GlobalPosition.Z);

			GetParent().AddChild(effect);

			QueueFree();
		}
	}

	public override void _Process(double delta)
	{
		fallVelocity -= gravity * delta;
		Vector3 direction = new Vector3(moveDirection * moveSpeed, (float)fallVelocity, 0.0f);

		sprite.RotateZ(20 * (float)delta);

		Velocity = direction;
		MoveAndSlide();

		if (IsOnFloor())
		{
			PackedScene breakScene = GD.Load<PackedScene>("res://Scenes/Effects/BottleBreak.tscn");
			Node3D effect = breakScene.Instantiate<Node3D>();
			effect.Position = new Vector3(GlobalPosition.X, GlobalPosition.Y + 0.5f, GlobalPosition.Z);

			GetParent().AddChild(effect);

			QueueFree();
		}
	}
}
