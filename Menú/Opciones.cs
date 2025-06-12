using Godot;
using System;

public partial class Opciones : Control
{
	[Export] public NodePath AudioManagerPath;
	[Export] public Panel Panel { get; set; }
	[Export] public HSlider HSlider { get; set; }
	[Export] public CheckBox CheckBox { get; set; }

	private AudioManager _audioManager;

	public override void _Ready()
	{
		_audioManager = GetNode<AudioManager>("/root/AudioManager");

		// Ocultamos el panel
		Panel.Visible = false;

		// Aplicamos el volumen inicial basado en el slider
		ApplySliderVolume((float)HSlider.Value);

		// Inicializamos el mute
		CheckBox.ButtonPressed = _audioManager.Muted;
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

		if (!isMuted)
		{
			// si desmuteamos, volvemos a aplicar el volumen actual del slider
			ApplySliderVolume((float)HSlider.Value);
		}
	}

	private void ApplySliderVolume(float sliderValue)
	{
		float linear = sliderValue / 100f;
		// convertir lineal a dB
		float db = linear <= 0f ? -80f : 20f * (float)Math.Log10(linear);
		_audioManager.VolumeDb = db;
	}
	public void _on_button_pressed()     => Panel.Visible = true;
	public void _on_button_2_pressed()   => GetTree().ChangeSceneToFile("res://Menú/Menu.tscn");
	public void _on_button_3_pressed()   => Panel.Visible = false;
}
