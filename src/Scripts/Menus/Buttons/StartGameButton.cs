using Godot;
using System;

public partial class StartGameButton : Button
{
	
	public override void _Ready()
	{
		SetProcess(false);
		Pressed += StartGame;
	}

	private void StartGame()
	{
		
	}
}
