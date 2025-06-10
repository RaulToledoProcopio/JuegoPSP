using Godot;

public partial class Portal8 : Area2D
{
	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
				
			GetTree().CallDeferred("change_scene_to_file",
				"res://Nivel/Nivel9/Nivel9.tscn");
		}
	}
}
