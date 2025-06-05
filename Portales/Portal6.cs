using Godot;

public partial class Portal6 : Area2D
{
	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			var audioManager = GetNode<AudioManager>("/root/AudioManager");
				audioManager.PlayForLevel(3);
			
			GetTree().CallDeferred("change_scene_to_file",
				"res://Nivel/Nivel7/Nivel7.tscn");
		}
	}
}
