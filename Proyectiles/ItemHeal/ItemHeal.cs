using Godot;
using System;

public partial class ItemHeal : Area2D
{
	private AudioStreamPlayer itemHealSoundPlayer;

	public override void _Ready()
	{
		itemHealSoundPlayer = GetNode<AudioStreamPlayer>("ItemHeal");
	}
	
	void _on_body_entered(Node body)
	{
		if (body is Player)
		{
			if (GameState.Health < 100)
				GameState.ChangeHp(25f);

			// Reproducir el sonido
			itemHealSoundPlayer.Play();

			// Ocultar y desactivar el ítem mientras suena el sonido
			Visible = false;
			SetProcess(false);
			SetPhysicsProcess(false);
			CollisionLayer = 0;
			CollisionMask = 0;

			// Esperar a que termine el sonido para liberar
			itemHealSoundPlayer.Finished += () => QueueFree();
		}
	}
}
