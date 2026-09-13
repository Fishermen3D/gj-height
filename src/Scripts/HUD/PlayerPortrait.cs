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
				AddTexture("res://Assets/Images/HP/reytt/HP_cat5.png");
				AddTexture("res://Assets/Images/HP/reytt/HP_cat4.png");
				AddTexture("res://Assets/Images/HP/reytt/HP_cat3.png");
				AddTexture("res://Assets/Images/HP/reytt/HP_cat2.png");
				AddTexture("res://Assets/Images/HP/reytt/HP_cat1.png");
				break;
			
			case Player.CharacterType.RABBIT:
				AddTexture("res://Assets/Images/HP/Pink/HP_kanin5.png");
				AddTexture("res://Assets/Images/HP/Pink/HP_kanin4.png");
				AddTexture("res://Assets/Images/HP/Pink/HP_kanin3.png");
				AddTexture("res://Assets/Images/HP/Pink/HP_kanin2.png");
				AddTexture("res://Assets/Images/HP/Pink/HP_kanin1.png");
				break;
			
			case Player.CharacterType.BEAR:
				AddTexture("res://Assets/Images/HP/Turkis/HP_bjorn5.png");
				AddTexture("res://Assets/Images/HP/Turkis/HP_bjorn4.png");
				AddTexture("res://Assets/Images/HP/Turkis/HP_bjorn3.png");
				AddTexture("res://Assets/Images/HP/Turkis/HP_bjorn2.png");
				AddTexture("res://Assets/Images/HP/Turkis/HP_bjorn1.png");
				break;
			
			case Player.CharacterType.JELLYFISH:
				AddTexture("res://Assets/Images/HP/Lilla/HP_jelly5.png");
				AddTexture("res://Assets/Images/HP/Lilla/HP_jelly4.png");
				AddTexture("res://Assets/Images/HP/Lilla/HP_jelly3.png");
				AddTexture("res://Assets/Images/HP/Lilla/HP_jelly2.png");
				AddTexture("res://Assets/Images/HP/Lilla/HP_jelly1.png");
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
