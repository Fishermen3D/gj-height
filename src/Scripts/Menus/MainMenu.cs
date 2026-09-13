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

		quitGameButton.Pressed += QuitGame;

		gameInfo.musicManager.PlayTheme();
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}

	private void StartGame()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/PlayerCountPicker.tscn", SceneManager.TransitionType.MATRIX, "How\nmany?");
	}

	public void RegrabFocus()
	{
		startGameButton.GrabFocus();
	}
}
