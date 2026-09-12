using Godot;
using System;

public partial class Level : GameScene
{

	Vector3 startSpawnPoint = new Vector3(-4.0f, 0.0f, 0.0f);

	public override void _Ready()
	{
		PackedScene PlayerScene = GD.Load<PackedScene>("res://Scenes/Characters/Player.tscn");

		for(int i = 0; i < gameInfo.amountOfPlayers; i++)
		{
			Player player = PlayerScene.Instantiate<Player>();
			player.inputPrefix = $"p{i+1}";
			player.characterType = gameInfo.characterTypes[i];

			player.Position = startSpawnPoint;
			startSpawnPoint.X += 2.0f;

			AddChild(player);
		}

		PackedScene playerHealthScene =GD.Load<PackedScene>("res://Scenes/UI/PlayerHealth.tscn");
		HealthManager healthManager = playerHealthScene.Instantiate<HealthManager>();

		AddChild(healthManager);


		GameCamera camera = new GameCamera();
		AddChild(camera);
	}

	public override void _Process(double delta)
	{
	}
}
