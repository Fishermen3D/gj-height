using Godot;
using System;

public partial class SpriteEffect : AnimatedSprite3D
{
	
	public override void _Ready()
	{
		SetProcess(false);
		AnimationFinished += Done;
	}

	private void Done()
	{
		QueueFree();
	}
}
