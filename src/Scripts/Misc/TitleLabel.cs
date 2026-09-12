using Godot;
using System;

public partial class TitleLabel : CanvasLayer
{
	[Export] Curve slideCurve;
	[Export] Label titleLabel;

	float target;
	float startX;
	float endX;

	public override void _Ready()
	{
		Hide();
	}

	public override void _Process(double delta)
	{
	}

	public void DisplayText(string text)
	{
		target = DisplayServer.WindowGetSize().X;
		titleLabel.OffsetTransformPosition = new Vector2(target, 0.0f);

		startX = target;
		endX = 0;

		titleLabel.Text = text;

		Tween tween = CreateTween();
		tween.TweenMethod(
			Callable.From<float>(UpdatePosition),
			0.0,
			1.0,
			1.5f
		).SetDelay(0.25);

		tween.Finished += HideAgain;
		Show();

	}

	private void HideAgain()
	{
		Hide();
	}

	public void UpdatePosition(float progress)
	{
		float curvedProgress = slideCurve.Sample(progress);
		float x = Mathf.Lerp(startX, endX, curvedProgress);
		titleLabel.OffsetTransformPosition = new Vector2(x, 0.0f);
	}
}
