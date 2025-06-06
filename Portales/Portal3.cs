using Godot;

public partial class Portal3 : Area2D
{
	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			var audioManager = GetNode<AudioManager>("/root/AudioManager");
				audioManager.PlayForLevel(4);
			
			GetTree().CallDeferred("change_scene_to_file",
				"res://Nivel/Nivel4/Nivel4.tscn");
		}
	}
}
