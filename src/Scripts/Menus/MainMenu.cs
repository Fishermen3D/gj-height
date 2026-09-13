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
		startGameButton.CallDeferred("grab_focus");

		quitGameButton.Pressed += QuitGame;

		gameInfo.musicManager.PlayTheme();
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}

	private void StartGame()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/HowToPlayMenu.tscn", SceneManager.TransitionType.MATRIX, "How to\nplay");
	}

	public void RegrabFocus()
	{
		startGameButton.GrabFocus();
	}
}
