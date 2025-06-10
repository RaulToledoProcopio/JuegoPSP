using Godot;
using System;

public partial class Portal1 : Area2D
{
	void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			var audioManager = GetNode<AudioManager>("/root/AudioManager");
			audioManager.PlayForLevel(2);
			
			var session = GetNode<SessionManager>("/root/SessionManager");
			var saveManager = GetNode<SaveManager>("/root/SaveManager");
			saveManager.SaveGame(session.Username, GetTree().CurrentScene.SceneFilePath);
			
			if (!string.IsNullOrEmpty(session.Username))
			{
				saveManager.SaveGame(session.Username, GetTree().CurrentScene.SceneFilePath);
			}
			else
			{
				GD.PrintErr("No hay usuario logueado, no se guarda partida.");
			}

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
