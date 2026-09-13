using Godot;
using System;

public partial class MusicManager : Node
{
	[Export] AudioStreamPlayer currentSong;

	public override void _Ready()
	{
		
	}

	public override void _Process(double delta)
	{
	}

	public void PlayTheme()
	{
		AudioStream song = GD.Load<AudioStream>("res://Assets/Audio/Music/Theme.wav");
		currentSong.Stream = song;
		currentSong.Play();
	}

	public void PlayLevel()
	{
		AudioStream song = GD.Load<AudioStream>("res://Assets/Audio/Music/Level.wav");
		currentSong.Stream = song;
		currentSong.Play();
	}

	public void PlayResults()
	{
		AudioStream song = GD.Load<AudioStream>("res://Assets/Audio/Music/Results.wav");
		currentSong.Stream = song;
		currentSong.Play();
	}

	public void StopSong()
	{
		currentSong.Stop();
	}
}
