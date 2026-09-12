using Godot;
using System;

public partial class MatrixTransition : Transition
{
	
	public override void _Ready()
	{
		base._Ready();
		step = 1.5;
	}

	public override bool UpdateEffect(double delta)
	{
		progress -= step * delta;
		shader.SetShaderParameter("animation_progress", (float)progress);

		if(progress <= 0.0)
		{
			return true;
		}

		return false;
	}

	public override void Reset(Texture2D texture = null)
	{
		progress = 1.0;
		shader.SetShaderParameter("animation_progress", (float)progress);

		if(texture != null)
		{
			Texture = texture;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
