using Godot;
using System;

public partial class MainMenu : GameMenu
{

	[Export] Button startGameButton;

	public override void _Ready()
	{
		base._Ready();

		startGameButton.Pressed += StartGame;
	}

	private void StartGame()
	{
		sceneManager.ChangeScene("res://Scenes/Levels/level.tscn", SceneManager.TransitionType.MATRIX);
	}
}
