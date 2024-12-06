using Godot;
using System;

public partial class Incoming : Control
{
	
	// Cambiar de escena al pulsar el primer botón del menú
	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Final.tscn");
	}
}
	
