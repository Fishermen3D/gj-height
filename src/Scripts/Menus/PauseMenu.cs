using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
	
	[Export] Button quitGameButton;

	public Node gameplayNode;
	public SceneManager sceneManager;


	StringName pauseInput = new StringName("p1_pause");

	public override void _Ready()
	{
		Hide();

		quitGameButton.Pressed += ReturnToMenu;
	}

	private void ReturnToMenu()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/MainMenu.tscn", SceneManager.TransitionType.INSTANT);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(pauseInput))
		{
			if (!Visible)
			{
				Show();
				gameplayNode.ProcessMode = ProcessModeEnum.Disabled;
			}
			else
			{
				Hide();
				gameplayNode.ProcessMode = ProcessModeEnum.Inherit;
			}
		}
	}
}
