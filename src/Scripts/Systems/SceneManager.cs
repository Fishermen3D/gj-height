using Godot;
using System;

public partial class SceneManager : Node
{

	Node currentScene;
	public MatrixTransition matrixTransitionRect;
	public GameInfo gameInfo;

	public enum TransitionType
	{
		INSTANT,
		FADE,
		MATRIX
	}

	public TransitionType currentTransition = TransitionType.INSTANT;

	//public TitleLabel titleLabel;

	public override void _Ready()
	{
		SetProcess(false);
	}

	public static bool isTrue()
	{
		return true;
	}

	public void ChangeScene(string scenePath, TransitionType transitionType, string message = null)
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
				

				LoadScene(scenePath, message);
				SetProcess(true);
				
				break;
		}
	}

	void FreezeCurrentScene()
	{
		foreach(Node child in GetChildren())
		{
			if(child is SceneTitle){ continue; }

			child.ProcessMode = ProcessModeEnum.Disabled;
		}
	}

	void LoadScene(string scenePath, string message = null)
	{
		foreach(Node child in GetChildren())
		{
			RemoveChild(child);
			child.QueueFree();
		}

		var newScene = message == null ? scenePath : "res://Scenes/Menu/SceneTitle.tscn";

		PackedScene scene = GD.Load<PackedScene>(newScene);
		Node sceneNode = scene.Instantiate();

		if(sceneNode is SceneTitle sceneTitle)
		{
			sceneTitle.title = message;
			sceneTitle.sceneToLoad = scenePath;
			sceneTitle.sceneManager = this;
		}

		/*if (!string.IsNullOrEmpty(message))
		{
			titleLabel.DisplayText(message);
		}*/

		if(sceneNode is GameMenu menuScene)
		{
			menuScene.sceneManager = this;
			menuScene.gameInfo = gameInfo;
		}

		if(sceneNode is ResultMenu resultScene)
		{
			resultScene.sceneManager = this;
			resultScene.gameInfo = gameInfo;
		}

		if(sceneNode is GameScene gameScene)
		{
			gameScene.sceneManager = this;
			gameScene.gameInfo = gameInfo;

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
