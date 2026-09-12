using Godot;
using System;

public partial class HealthManager : CanvasLayer
{
	
	HBoxContainer helmetContainer;

	public override void _Ready()
	{
		SetProcess(false);

		helmetContainer = GetNode<HBoxContainer>("Control/MarginContainer/HBoxContainer");

		foreach(Node child in GetParent().GetChildren())
		{
			if(child is Player player)
			{
				PackedScene playerHelmetScene = GD.Load<PackedScene>("res://Scenes/UI/PlayerHelmet.tscn");
				PlayerHelmet helmet = playerHelmetScene.Instantiate<PlayerHelmet>();
				player.helmet = helmet;
				helmet.SetCharacter(player.characterType);

				helmetContainer.AddChild(helmet);
			}
		}
	}
}
