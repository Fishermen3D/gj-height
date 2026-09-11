using Godot;
using System;

public partial class SceneManager : Node
{

	Node currentScene;

	public enum TransitionType
	{
		INSTANT,
		FADE
	}

	public override void _Ready()
	{
		SetProcess(false);
	}

	public static bool isTrue()
	{
		return true;
	}

	public void ChangeScene(string scenePath, TransitionType transitionType)
	{
		switch (transitionType)
		{
			case TransitionType.INSTANT:
				LoadScene(scenePath);
				break;
		}
	}

	void LoadScene(string scenePath)
	{
		foreach(Node child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}

		PackedScene scene = GD.Load<PackedScene>(scenePath);
		Node sceneNode = scene.Instantiate();

		if(sceneNode is GameMenu menuScene)
		{
			menuScene.sceneManager = this;
		}

		if(sceneNode is GameScene gameScene)
		{
			gameScene.sceneManager = this;

			PackedScene pauseScene = GD.Load<PackedScene>("res://Scenes/Menu/PauseMenu.tscn");
			PauseMenu menu = pauseScene.Instantiate<PauseMenu>();
			menu.gameplayNode = gameScene;
			menu.sceneManager = this;

			AddChild(menu);
		}

		AddChild(sceneNode);
	}
}
