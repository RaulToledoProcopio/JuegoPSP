using Godot;
using System;
using System.Text.Json;

public partial class SaveManager : Node
{
	public void SaveGame(string username, string levelPath)
	{
		var saveData = new SaveData
		{
			Health = GameState.Health,
			Ammo = GameState.Ammo,
			LevelPath = levelPath
		};

		string json = JsonSerializer.Serialize(saveData);
		string savePath = $"user://save_{username}.json";
		using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
		file.StoreString(json);
		GD.Print("Juego guardado en: " + savePath);
	}

	public SaveData LoadGame(string username)
	{
		string savePath = $"user://save_{username}.json";
		if (!FileAccess.FileExists(savePath))
			return null;

		using var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
		string json = file.GetAsText();
		return JsonSerializer.Deserialize<SaveData>(json);
	}

	public void DeleteGame(string username)
	{
		var dir = DirAccess.Open("user://");
		if (dir != null && dir.FileExists($"save_{username}.json"))
			dir.Remove($"save_{username}.json");
	}
}

public class SaveData
{
	public float Health { get; set; }
	public int Ammo { get; set; }
	public string LevelPath { get; set; } = "";
}
