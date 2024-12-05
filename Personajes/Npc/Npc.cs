using Godot;
using System;

public partial class Npc : Node2D
{
	
	public Label texto; // Label con el texto del mensaje.
	
	public override void _Ready()
	{
		texto = GetNode<Label>("AnimatedSprite2D/Label"); // Referencia al nodo del Label.
	}
}
