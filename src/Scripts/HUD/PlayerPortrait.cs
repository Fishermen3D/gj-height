using Godot;
using System;

public partial class PlayerPortrait : Node
{
	
	public Player.CharacterType characterType = Player.CharacterType.NONE;

	public override void _Ready()
	{
		SetProcess(false);

		switch (characterType)
		{
			case Player.CharacterType.CAT:
				AddTexture("res://Assets/Images/tmp_images/cat/damage4.png");
				AddTexture("res://Assets/Images/tmp_images/cat/damage3.png");
				AddTexture("res://Assets/Images/tmp_images/cat/damage2.png");
				AddTexture("res://Assets/Images/tmp_images/cat/damage1.png");
				AddTexture("res://Assets/Images/tmp_images/Cat.png");
				GD.Print("Added cat portrait");
				break;
			
			case Player.CharacterType.RABBIT:
				AddTexture("res://Assets/Images/tmp_images/rabbit/Damage4.png");
				AddTexture("res://Assets/Images/tmp_images/rabbit/Damage3.png");
				AddTexture("res://Assets/Images/tmp_images/rabbit/Damage2.png");
				AddTexture("res://Assets/Images/tmp_images/rabbit/Damage1.png");
				AddTexture("res://Assets/Images/tmp_images/Rabbit.png");
				GD.Print("Added rabbit portrait");
				break;
			
			case Player.CharacterType.BEAR:
				AddTexture("res://Assets/Images/tmp_images/bear/damage4.png");
				AddTexture("res://Assets/Images/tmp_images/bear/damage3.png");
				AddTexture("res://Assets/Images/tmp_images/bear/damage2.png");
				AddTexture("res://Assets/Images/tmp_images/bear/damage1.png");
				AddTexture("res://Assets/Images/tmp_images/Fox.png");
				GD.Print("Added bear portrait");
				break;
			
			case Player.CharacterType.JELLYFISH:
				AddTexture("res://Assets/Images/tmp_images/Jelly/Damage4.png");
				AddTexture("res://Assets/Images/tmp_images/Jelly/Damage3.png");
				AddTexture("res://Assets/Images/tmp_images/Jelly/Damage2.png");
				AddTexture("res://Assets/Images/tmp_images/Jelly/Damage1.png");
				AddTexture("res://Assets/Images/tmp_images/Jelly.png");
				GD.Print("Added jelly portrait");
				break;
		}

	}

	public Texture2D GetPortrait(int index)
	{
		return GetChild<TextureRect>(index).Texture;
	}

	void AddTexture(string source)
	{
		TextureRect normal = new TextureRect();
		normal.Texture = GD.Load<Texture2D>(source);
		normal.Hide();
		AddChild(normal);
	}
}
