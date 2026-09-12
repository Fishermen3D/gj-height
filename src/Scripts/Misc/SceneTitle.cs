using Godot;
using System;

public partial class SceneTitle : Node2D
{
	
	[Export] Curve slideCurve;

	public string title = string.Empty;
	public string sceneToLoad = string.Empty;
	public SceneManager sceneManager;

	Label label;

	float target;
	float startX;
	float endX;

	public override void _Ready()
	{
		label = GetNode<Label>("CanvasLayer/SceneTitle/Label");
		label.Text = title;

		target = DisplayServer.WindowGetSize().X;
		label.OffsetTransformPosition = new Vector2(target, 0.0f);

		startX = target;
		endX = -target;

		Tween tween = CreateTween();
		tween.TweenMethod(
			Callable.From<float>(UpdatePosition),
			0.0,
			1.0,
			1.5f
		).SetDelay(0.25);

		tween.Parallel().TweenCallback(Callable.From(ChangeScene)).SetDelay(1.7f);

		tween.Finished += ChangeScene;
		
	}

	public void UpdatePosition(float progress)
	{
		float curvedProgress = slideCurve.Sample(progress);
		float x = Mathf.Lerp(startX, endX, curvedProgress);
		label.OffsetTransformPosition = new Vector2(x, 0.0f);
	}

	private void ChangeScene()
	{
		sceneManager.ChangeScene(sceneToLoad, SceneManager.TransitionType.MATRIX);
	}
}
