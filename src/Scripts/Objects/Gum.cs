using Godot;
using System;

public partial class Gum : CharacterBody3D
{

	double gravity = 10.0;
	double fallVelocity = 0.0;
	double step = 0.2;

	float moveSpeed = 10.0f;

	public enum GumState
	{
		FLYING,
		LANDED
	}

	GumState currentState = GumState.FLYING;

	Random random = new Random();

	public override void _Ready()
	{
		moveSpeed = random.Next(5, 20);
	}

	public override void _Process(double delta)
	{
		switch (currentState)
		{
			case GumState.FLYING:
				UpdateFall(delta);
				break;
		}
	}

	void UpdateFall(double delta)
	{
		Vector3 direction = new Vector3(moveSpeed, (float)fallVelocity, 0.0f);

		Velocity = direction;
		MoveAndSlide();

		if (!IsOnFloor())
		{
			fallVelocity -= gravity * delta;
		}
		else
		{
			fallVelocity = 0.0;
			currentState = GumState.LANDED;
		}
	}
}
