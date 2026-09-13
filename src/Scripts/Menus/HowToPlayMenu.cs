using Godot;
using System;

public partial class HowToPlayMenu : GameMenu
{
	[Export] Button nextButton;
	[Export] Button returnButton;

	public override void _Ready()
	{
		base._Ready();

		nextButton.Pressed += GoToCountPicker;
		returnButton.Pressed += GoToMenu;

		nextButton.CallDeferred("grab_focus");
	}

	private void GoToMenu()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/MainMenu.tscn", SceneManager.TransitionType.MATRIX);
	}

	private void GoToCountPicker()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/PlayerCountPicker.tscn", SceneManager.TransitionType.MATRIX, "How many\nPlayers?");
	}

	public override void _Process(double delta)
	{
	}
}
