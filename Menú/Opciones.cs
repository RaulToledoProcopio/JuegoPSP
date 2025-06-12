using Godot;
using System;

public partial class Opciones : Control
{
	[Export] public NodePath AudioManagerPath;
	[Export] public Panel Panel { get; set; }
	[Export] public HSlider HSlider { get; set; }
	[Export] public CheckBox CheckBox { get; set; }

	private AudioManager _audioManager;
	private Button continueButton;

	public override void _Ready()
	{
		_audioManager = GetNode<AudioManager>("/root/AudioManager");
		continueButton = GetNode<Button>("VBoxContainer/Button");

		// Ocultamos el panel
		Panel.Visible = false;

		// Leer valores guardados desde ProjectSettings
		float savedVolume = (float)ProjectSettings.GetSetting("audio/volume", 50.0);
		bool savedMuted = (bool)ProjectSettings.GetSetting("audio/muted", false);

		// Actualizar los controles visuales con los valores guardados
		HSlider.Value = savedVolume;
		CheckBox.ButtonPressed = savedMuted;

		// Aplicar configuración al audio
		if (!savedMuted)
			ApplySliderVolume(savedVolume);
		else
			_audioManager.Muted = true;
			
		continueButton.GrabFocus();
	}
	// Cuando movemos el slider
	public void OnVolumeChanged(double value)
	{
		if (!CheckBox.ButtonPressed)
		{
			ApplySliderVolume((float)value);
		}
	}

	public void OnMuteToggled(bool isMuted)
	{
		_audioManager.Muted = isMuted;

		ProjectSettings.SetSetting("audio/muted", isMuted);
		ProjectSettings.Save();

		if (!isMuted)
		{
			ApplySliderVolume((float)HSlider.Value);
		}
	}


	private void ApplySliderVolume(float sliderValue)
	{
		float linear = sliderValue / 100f;
		float db = linear <= 0f ? -50f : 20f * (float)Math.Log10(linear);
		_audioManager.VolumeDb = db;
	// Guardar volumen en ProjectSettings
	ProjectSettings.SetSetting("audio/volume", sliderValue);
	ProjectSettings.Save(); // Importante para que persista
	}

	public void _on_button_pressed()     => Panel.Visible = true;
	public void _on_button_2_pressed()   => GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
	public void _on_button_3_pressed()
	{
	
		Panel.Visible = false;
		continueButton.GrabFocus();
	}   
}
