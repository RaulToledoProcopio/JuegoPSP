using Godot;
using System;

public partial class Final : Godot.Control
{
	
	// Cambiar de escena al pulsar el primer botón del menú
	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://Nivel/Nivel1/Nivel1.tscn");
	}
	
	// Cambiar de escena al pulsar el segundo botón del menú
	private void _on_button_2_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
	}
	
	// Cambiar de escena al pulsar el tercer botón del menú
	private void _on_button_3_pressed(){
		GetTree().Quit();
	}
	
	// Cambiar de escena al pulsar el botón lo que está por venir
	private void _on_button_4_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Incoming.tscn");
	}
}
