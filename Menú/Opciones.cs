using Godot;
using System;

public partial class Opciones : Control
{
	
	// Cambiar de escena al pulsar el botón del menú
	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
	}
}
