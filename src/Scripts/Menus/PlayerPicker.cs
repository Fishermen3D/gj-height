using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerPicker : Panel
{
	public string inputPrefix = string.Empty;
	public CharacterPicker pickerMenu;

	StringName leftInput;
	StringName rightInput;
	StringName selectInput;

	int currentCharacter = 0;

	public bool isReady = false;

	Label readyLabel;

	TextureRect portrait;

	public int playerIndex = 0;

	Player.CharacterType selectedCharacter = Player.CharacterType.NONE;

	AudioStreamPlayer readySound;
	AudioStreamPlayer changeSound;

	Label nameLabel;

	Dictionary<int, Player.CharacterType> portraitIndex = new()
	{
		{0, Player.CharacterType.CAT},
		{1, Player.CharacterType.BEAR},
		{2, Player.CharacterType.RABBIT},
		{3, Player.CharacterType.JELLYFISH}
	};

	public override void _Ready()
	{
		leftInput = new StringName($"{inputPrefix}_left");
		rightInput = new StringName($"{inputPrefix}_right");
		selectInput = new StringName($"{inputPrefix}_jump");

		nameLabel = GetNode<Label>("VBoxContainer/Label");
		nameLabel.Text = $"Player {playerIndex + 1}";

		readyLabel = GetNode<Label>("VBoxContainer/ReadyLabel");
		readyLabel.Text = string.Empty;

		portrait = GetNode<TextureRect>("VBoxContainer/TextureRect");
		currentCharacter = playerIndex;
		portrait.Texture = pickerMenu.GetPortrait(portraitIndex[playerIndex]);
		
		readySound = GetNode<AudioStreamPlayer>("Sting");
		changeSound = GetNode<AudioStreamPlayer>("Change");
	}

	public override void _Process(double delta)
	{
		if (!isReady)
		{
			if (Input.IsActionJustPressed(leftInput))
			{
				if(currentCharacter - 1 < 0)
				{
					currentCharacter = 3;
				}
				else
				{
					currentCharacter--;
				}

				portrait.Texture = pickerMenu.GetPortrait(portraitIndex[currentCharacter]);
				changeSound.Play();
			}

			if (Input.IsActionJustPressed(rightInput))
			{
				if(currentCharacter + 1 > 3)
				{
					currentCharacter = 0;
				}
				else
				{
					currentCharacter++;
				}

				portrait.Texture = pickerMenu.GetPortrait(portraitIndex[currentCharacter]);
				changeSound.Play();
			}

			if (Input.IsActionJustPressed(selectInput))
			{
				pickerMenu.PlayerIsready();
				isReady = true;
				readyLabel.Text = "Ready!";
				selectedCharacter = portraitIndex[currentCharacter];
				readySound.Play();
			}
		}
	}

	public void forceStart()
	{
		pickerMenu.PlayerIsready();
		isReady = true;
		readyLabel.Text = "Ready!";
		selectedCharacter = portraitIndex[currentCharacter];
	}

	public Player.CharacterType GetPickedType()
	{
		return portraitIndex[currentCharacter];
	}
}
