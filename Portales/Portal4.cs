using Godot;

public partial class Portal4 : Area2D
{
	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			
			GetTree().CallDeferred("change_scene_to_file",
				"res://Nivel/Nivel5/Nivel5.tscn");
		}
	}
}
