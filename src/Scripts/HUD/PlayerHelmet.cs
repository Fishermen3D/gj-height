using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerHelmet : Panel
{
	
	int health = 5;
	public int playerIndex = 0;

	Label playerNameLabel;

	TextureRect portrait;

	Dictionary<Player.CharacterType, string> CharacterPortraits = new()
	{
		{ Player.CharacterType.CAT, "res://Assets/Images/tmp_images/Cat.png" },
		{ Player.CharacterType.BEAR, "res://Assets/Images/tmp_images/Fox.png" },
		{ Player.CharacterType.RABBIT, "res://Assets/Images/tmp_images/Rabbit.png" },
		{ Player.CharacterType.JELLYFISH, "res://Assets/Images/tmp_images/Jelly.png" }
	};


	public override void _Ready()
	{
		SetProcess(false);
		playerNameLabel = GetNode<Label>("HBoxContainer/Label");
		playerNameLabel.Text = $"Player {playerIndex}";
	}

	public void SetCharacter(Player.CharacterType character)
	{
		if(portrait == null)
		{
			portrait = GetNode<TextureRect>("HBoxContainer/TextureRect");
		}

		if(!string.IsNullOrEmpty(CharacterPortraits.GetValueOrDefault(character, "")))
		{
			Texture2D portraitTexture = GD.Load<Texture2D>(CharacterPortraits.GetValueOrDefault(character, ""));
			portrait.Texture = portraitTexture;
		}

	}

	public bool TakeDamage()
	{
		if(health - 1 <= 0)
		{
			health = 0;
			playerNameLabel.Text = "Player (0)";
			return true;
		}
		else
		{
			health--;
		}

		playerNameLabel.Text = $"Player ({health})";

		return false;
	}
}
