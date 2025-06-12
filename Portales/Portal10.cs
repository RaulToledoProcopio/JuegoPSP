using Godot;
using System;

public partial class Portal10 : Area2D
{
	
	void _on_body_entered(Node body)
	{
		if (body is Player player)
		{
			
			var audioManager = GetNode<AudioManager>("/root/AudioManager");
			audioManager.StopMusic();
			
			Crono crono = GetNode<Crono>("/root/Crono");
			crono.SetCanSubmitScore(true);
			crono.StopTimer(submitScore: true);
			
			float totalTime = crono.GetElapsedTime();
			GD.Print("Tiempo total en niveles: " + totalTime + " segundos.");
			
			var session = GetNode<SessionManager>("/root/SessionManager");
			var saveManager = GetNode<SaveManager>("/root/SaveManager");
			if (!string.IsNullOrEmpty(session.Username))
			{
				saveManager.DeleteGame(session.Username);
				GD.Print("Partida borrada al completar el juego.");
			}
			else
			{
				GD.PrintErr("No hay usuario logueado, no se pudo borrar la partida.");
				}
			
			GetTree().CallDeferred("change_scene_to_file", "res://Menú/Credits.tscn");
		}
	}
}
