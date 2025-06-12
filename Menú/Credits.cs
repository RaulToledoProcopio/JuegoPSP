using Godot;
using System;

public partial class Credits : Control
{
	[Export] public float ScrollSpeed = 50f;

	private Label _creditsLabel;
	private AudioStreamPlayer _musicPlayer;
	private float _startY;

	public override void _Ready()
	{
		_creditsLabel = GetNode<Label>("CreditsContainer/Label");
		_musicPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		_musicPlayer.Play();
	}

	public override void _Process(double delta)
	{
		if (_creditsLabel == null)
			return;

		// Mueve el Label hacia arriba
		var pos = _creditsLabel.Position;
		pos.Y -= (float)(ScrollSpeed * delta);
		_creditsLabel.Position = pos;

		// Cuando el último píxel del texto haya salido por completo:
		if (pos.Y + _creditsLabel.Size.Y < 0)
			GetTree().ChangeSceneToFile("res://Menú/Final.tscn");
	}
}
