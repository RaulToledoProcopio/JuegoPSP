using Godot;
using System;

public partial class ItemAmmo : Area2D
{
	
	// Método para recargar las dagas.
	void _on_body_entered(Node body)
	{
		// Comprobamos si el nodo que entra es un jugador.
		if (body is Player player)
		{
			player.Ammo += 5; // Aumentamos la munición del jugador en 5.
			QueueFree(); // Eliminamos el item.
		}
	}
}
