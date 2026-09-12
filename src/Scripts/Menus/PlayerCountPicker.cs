using Godot;
using System;

public partial class PlayerCountPicker : GameMenu
{

	[Export] Button twoPlayerButton;
	[Export] Button threePlayerButton;
	[Export] Button fourPlayerButton;

	public override void _Ready()
	{
		base._Ready();

		twoPlayerButton.Pressed += StartTwoPlayer;
		threePlayerButton.Pressed += StartThreePlayer;
		fourPlayerButton.Pressed += StartFourPlayer;

		twoPlayerButton.GrabFocus();
	}

	private void StartFourPlayer()
	{
		gameInfo.amountOfPlayers = 4;
		sceneManager.ChangeScene("res://Scenes/Menu/CharacterPicker.tscn", SceneManager.TransitionType.MATRIX, "Pick your\ncharacter");
	}

	private void StartThreePlayer()
	{
		gameInfo.amountOfPlayers = 3;
		sceneManager.ChangeScene("res://Scenes/Menu/CharacterPicker.tscn", SceneManager.TransitionType.MATRIX, "Pick your\ncharacter");
	}

	private void StartTwoPlayer()
	{
		gameInfo.amountOfPlayers = 2;
		sceneManager.ChangeScene("res://Scenes/Menu/CharacterPicker.tscn", SceneManager.TransitionType.MATRIX, "Pick your\ncharacter");
	}
}
