using Godot;
using System;

public partial class SceneManager : Node
{

	Node currentScene;
	public MatrixTransition matrixTransitionRect;

	public enum TransitionType
	{
		INSTANT,
		FADE,
		MATRIX
	}

	public TransitionType currentTransition = TransitionType.INSTANT;

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
			
			case TransitionType.MATRIX:
				currentTransition = transitionType;
				FreezeCurrentScene();

				Image image = GetViewport().GetTexture().GetImage();
				Texture2D texture = ImageTexture.CreateFromImage(image);

				matrixTransitionRect.Reset(texture);
				matrixTransitionRect.Show();
				LoadScene(scenePath);
				SetProcess(true);
				break;
		}
	}

	void FreezeCurrentScene()
	{
		foreach(Node child in GetChildren())
		{
			child.ProcessMode = ProcessModeEnum.Disabled;
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

	public override void _Process(double delta)
	{
		switch (currentTransition)
		{
			case TransitionType.MATRIX:
				if (matrixTransitionRect.UpdateEffect(delta))
				{
					matrixTransitionRect.Hide();
					SetProcess(false);
				}
				break;
		}
	}
}
