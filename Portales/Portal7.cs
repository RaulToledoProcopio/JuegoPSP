using Godot;

public partial class Portal7 : Area2D
{
	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			var audioManager = GetNode<AudioManager>("/root/AudioManager");
				audioManager.PlayForLevel(8);
			
			GetTree().CallDeferred("change_scene_to_file",
				"res://Nivel/Nivel8/Nivel8.tscn");
		}
	}
}
