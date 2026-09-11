using Godot;
using System;

public partial class GameMenu : Control
{
	
	public SceneManager sceneManager;

	public override void _Ready()
	{
		SetProcess(false);
	}
}
