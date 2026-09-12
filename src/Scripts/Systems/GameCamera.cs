using Godot;
using System;
using System.Collections.Generic;

public partial class GameCamera : Camera3D
{
	
	List<Player> players = [];

	float followSpeed = 10.0f;

	public override void _Ready()
	{
		foreach(Node child in GetParent().GetChildren())
		{
			if(child is Player player)
			{
				players.Add(player);
			}
		}
	}

	public override void _Process(double delta)
	{
		Vector3 centerPoint = GetCenterPointOfPlayers();
		Vector3 point = new Vector3(centerPoint.X, 5.0f, 12.0f);
		GlobalPosition = GlobalPosition.Lerp(point, (float)delta * followSpeed);
	}

	Vector3 GetCenterPointOfPlayers()
	{
		Vector3 total = new Vector3();
		int totalAmount = 0;

		foreach(Player player in players)
		{
			if(player.currentState == Player.PlayerState.DEAD){ continue; }
			total += player.GlobalPosition;
			totalAmount++;
		}

		return total / totalAmount;
	}

	public void RemovePlayer(Player player)
	{
		players.Remove(player);
	}
}
