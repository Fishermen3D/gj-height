using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{

	public enum PlayerState
	{
		PLAYING,
		DEAD
	}

	public enum CharacterType
	{
		NONE,
		JELLYFISH,
		CAT,
		BEAR,
		RABBIT
	}

	[Export] public CharacterType characterType = CharacterType.NONE;

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
	RayCast3D hitRay;
	AudioStreamPlayer boingSoundPlayer1;
	AudioStreamPlayer boingSoundPlayer2;
	AudioStreamPlayer soundToPlay;
	AudioStreamPlayer hitSound;

	bool jumpInputOk = false;
	
	double baseJumpForce = 10.0;
	double jumpAddition = 2.0;
	double jumpForceReset = 10.0;

	Random random = new Random();

	public PlayerHelmet helmet;
	public GameCamera camera;

	CollisionShape3D myShape;
	CollisionShape3D hitShape;

	AnimatedSprite3D sprite;
	StringName characterFrames;

	Dictionary<CharacterType, StringName> spriteFrameGroup = new()
	{
		{ CharacterType.NONE, new StringName() },
		{ CharacterType.CAT, new StringName("Cat") },
		{ CharacterType.BEAR, new StringName("Bear") },
		{ CharacterType.RABBIT, new StringName("Rabbit") },
		{ CharacterType.JELLYFISH, new StringName("Jelly") }
	};

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
		hitRay = GetNode<RayCast3D>("HitRay");
		boingSoundPlayer1 = GetNode<AudioStreamPlayer>("BoingPlayerOne");
		boingSoundPlayer2 = GetNode<AudioStreamPlayer>("BoingPlayerTwo");
		hitSound = GetNode<AudioStreamPlayer>("Hit");

		myShape = GetNode<CollisionShape3D>("CollisionShape3D");
		hitShape = GetNode<CollisionShape3D>("Hitbox/CollisionShape3D");

		sprite = GetNode<AnimatedSprite3D>("ModelPivot/AnimatedSprite3D");
		sprite.Play(spriteFrameGroup[characterType]);
		sprite.Stop();

		soundToPlay = boingSoundPlayer1;
	}

	public override void _Process(double delta)
	{
		switch (currentState)
		{
			case PlayerState.PLAYING:
				UpdateMovement(delta);
				break;
			
			case PlayerState.DEAD:
				UpdateFall(delta);
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

			soundToPlay.PitchScale = (float)random.NextDouble() + 0.5f;
			soundToPlay.Play();

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

			if (hitRay.IsColliding())
			{
				Node objectOfInterest = (Node)floorChecker.GetCollider();
				if(objectOfInterest is Player opponent)
				{
					opponent.GetHit();

					fallVelocity = baseJumpForce * 1.2;
				}
			}
		}
	}

	void UpdateFall(double delta)
	{
		Vector3 fallDirection = new Vector3(0.0f, (float)fallVelocity, 0.0f);
		Velocity = fallDirection;
		MoveAndSlide();

		if (!IsOnFloor())
		{
			fallVelocity -= gravity * delta;
		}
		else
		{
			fallVelocity = 0.0;
		}
	}

	void ResetJumpForce()
	{
		baseJumpForce = jumpForceReset;
		fallVelocity = baseJumpForce;
	}

	public void PushDown(float amount = -30.0f)
	{
		hitSound.Play();
		fallVelocity = amount;
	}

	public void GetHit()
	{
		if(currentState == PlayerState.DEAD){ return; }
		
		if (!helmet.TakeDamage())
		{
			PushDown();
		}
		else
		{
			SetState(PlayerState.DEAD);
			//camera.RemovePlayer(this);
		}
	}

	public void SetState(PlayerState state)
	{
		if(currentState == PlayerState.DEAD){ return; }
		currentState = state;

		if(state == PlayerState.DEAD)
		{
			//myShape.Disabled = true;
			hitShape.Disabled = true;
		}
	}
}
