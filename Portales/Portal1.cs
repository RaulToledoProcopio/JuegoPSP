using Godot;
using System;

public partial class Portal1 : Area2D
{
	void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			
			Crono crono = GetNode<Crono>("/root/Crono");
			crono.StartTimer();
		
			var tiempoLabel = GetNode<Label>("/root/CronometroUI/CronoUI");

			if (tiempoLabel != null)
			{
				GD.Print("Mostrando cronómetro...");
				tiempoLabel.Visible = true;
			}
			
			GetTree().CallDeferred("change_scene_to_file", "res://Nivel/Nivel2/Nivel2.tscn");
		}
	}
}
