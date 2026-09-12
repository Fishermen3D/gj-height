using Godot;
using System;

public partial class CharacterPicker : GameMenu
{

	Button startMatchButton;
	Button returnButton;

	public override void _Ready()
	{
		base._Ready();

		startMatchButton = GetNode<Button>("StartMatch");
		returnButton = GetNode<Button>("Back");

		startMatchButton.Pressed += StartMatch;
		returnButton.Pressed += ReturnMenu;
	}

	private void ReturnMenu()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/MainMenu.tscn", SceneManager.TransitionType.MATRIX);
	}

	private void StartMatch()
	{
		sceneManager.ChangeScene("res://Scenes/Levels/level.tscn", SceneManager.TransitionType.MATRIX, "Get\nReady!");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
