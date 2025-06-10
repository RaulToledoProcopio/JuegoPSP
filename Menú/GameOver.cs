using Godot;
using System;

public partial class GameOver : Control
{
	// Cambiar de escena al pulsar el primer botón del menú
	private void _on_button_pressed(){
		var crono = GetNode<Crono>("/root/Crono");
		crono.StopTimer(submitScore: false);
		crono.ResetTimer();
		GameState.Reset();
		GetTree().ChangeSceneToFile("res://Nivel/Nivel1/Nivel1.tscn");
	}
	
	// Cambiar de escena al pulsar el segundo botón del menú
	private void _on_button_2_pressed(){
		var crono = GetNode<Crono>("/root/Crono");
		crono.StopTimer(submitScore: false);
		crono.ResetTimer();
		crono.HideTimer();
		GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
	}
	
	// Cambiar de escena al pulsar el tercer botón del menú
	private void _on_button_3_pressed(){
		GetTree().Quit();
	}
}
