using Godot;
using System;

public partial class Mensaje1 : Area2D
{
	private Npc mensaje1;
	
	public override void _Ready()
	{
		// Obtenemos el nodo del Npc.
		mensaje1 = GetNode<Npc>("../Npc");
	}
	
	/* Este método es para comprobar que el nodo que entra en el área 2p que
	procea el mensaje del NPC sea un player */
	
	private void _on_body_entered(CharacterBody2D body)
	{
		// Comprobamos si el cuerpo que entra en el área es un player.
		if (body is Player player)
		{
			mensaje1.texto.Visible = true; // Mostramos el mensaje del NPC.
		}	
	}
}
