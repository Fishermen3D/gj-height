using System.Collections.Generic;
using Godot;

public partial class GameInfo : Node3D
{

	public static SceneManager sceneManager;

	public int amountOfPlayers = 2;

	public List<Player.CharacterType> characterTypes = [];

	public override void _Ready()
	{
		SetProcess(false);

		sceneManager = new SceneManager
		{
			Name = "SceneManager"
		};
		
		sceneManager.gameInfo = this;

		AddChild(sceneManager);

		foreach(Node child in GetTree().Root.GetChildren())
		{
			if(child is not GameInfo)
			{
				if(child is GameMenu menu)
				{
					menu.sceneManager = sceneManager;
				}

				if(child is GameScene scene)
				{
					scene.sceneManager = sceneManager;

					PackedScene pauseScene = GD.Load<PackedScene>("res://Scenes/Menu/PauseMenu.tscn");
					PauseMenu pauseMenu = pauseScene.Instantiate<PauseMenu>();
					pauseMenu.gameplayNode = scene;
					pauseMenu.sceneManager = sceneManager;

					sceneManager.AddChild(pauseMenu);
				}

				child.CallDeferred("reparent", sceneManager);
			}
		}

		CanvasLayer transitionLayer = new CanvasLayer();
		transitionLayer.Layer = 100;

		PackedScene matricTransitionScene = GD.Load<PackedScene>("res://Scenes/UI/TransitionEffect.tscn");
		MatrixTransition matrixEffect = matricTransitionScene.Instantiate<MatrixTransition>();
		sceneManager.matrixTransitionRect = matrixEffect;

		transitionLayer.AddChild(matrixEffect);

		AddChild(transitionLayer);

	}

	public static SceneManager GetSceneManager()
	{
		return sceneManager;
	}

	public override void _Process(double delta)
	{
		
	}
}
