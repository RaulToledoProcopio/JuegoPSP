using Godot;

public partial class Portal7 : Area2D
{
	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			var audioManager = GetNode<AudioManager>("/root/AudioManager");
			audioManager.PlayForLevel(8);
			
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
			
			GetTree().CallDeferred("change_scene_to_file",
				"res://Nivel/Nivel8/Nivel8.tscn");
		}
	}
}
