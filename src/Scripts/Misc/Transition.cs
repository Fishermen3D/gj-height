using Godot;
using System;

public partial class Transition : TextureRect
{

	public ShaderMaterial shader;

	public double progress = 0.0;
	public double step = 0.2;

	public virtual bool UpdateEffect(double delta){ return false; }
	public virtual void Reset(Texture2D texture = null){}

	public override void _Ready()
	{
		SetProcess(false);
		shader = Material as ShaderMaterial;
		Hide();
	}

	public override void _Process(double delta)
	{
	}
}
