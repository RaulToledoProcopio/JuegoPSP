using Godot;
using System;

public partial class Menu : Godot.Control
{
	public override void _Ready()
	{
		var audioManager = GetNode<AudioManager>("/root/AudioManager");
		audioManager.PlayForLevel(0);
	}
	
	// Cambiar de escena al pulsar el primer botón del menú	
	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://Nivel/Nivel1/Nivel1.tscn");
	}
	
	// Cambiar de escena al pulsar el segundo botón del menú
	private void _on_button_2_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Opciones.tscn");
	}
	
	// Cambiar de escena al pulsar el tercer botón del menú
	private void _on_button_3_pressed(){
		GetTree().Quit();
	}
}
