using Godot;
using System;

public partial class Incoming : Control
{
	private Button continueButton;
	
	public override void _Ready()
	{
		
		continueButton = GetNode<Button>("Sprite2D/Button");
		continueButton.GrabFocus();
	}
	
	// Cambiar de escena al pulsar el primer botón del menú
	private void _on_button_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Final.tscn");
	}
}
	
