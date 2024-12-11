using Godot;
using System;

public partial class Npc : Node2D
{
	
	public RichTextLabel texto; // RichTextLabel con el texto del mensaje.
	
	public override void _Ready()
	{
		texto = GetNode<RichTextLabel>("AnimatedSprite2D/Label"); // Referencia al nodo del Label.
	}
}
