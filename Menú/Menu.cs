using Godot;
using System;

public partial class Menu : Godot.Control
{
	private Button continueButton;
	
	public override void _Ready()
{
	var audioManager = GetNode<AudioManager>("/root/AudioManager");
	audioManager.PlayForLevel(0);

	continueButton = GetNode<Button>("VBoxContainer/Button");

	var saveData = GetCurrentUserSaveData();

	if (saveData == null)
	{
		continueButton.Disabled = true;
		GD.Print("No hay partida guardada para este usuario.");
	}
}

	
	private SaveData GetCurrentUserSaveData()
{
	var session = GetNode<SessionManager>("/root/SessionManager");
	var saveManager = GetNode<SaveManager>("/root/SaveManager");
	return saveManager.LoadGame(session.Username);
}

	
	private void _on_button_pressed()
{
	var saveData = GetCurrentUserSaveData();

	if (saveData != null)
	{
		GameState.Health = saveData.Health;
		GameState.Ammo = saveData.Ammo;
		GameState.CurrentUI?.UpdateHealth(GameState.Health);
		GameState.CurrentUI?.UpdateAmmo(GameState.Ammo);
		GetTree().ChangeSceneToFile(saveData.LevelPath);
	}
	else
	{
		continueButton.Disabled = true;
	}
}
	
	// Cambiar de escena al pulsar el primer botón del menú	
	private void _on_button_2_pressed(){
		GameState.Reset();
		GetTree().ChangeSceneToFile("res://Nivel/Nivel1/Nivel1.tscn");
	}
	
	private void _on_button_3_pressed(){
		GetTree().ChangeSceneToFile("res://Menú/Opciones.tscn");
	}
	
	private void _on_button_4_pressed(){
		GetTree().Quit();
	}
}
