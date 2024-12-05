using Godot;
using System;

public partial class ItemHeal : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	void _on_body_entered(Node body)
	{
		// Comprobamos si el nodo que entra es el jugador (o cualquier nodo que quieras detectar)
		if (body is Player player)
		{
			if (player.hp < 100){
			// Recargar la munición del jugador al valor máximo
			player.hp += 25;
			}
			QueueFree();
		}
	}
}
