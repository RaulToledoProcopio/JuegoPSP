using Godot;
using System;

public partial class Portal3 : Area2D
{
	
	void _on_body_entered(Node body)
	{
		// Comprobamos si el nodo que entra es un jugador y lo saltamos a la otra escena.
		if (body is Player player)
		{
			GetTree().CallDeferred("change_scene_to_file", "res://Menú/Final.tscn");
		}
	}
}
