using Godot;
using System;

public partial class Pause : Control
{
	public override void _Ready()
	{
		Visible = false; // Desactivamos la visibilidad del menú de pausa cuando comienza el juego.
	}
	
	// Este método se llama cuando el sistema recibe un evento de entrada.
	public override void _Input(InputEvent @event)
	{
		// Verificamos si la acción definida como ui_pause ha sido presionada.
		if (@event.IsActionPressed("ui_pause"))
		{
			bool isPaused = GetTree().Paused; // Obtenemos el estado actual de la pausa del árbol de nodos.
			GetTree().Paused = !isPaused; // Cambiamos el estado de la pausa.
			SetProcessInput(true);
			// Si el juego está en pausa, el menú de pausa será visible.
			Visible = GetTree().Paused;
		}
	}
}
