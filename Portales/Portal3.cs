using Godot;
using System;

public partial class Portal3 : Area2D
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
			
			GetTree().CallDeferred("change_scene_to_file", "res://Menú/Final.tscn");
		}
	}
}
