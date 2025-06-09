using Godot;
using System;

public partial class Fall : Area2D
{
	private AudioStreamPlayer2D _fallSound;

	public override void _Ready()
	{
		// Carga y añade el AudioStreamPlayer dinámicamente
		//_fallSound = new AudioStreamPlayer();
		//_fallSound.Stream = GD.Load<AudioStream>("res://ruta/a/tu/sonido/fall.mp3");
		//AddChild(_fallSound);
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player player)
		{
			GameState.Health = 0;
			//_fallSound.Play();
		}
	}
}
