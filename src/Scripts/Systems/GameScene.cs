using Godot;
using System;

public partial class GameScene : Node3D
{
	
	public SceneManager sceneManager;

	public override void _Ready()
	{
		SetProcess(false);
	}


	public override void _Process(double delta)
	{
	}
}
