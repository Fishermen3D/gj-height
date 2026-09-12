using Godot;
using System;

public partial class MainMenu : GameMenu
{

	[Export] Button startGameButton;
	[Export] Button quitGameButton;

	public override void _Ready()
	{
		base._Ready();

		startGameButton.Pressed += StartGame;
		startGameButton.GrabFocus();
		startGameButton.GrabClickFocus();

		quitGameButton.Pressed += QuitGame;
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}

	private void StartGame()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/PlayerCountPicker.tscn", SceneManager.TransitionType.MATRIX, "How\nmany?");
	}
}
