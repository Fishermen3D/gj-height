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

	public Level currentLevel;

	PackedScene groundHitScene;
	PackedScene headHitEffect;

	public int playerIndex = 0;

	Node3D modelPivot;

	double okLength = 0.5;
	double okTimer = 0.0;

	public bool sticky = false;

	double stickyModifier = 0.1;

	double stickyTimer = 5.0;
	double stickyTime = 0.0;

	AnimatedSprite3D mark;

	AudioStreamPlayer downSound;

	public static Dictionary<CharacterType, StringName> spriteFrameGroup = new()
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

		groundHitScene = GD.Load<PackedScene>("res://Scenes/Effects/GroundHitEffect.tscn");
		headHitEffect = GD.Load<PackedScene>("res://Scenes/Effects/HeadHitEffect.tscn");

		modelPivot = GetNode<Node3D>("ModelPivot");

		mark = GetNode<AnimatedSprite3D>("Xmark");
		mark.Hide();

		downSound = GetNode<AudioStreamPlayer>("Down");
	}

	public override void _Process(double delta)
	{
		if(currentLevel.currentState == Level.LevelState.COUNTDOWN){ return; }

		if (sticky)
		{
			stickyTime += delta;
			if(stickyTime > stickyTimer)
			{
				sticky = false;
				ResetJump();
				stickyTime = 0.0;
			}
		}

		if (jumpInputOk)
		{
			okTimer += delta;
			if(okTimer >= okLength)
			{
				jumpInputOk = false;
				okTimer = 0.0;
			}
		}

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
				/*baseJumpForce += jumpAddition;
				fallVelocity = baseJumpForce;
				*/
				Jump();
				jumpInputOk = false;
			}
			else
			{
				/*baseJumpForce = jumpForceReset;
				fallVelocity = baseJumpForce;*/
				if (!sticky)
				{
					ResetJump();
				}
				else
				{
					Jump();
				}
				//Jump();
			}

			soundToPlay.PitchScale = (float)random.NextDouble() + 0.5f;
			soundToPlay.Play();

			Node3D groundHitNode = groundHitScene.Instantiate<Node3D>();
			groundHitNode.Position = GlobalPosition;
			currentLevel.AddChild(groundHitNode);
		}
		else
		{
			fallVelocity -= gravity * delta;

			if(fallVelocity < 0.0)
			{
				if (Input.IsActionJustPressed(jumpInput))
				{
					jumpInputOk = true;
					okTimer = 0.0;
				}

				if (floorChecker.IsColliding())
				{
					/*if (Input.IsActionJustPressed(jumpInput))
					{
						jumpInputOk = true;
					}*/
				}
			}

			if (hitRay.IsColliding())
			{
				Node3D objectOfInterest = (Node3D)floorChecker.GetCollider();
				if(objectOfInterest is Player opponent)
				{
					opponent.GetHit();

					fallVelocity = baseJumpForce * 1.05;

					Node3D headHit = headHitEffect.Instantiate<Node3D>();
					headHit.Position = objectOfInterest.GlobalPosition;

					currentLevel.AddChild(headHit);
					hitSound.Play();
				}
			}
		}
	}

	void Jump()
	{
		double mod = sticky ? 0.1 : 1.0;

		baseJumpForce += jumpAddition;
		fallVelocity = baseJumpForce * mod;
		//jumpInputOk = false;
	}

	void ResetJump()
	{
		baseJumpForce = jumpForceReset;
		fallVelocity = baseJumpForce;
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
			currentLevel.CheckPlayerCount();
			mark.Show();
			downSound.Play();
		}
	}

	public void MakeSticky()
	{
		sticky = true;
		Jump();
	}

	public void knockDown()
	{
		fallVelocity = -20;
	}
}
