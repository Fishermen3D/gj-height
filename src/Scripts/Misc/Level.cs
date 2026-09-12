using Godot;
using System;

public partial class Level : GameScene
{

	Vector3 startSpawnPoint = new Vector3(-4.0f, 0.0f, 0.0f);

	Timer winTimer;

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
			startSpawnPoint.X += 2.0f;

			AddChild(player);
		}

		PackedScene playerHealthScene =GD.Load<PackedScene>("res://Scenes/UI/PlayerHealth.tscn");
		HealthManager healthManager = playerHealthScene.Instantiate<HealthManager>();

		AddChild(healthManager);


		GameCamera camera = new GameCamera();
		AddChild(camera);

		winTimer = GetNode<Timer>("WinnerTimer");
		winTimer.Timeout += GoToWinScreen;
	}

	private void GoToWinScreen()
	{
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

			winTimer.Start();
		}
	}

	public override void _Process(double delta)
	{
		
	}
}
