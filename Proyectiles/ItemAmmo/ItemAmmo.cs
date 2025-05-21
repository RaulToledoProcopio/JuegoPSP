using Godot;
using System;

public partial class ItemAmmo : Area2D
{
	private AudioStreamPlayer itemAmmoSoundPlayer;

	public override void _Ready()
	{
		itemAmmoSoundPlayer = GetNode<AudioStreamPlayer>("ItemAmmo");
	}

	private void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			GameState.ChangeAmmo(5);

			// Reproducir el sonido
			itemAmmoSoundPlayer.Play();

			// Ocultar y desactivar el ítem mientras suena el sonido
			Visible = false;
			SetProcess(false);
			SetPhysicsProcess(false);
			CollisionLayer = 0;
			CollisionMask = 0;

			// Esperar a que termine el sonido para liberar
			itemAmmoSoundPlayer.Finished += () => QueueFree();
		}
	}
}
