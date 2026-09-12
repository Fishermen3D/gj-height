using Godot;
using System;

public partial class ResultMenu : Node3D
{
	
	[Export] Button quitButton;
	[Export] Button rematchButton;

	StringName jumpInput = new StringName("continue");

	public SceneManager sceneManager;
	public GameInfo gameInfo;

	public override void _Ready()
	{
		base._Ready();

		rematchButton.Pressed += Rematch;
		quitButton.Pressed += ToMenu;

		rematchButton.GrabFocus();
	}

	private void ToMenu()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/MainMenu.tscn", SceneManager.TransitionType.MATRIX);
	}

	private void Rematch()
	{
		sceneManager.ChangeScene("res://Scenes/Levels/level.tscn", SceneManager.TransitionType.MATRIX, "One\nmore");
	}

}
