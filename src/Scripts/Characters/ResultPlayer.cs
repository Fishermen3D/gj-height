using Godot;
using System;

public partial class ResultPlayer : CharacterBody3D
{
	
	Sprite3D sprite;

	double baseJumpForce = 10.0;
	double jumpAddition = 2.0;
	double jumpForceReset = 10.0;

	double gravity = 20.0f;
	double fallVelocity = 0.0;
	double jumpForce = 10.0;

	AudioStreamPlayer soundToPlay;

	Random random = new Random();

	public override void _Ready()
	{
		soundToPlay = GetNode<AudioStreamPlayer>("Twang");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector3 moveDirection = new Vector3(0.0f, (float)fallVelocity, 0.0f);

		Velocity = moveDirection;
		MoveAndSlide();

		if (IsOnFloor())
		{

			soundToPlay.PitchScale = (float)random.NextDouble() + 0.5f;
			soundToPlay.Play();

			fallVelocity = baseJumpForce;

			/*if (Input.IsActionJustPressed(jumpInput))
			{
				fallVelocity = jumpForce;
				return;
			}*/

			//fallVelocity = 0.0;
		}
		else
		{
			fallVelocity -= gravity * delta;
		}
	}

	public void SetPlayer(Player.CharacterType type)
	{
		
	}
}
