using Godot;
using System;

public partial class Water2 : Area2D
{
	private AudioStreamPlayer2D _waterSound;

	public override void _Ready()
	{
		//Carga y añade el AudioStreamPlayer2D dinámicamente
		_waterSound = new AudioStreamPlayer2D();
		_waterSound.Stream = GD.Load<AudioStream>("res://BSO/water.mp3");
		_waterSound.VolumeDb = 10;
		AddChild(_waterSound);
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			GameState.DiedByFall = true;
			GameState.Health = 0;
			_waterSound.Play();
		}
	}
}
