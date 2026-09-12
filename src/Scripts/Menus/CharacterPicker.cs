using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterPicker : GameMenu
{

	Button startMatchButton;
	Button returnButton;

	Timer timer;

	HBoxContainer pickerContainer;

	int readyCount = 0;

	public Dictionary<Player.CharacterType, Texture2D> portraits = new();

	StringName testStartInput = new StringName("start_debug");

	public override void _Ready()
	{
		base._Ready();
		SetProcess(true);

		startMatchButton = GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/StartMatch");
		returnButton = GetNode<Button>("MarginContainer/VBoxContainer/VBoxContainer/Back");

		pickerContainer = GetNode<HBoxContainer>("MarginContainer/VBoxContainer/HBoxContainer");

		startMatchButton.Pressed += StartMatch;
		returnButton.Pressed += ReturnMenu;

		portraits.Add(Player.CharacterType.CAT, GD.Load<Texture2D>("res://Assets/Images/Characters/Cat.PNG"));
		portraits.Add(Player.CharacterType.BEAR, GD.Load<Texture2D>("res://Assets/Images/Characters/Bear.PNG"));
		portraits.Add(Player.CharacterType.RABBIT, GD.Load<Texture2D>("res://Assets/Images/Characters/Rabbit.PNG"));
		portraits.Add(Player.CharacterType.JELLYFISH, GD.Load<Texture2D>("res://Assets/Images/Characters/Jelly.PNG"));

		PackedScene playerPickerScene = GD.Load<PackedScene>("res://Scenes/Menu/PlayerPicker.tscn");

		for(int i = 0; i < gameInfo.amountOfPlayers; i++)
		{
			PlayerPicker picker = playerPickerScene.Instantiate<PlayerPicker>();
			picker.inputPrefix = $"p{i+1}";
			picker.pickerMenu = this;
			picker.playerIndex = i;

			pickerContainer.AddChild(picker);
		}

		timer = GetNode<Timer>("DelayTimer");
		timer.Timeout += StartMatch;

		startMatchButton.GrabFocus();

		gameInfo.characterTypes.Clear();
	}

	private void ReturnMenu()
	{
		sceneManager.ChangeScene("res://Scenes/Menu/MainMenu.tscn", SceneManager.TransitionType.MATRIX);
	}

	private void StartMatch()
	{
		sceneManager.ChangeScene("res://Scenes/Levels/level.tscn", SceneManager.TransitionType.MATRIX, "Get\nReady!");
	}

	public void PlayerIsready()
	{
		readyCount++;
		if(readyCount == gameInfo.amountOfPlayers)
		{
			timer.Start();
			
			foreach(Node child in pickerContainer.GetChildren())
			{
				if(child is PlayerPicker picker)
				{
					gameInfo.characterTypes.Add(picker.GetPickedType());
				}
			}

			//sceneManager.ChangeScene("res://Scenes/Levels/level.tscn", SceneManager.TransitionType.MATRIX, "Get\nReady!");
		}
	}

	public Texture2D GetPortrait(Player.CharacterType type)
	{
		return portraits[type];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed(testStartInput))
		{
			foreach(Node child in pickerContainer.GetChildren())
			{
				if(child is PlayerPicker picker)
				{
					picker.forceStart();
				}
			}
		}
	}
}
