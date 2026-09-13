using Godot;
using System;
using System.Collections.Generic;

public partial class HealthManager : CanvasLayer
{
	
	HBoxContainer helmetContainer;

	List<PlayerPortrait> portraits = [];
	List<Player.CharacterType> types = [];

	public override void _Ready()
	{
		SetProcess(false);

		helmetContainer = GetNode<HBoxContainer>("Control/MarginContainer/HBoxContainer");

		int index = 0;

		foreach(Node child in GetParent().GetChildren())
		{
			if(child is Player player)
			{
				index++;

				PackedScene playerHelmetScene = GD.Load<PackedScene>("res://Scenes/UI/PlayerHelmet.tscn");
				PlayerHelmet helmet = playerHelmetScene.Instantiate<PlayerHelmet>();
				player.helmet = helmet;
				//helmet.SetCharacter(player.characterType);
				helmet.playerIndex = index;
				helmet.characterType = player.characterType;

				helmetContainer.AddChild(helmet);

				/*PlayerPortrait portrait = new PlayerPortrait();
				portrait.characterType = player.characterType;

				AddChild(portrait);

				helmet.portraitGroup = portrait;*/
			}
		}
	}
}
