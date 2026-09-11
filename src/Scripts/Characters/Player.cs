using Godot;
using System;

public partial class Player : CharacterBody3D
{

	public enum PlayerState
	{
		PLAYING,
		DEAD
	}

	[Export] public string inputPrefix = string.Empty;

	public PlayerState currentState = PlayerState.PLAYING;

	StringName leftInput;
	StringName rightInput;
	StringName jumpInput;

	float moveSpeed = 10.0f;
	double gravity = 20.0f;
	double fallVelocity = 0.0;
	double jumpForce = 10.0;
	
	Label3D label;

	RayCast3D floorChecker;
	AudioStreamPlayer boingSound;

	bool jumpInputOk = false;
	
	double baseJumpForce = 10.0;
	double jumpAddition = 2.0;
	double jumpForceReset = 10.0;

	Random random = new Random();

	public override void _Ready()
	{
		if (string.IsNullOrEmpty(inputPrefix))
		{
			GD.PrintErr("Input prefix has not been set for a player.");
			SetProcess(false);
			return;
		}

		leftInput = new StringName($"{inputPrefix}_left");
		rightInput = new StringName($"{inputPrefix}_right");
		jumpInput = new StringName($"{inputPrefix}_jump");

		label = GetNode<Label3D>("Label3D");
		label.Text = inputPrefix.ToUpper();

		floorChecker = GetNode<RayCast3D>("RayCast3D");
		boingSound = GetNode<AudioStreamPlayer>("Boing");
	}

	public override void _Process(double delta)
	{
		switch (currentState)
		{
			case PlayerState.PLAYING:
				UpdateMovement(delta);
				break;
		}
	}

	public void UpdateMovement(double delta)
	{
		float horizontalInput = Input.GetActionStrength(rightInput) - Input.GetActionStrength(leftInput);
		Vector3 moveDirection = new Vector3(horizontalInput * moveSpeed, (float)fallVelocity, 0.0f);

		Velocity = moveDirection;
		MoveAndSlide();

		if (IsOnFloor())
		{
			if (jumpInputOk)
			{
				baseJumpForce += jumpAddition;
				fallVelocity = baseJumpForce;
				jumpInputOk = false;
			}
			else
			{
				baseJumpForce = jumpForceReset;
				fallVelocity = baseJumpForce;
			}

			boingSound.PitchScale = (float)random.NextDouble() + 0.5f;
			boingSound.Play();

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

			if(fallVelocity < 0.0)
			{
				if (floorChecker.IsColliding())
				{
					//Node objectOfInterest = (Node)floorChecker.GetCollider();
					if (Input.IsActionJustPressed(jumpInput))
					{
						jumpInputOk = true;
					}
				}
			}
		}
	}
}
