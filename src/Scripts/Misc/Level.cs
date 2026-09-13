using Godot;
using System;

public partial class Level : GameScene
{
	public enum LevelState
	{
		COUNTDOWN,
		PLAYING
	}

	public LevelState currentState = LevelState.COUNTDOWN;

	Vector3 startSpawnPoint = new Vector3(-4.0f, 1.0f, 0.0f);

	Timer winTimer;

	AudioStreamPlayer boomSound;

	Label3D matchOverLabel;

	GameCamera camera;
	HealthManager healthManager;

	Label countDownLabel;

	AudioStreamPlayer hihatSound;

	double countTimer = 1.0;
	int currentCount = 3;

	public AudioStreamPlayer glassSound;

	public override void _Ready()
	{
		PackedScene PlayerScene = GD.Load<PackedScene>("res://Scenes/Characters/Player.tscn");

		for(int i = 0; i < gameInfo.amountOfPlayers; i++)
		{
			Player player = PlayerScene.Instantiate<Player>();
			player.inputPrefix = $"p{i+1}";
			player.characterType = gameInfo.characterTypes[i];
			player.currentLevel = this;

			player.Position = startSpawnPoint;
			player.playerIndex = i+1;
			startSpawnPoint.X += 2.0f;

			AddChild(player);
		}

		PackedScene playerHealthScene = GD.Load<PackedScene>("res://Scenes/UI/PlayerHealth.tscn");
		healthManager = playerHealthScene.Instantiate<HealthManager>();

		AddChild(healthManager);


		camera = new GameCamera();
		AddChild(camera);

		winTimer = GetNode<Timer>("WinnerTimer");
		winTimer.Timeout += GoToWinScreen;

		boomSound = GetNode<AudioStreamPlayer>("Boom");
		matchOverLabel = GetNode<Label3D>("MatchOverLabel");
		matchOverLabel.Hide();

		countDownLabel = GetNode<Label>("CanvasLayer/CountDownLabel");
		countDownLabel.Text = "3";

		gameInfo.musicManager.StopSong();

		hihatSound = GetNode<AudioStreamPlayer>("Hihat");

		glassSound = GetNode<AudioStreamPlayer>("Glass");
	}

	private void GoToWinScreen()
	{
		gameInfo.musicManager.PlayResults();
		sceneManager.ChangeScene("res://Scenes/Menu/ResultMenu.tscn", SceneManager.TransitionType.MATRIX, "And the\nwinner is.");
	}

	public void CheckPlayerCount()
	{
		int count = 0;
		foreach(Node child in GetChildren())
		{
			if(child is Player player)
			{
				if(player.currentState != Player.PlayerState.DEAD)
				{
					count++;
				}
			}
		}

		if(count == 1)
		{
			foreach(Node child in GetChildren())
			{
				if(child.ProcessMode != ProcessModeEnum.Always)
				{
					child.ProcessMode = ProcessModeEnum.Disabled;
				}
			}

			Player winner;
			foreach(Node child in GetChildren())
			{
				if(child is Player player)
				{
					if(player.currentState != Player.PlayerState.DEAD)
					{
						winner = player;
						gameInfo.winnerPlayerIndex = winner.playerIndex;
						gameInfo.winnerType = winner.characterType;
						
						break;
					}
				}
			}

			boomSound.Play();
			winTimer.Start();
			gameInfo.musicManager.StopSong();

			//matchOverLabel.GlobalPosition = new Vector3(camera.GlobalPosition.X, matchOverLabel.GlobalPosition.Y, matchOverLabel.GlobalPosition.Z);
			matchOverLabel.Show();
			healthManager.Hide();
		}
	}

	public override void _Process(double delta)
	{
		countTimer -= delta;
		if(countTimer <= 0.0)
		{
			currentCount--;
			countDownLabel.Text = $"{currentCount}";
			hihatSound.Play();

			if(currentCount == 0)
			{
				gameInfo.musicManager.PlayLevel();
				currentState = LevelState.PLAYING;
				countDownLabel.Text = "GO!";

				foreach(Node child in GetChildren())
				{
					if(child is Puffin puffin)
					{
						puffin.spitTimer.Start();
					}
				}
			}

			if(currentCount == -1)
			{
				countDownLabel.Hide();
				
				
				SetProcess(false);
			}
			countTimer = 1;
		}
	}
}
